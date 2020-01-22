using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class Enemy : SerializedMonoBehaviour
    {
        [Title("Enemy Stats")] [OdinSerialize] private float Health { get; set; } = 100f;

        [OdinSerialize] private int ScoreValue { get; set; } = 100;

        [Title("Shooting")]
        [ReadOnly]
        [OdinSerialize]
        private float ShotCounter { get; set; }

        [OdinSerialize] private GameObject ShootPrefab { get; set; }
        [OdinSerialize] private float ShootSpeed { get; set; } = 8;
        [MinMaxSlider(0, 5)] [OdinSerialize] private Vector2 TimeBetweenShoots { get; set; }

        [Title("Visual Effects")]
        [OdinSerialize]
        private GameObject DeathPrefab { get; set; }

        [OdinSerialize] private float DurationOfExplosion { get; set; } = 0.8f;

        [Title("Sound Effects")]
        [OdinSerialize]
        private AudioClip ShootingSound { get; set; }

        [MinMaxSlider(0, 1)] [OdinSerialize] private Vector2 ShootingVolume { get; set; }
        [OdinSerialize] private AudioClip DeathSound { get; set; }
        [MinMaxSlider(0, 1)] [OdinSerialize] private Vector2 DeathSoundVolume { get; set; }

        private Camera _mainCamera;
        private SpriteRenderer _spriteRenderer;
        private Color _startColor;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private void Start()
        {
            _mainCamera = Camera.main;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _startColor = _spriteRenderer.color;
            ShotCounter = Random.Range(TimeBetweenShoots.x, TimeBetweenShoots.y);
            this.UpdateAsObservable()
                .Sample(TimeSpan.FromSeconds(ShotCounter))
                .Subscribe(_ => Shoot())
                .AddTo(this);
        }

        private void Shoot()
        {
            var position = transform.position;
            var laser = Instantiate(ShootPrefab, new Vector2(position.x, position.y - 1), Quaternion.identity);
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -ShootSpeed);
            AudioSource.PlayClipAtPoint(ShootingSound, _mainCamera.transform.position,
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
            // Damage effect
            _disposables.Clear();
            this.UpdateAsObservable()
                .Select(_ => _spriteRenderer.color = new Color(166.0f/255.0f, 33.0f / 255.0f, 33.0f / 255.0f))
                .Delay(TimeSpan.FromMilliseconds(100))
                .Subscribe(_ => _spriteRenderer.color = _startColor)
                .AddTo(_disposables);
            if (Health <= 0) Die();
        }

        private void Die()
        {
            GameSession.GameSession.AddScore(ScoreValue);
            Destroy(gameObject);
            var transform1 = transform;
            var explosion = Instantiate(DeathPrefab, transform1.position, transform1.rotation);
            Destroy(explosion, DurationOfExplosion);
            AudioSource.PlayClipAtPoint(DeathSound, _mainCamera.transform.position,
                Random.Range(DeathSoundVolume.x, DeathSoundVolume.y));
        }

        private void OnDestroy()
        {
            _disposables.Clear();
        }
    }
}