using System;
using SingletonScriptableObject;
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

        [Title("Visual Effects")]
        [OdinSerialize]
        private GameObject DeathPrefab { get; set; }

        [OdinSerialize] private float DurationOfExplosion { get; set; } = 0.8f;

        [Title("Sound Effects")]
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
                .Select(_ => _spriteRenderer.color = Color.red)
                .Delay(TimeSpan.FromMilliseconds(40))
                .Subscribe(_ => _spriteRenderer.color = _startColor)
                .AddTo(_disposables);
            if (Health <= 0) Die();
        }

        private void Die()
        {
            GameSession.AddScore(ScoreValue);
            SelfDestroy	();
            var explosion = Instantiate(DeathPrefab, transform.position, transform.rotation);
            Destroy(explosion, DurationOfExplosion);
            AudioSource.PlayClipAtPoint(DeathSound, _mainCamera.transform.position,
                Random.Range(DeathSoundVolume.x, DeathSoundVolume.y));
        }

        private void OnDestroy()
        {
            _disposables.Clear();
        }

        public void SelfDestroy()
        {
            EnemyRuntimeSet.Remove(this);
            Destroy(gameObject);
        }
    }
}