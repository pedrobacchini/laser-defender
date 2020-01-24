using System;
using SingletonScriptableObject;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class Enemy : SerializedMonoBehaviour
    {
        public EnemyClass EnemyClass { private get; set; }

        private float _health;
        private Camera _mainCamera;
        private SpriteRenderer _spriteRenderer;
        private Color _startColor;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private void Start()
        {
            GetComponent<Transform>().localScale = EnemyClass.Size;
            _health = EnemyClass.MaxHealth;
            _mainCamera = Camera.main;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = EnemyClass.Sprite;
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
            _health -= damageDealer.Damage;
            // Damage effect
            _disposables.Clear();
            this.UpdateAsObservable()
                .Select(_ => _spriteRenderer.color = Color.red)
                .Delay(TimeSpan.FromMilliseconds(40))
                .Subscribe(_ => _spriteRenderer.color = _startColor)
                .AddTo(_disposables);
            if (_health <= 0) Die();
        }

        private void Die()
        {
            GameMaster.AddScore(EnemyClass.ScoreValue);
            SelfDestroy();
            var explosion = Instantiate(EnemyClass.DeathPrefab, transform.position, transform.rotation);
            Destroy(explosion, EnemyClass.DurationOfExplosion);
            AudioSource.PlayClipAtPoint(EnemyClass.DeathSound, _mainCamera.transform.position,
                Random.Range(EnemyClass.DeathSoundVolume.x, EnemyClass.DeathSoundVolume.y));
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