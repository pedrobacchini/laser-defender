using System;
using System.Collections;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Player : SerializedMonoBehaviour
{
    [OdinSerialize] private float Speed { get; set; } = 10;
    [OdinSerialize] private GameObject LaserPrefab { get; set; }
    [OdinSerialize] private float ProjectileSpeed { get; set; } = 20;
    [OdinSerialize] private float ProjectileFiringPeriod { get; set; } = 0.1f;

    private Boundary _boundary;

    private void Start()
    {
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
            .Subscribe(newPosition => transform.position = newPosition);

        var isShooting = false;

        axisObservable
            .Select(axis => !axis.Equals(Vector2.zero))
            .Subscribe(isMoving =>
            {
                if (!isMoving && !isShooting)
                {
                    _firingCoroutine = StartCoroutine(FireContinuously());
                    isShooting = true;
                }

                if (isMoving && isShooting)
                {
                    StopCoroutine(_firingCoroutine);
                    isShooting = false;
                }
            });

//        this.UpdateAsObservable()
//            .Where(_ => Input.GetButton("Fire1"))
//            .Select(_ => Instantiate(LaserPrefab, transform.position, Quaternion.identity))
//            .Select(laser => laser.GetComponent<Rigidbody2D>())
//            .Select(rigidbody2dLaser => rigidbody2dLaser.velocity = new Vector2(0, ProjectileSpeed))
//            .Subscribe();

//            .TimeInterval(TimeSpan.FromSeconds(ProjectileFiringPeriod))
//            .Delay(TimeSpan.FromSeconds(ProjectileFiringPeriod))
//            .Subscribe();

//        this.UpdateAsObservable()
//            .Where(_ => Input.GetButtonDown("Fire1"))
//            .Subscribe(_ => _firingCoroutine = StartCoroutine(FireContinuously()));
//        
//        this.UpdateAsObservable()
//            .Where(_ => Input.GetButtonUp("Fire1"))
//            .Subscribe(_ => StopCoroutine(_firingCoroutine));
    }

    private Coroutine _firingCoroutine;

//    private void Update()
//    {
//        if (Input.GetButtonDown("Fire1"))
//        {
//            _firingCoroutine = StartCoroutine(FireContinuously());
//        }
//        if (Input.GetButtonUp("Fire1"))
//        {
//            StopCoroutine(_firingCoroutine);
//        }
//    }

    private IEnumerator FireContinuously()
    {
        while (true)
        {
            var laser = Instantiate(LaserPrefab, transform.position, Quaternion.identity);
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, ProjectileSpeed);
            yield return new WaitForSeconds(ProjectileFiringPeriod);
        }
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
}