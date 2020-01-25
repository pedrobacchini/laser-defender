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
        private EnemyClass _enemyClass;
        private float _health;
        private Camera _mainCamera;
        private SpriteRenderer _spriteRenderer;
        private Color _startColor;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private Transform _transform;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _startColor = _spriteRenderer.color;
        }

        public void StartEnemy(EnemyClass enemyClass)
        {
            _mainCamera = Camera.main;
            _enemyClass = enemyClass;
            _transform.localScale = _enemyClass.Size;
            _health = _enemyClass.MaxHealth;
            _spriteRenderer.sprite = _enemyClass.Sprite;
            _spriteRenderer.color = _startColor;
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
            GameMaster.AddScore(_enemyClass.ScoreValue);
            SelfDestroy();
            var explosion = Instantiate(_enemyClass.DeathPrefab, transform.position, transform.rotation);
            Destroy(explosion, _enemyClass.DurationOfExplosion);
            AudioSource.PlayClipAtPoint(_enemyClass.DeathSound, _mainCamera.transform.position,
                Random.Range(_enemyClass.DeathSoundVolume.x, _enemyClass.DeathSoundVolume.y));
        }

        private void OnDisable()
        {
            _disposables.Clear();
        }

        public void SelfDestroy()
        {
            EnemyRuntimeSet.Remove(this);
            gameObject.SetActive(false);
        }
    }
}