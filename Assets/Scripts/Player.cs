using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : SerializedMonoBehaviour
{
    [Title("Player")] [OdinSerialize] private float Speed { get; set; } = 10;
    [OdinSerialize] private int Health { get; set; }

    [Title("Laser")] [OdinSerialize] private GameObject LaserPrefab { get; set; }
    [OdinSerialize] private float LaserSpeed { get; set; } = 15;
    [OdinSerialize] private float LaserFiringPeriod { get; set; } = 0.1f;

    [OdinSerialize] private bool ShootingPaused { get; set; } = false;

    [Title("SFX")] [OdinSerialize] private AudioClip ShootingClip { get; set; }
    [MinMaxSlider(0, 1)] [OdinSerialize] private Vector2 ShootingVolume { get; set; }
    [OdinSerialize] private AudioClip DeathClip { get; set; }
    [MinMaxSlider(0, 1)] [OdinSerialize] private Vector2 DeathClipVolume { get; set; }

    private Boundary _boundary;
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
        SetUpMoveBoundaries();

        var axisObservable = this.FixedUpdateAsObservable()
            .Select(_ => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));

        // Movement inputs tick on FixedUpdate
        axisObservable
            .Select(axis =>
            {
                var currentPosition = transform.position;
                var newXPos = Mathf.Clamp(currentPosition.x + axis.x * Time.deltaTime * Speed, _boundary.XMin,
                    _boundary.XMax);
                var newYPos = Mathf.Clamp(currentPosition.y + axis.y * Time.deltaTime * Speed, _boundary.YMin,
                    _boundary.YMax);
                return new Vector2(newXPos, newYPos);
            })
            .Subscribe(newPosition => transform.position = newPosition)
            .AddTo(this);

        // Stopped Shooting
        axisObservable
            .Where(_ => ShootingPaused)
            .Sample(TimeSpan.FromSeconds(LaserFiringPeriod))
            .Where(axis => axis.Equals(Vector2.zero))
            .Subscribe(_ => Fire())
            .AddTo(this);

        // Fire1 Shooting
        Observable.EveryUpdate()
            .Where(_ => !ShootingPaused)
            .Sample(TimeSpan.FromSeconds(LaserFiringPeriod))
            .Where(_ => Input.GetButton("Fire1"))
            .Subscribe(_ => Fire())
            .AddTo(this);
    }

    private void Fire()
    {
        var laser = Instantiate(LaserPrefab, transform.position, Quaternion.identity);
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, LaserSpeed);
        AudioSource.PlayClipAtPoint(ShootingClip, _mainCamera.transform.position,
            Random.Range(ShootingVolume.x, ShootingVolume.y));
    }

    private void SetUpMoveBoundaries()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            throw new ArgumentException("Missing Sprite Renderer " + spriteRenderer.name);

        var gameCamera = Camera.main;
        if (gameCamera == null)
            throw new ArgumentException("Put main camera in Scene");

        var zeroPoint = gameCamera.ViewportToWorldPoint(new Vector2(0, 0));
        var onePoint = gameCamera.ViewportToWorldPoint(new Vector2(1, 1));

        var size = spriteRenderer.size / 2;

        _boundary = new Boundary(
            zeroPoint.x + size.x,
            onePoint.x - size.x,
            zeroPoint.y + size.y,
            onePoint.y - size.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var damageDealer = other.GetComponent<DamageDealer>();
        if (damageDealer) ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        damageDealer.Hit();
        Health -= damageDealer.Damage;
        if (Health <= 0) Die();
    }

    private void Die()
    {
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(DeathClip, _mainCamera.transform.position,
            Random.Range(DeathClipVolume.x, DeathClipVolume.y));
    }
}