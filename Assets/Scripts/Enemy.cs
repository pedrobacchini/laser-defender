using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : SerializedMonoBehaviour
{
    [Title("Enemy")] [OdinSerialize] private float Health { get; set; } = 100f;

    [Title("Laser")]
    [ReadOnly]
    [OdinSerialize]
    private float ShotCounter { get; set; }

    [OdinSerialize] private GameObject LaserPrefab { get; set; }
    [OdinSerialize] private float LaserSpeed { get; set; } = 8;
    [MinMaxSlider(0, 5)] [OdinSerialize] private Vector2 TimeBetweenShoots { get; set; }

    [Title("VFX")] [OdinSerialize] private GameObject DeathPrefab { get; set; }
    [OdinSerialize] private float DurationOfExplosion { get; set; } = 0.8f;
    
    [Title("SFX")] [OdinSerialize] private AudioClip ShootingClip { get; set; }
    
    [MinMaxSlider(0, 1)] [OdinSerialize] private Vector2 ShootingVolume { get; set; }
    [OdinSerialize] private AudioClip DeathClip { get; set; }
    [MinMaxSlider(0, 1)] [OdinSerialize] private Vector2 DeathClipVolume { get; set; }

    private Camera _mainCamera;
    
    private void Start()
    {
        _mainCamera = Camera.main;
        ResetShotCounter();
        this.UpdateAsObservable()
            .Where(_ => ShotCounter <= 0f)
            .Subscribe(_ =>
            {
                Fire();
                ResetShotCounter();
            });
    }

    private void Update()
    {
        ShotCounter -= Time.deltaTime;
    }

    private void ResetShotCounter()
    {
        ShotCounter = Time.deltaTime + Random.Range(TimeBetweenShoots.x, TimeBetweenShoots.y);
    }

    private void Fire()
    {
        var position = transform.position;
        var laser = Instantiate(LaserPrefab, new Vector2(position.x, position.y - 1), Quaternion.identity);
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -LaserSpeed);
        AudioSource.PlayClipAtPoint(ShootingClip, _mainCamera.transform.position, 
            Random.Range(ShootingVolume.x, ShootingVolume.y));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var damageDealer = other.gameObject.GetComponent<DamageDealer>();
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
        var transform1 = transform;
        var explosion = Instantiate(DeathPrefab, transform1.position, transform1.rotation);
        Destroy(explosion, DurationOfExplosion);
        AudioSource.PlayClipAtPoint(DeathClip, Camera.main.transform.position, Random.Range(DeathClipVolume.x, DeathClipVolume.y));
    }
}