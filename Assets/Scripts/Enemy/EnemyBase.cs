using System;
using SingletonScriptableObject;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public abstract class EnemyBase : SerializedMonoBehaviour
    {
        public FloatReactiveProperty CurrentHealth { get; } = new FloatReactiveProperty();
        protected Camera _mainCamera;
        protected SpriteRenderer _spriteRenderer;
        protected Color _startColor;
        protected Transform _transform;
        protected int _scoreValue;
        protected GameObject _deathPrefab;
        protected float _durationOfExplosion;
        protected AudioClip _deathSound;
        protected Vector2 _deathSoundVolume;

        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private void Awake()
        {
            _transform = GetComponent<Transform>();
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
            CurrentHealth.Value -= damageDealer.Damage;
            // Damage effect
            _disposables.Clear();
            this.UpdateAsObservable()
                .Select(_ => _spriteRenderer.color = Color.red)
                .Delay(TimeSpan.FromMilliseconds(40))
                .Subscribe(_ => _spriteRenderer.color = _startColor)
                .AddTo(_disposables);
            if (CurrentHealth.Value <= 0) Die();
        }

        private void Die()
        {
            GameMaster.AddScore(_scoreValue);
            SelfDestroy();
            var explosion = Instantiate(_deathPrefab, transform.position, transform.rotation);
            Destroy(explosion, _durationOfExplosion);
            AudioSource.PlayClipAtPoint(_deathSound, _mainCamera.transform.position,
                Random.Range(_deathSoundVolume.x, _deathSoundVolume.y));
        }

        private void OnDisable()
        {
            _disposables.Clear();
        }

        public virtual void SelfDestroy()
        {
            gameObject.SetActive(false);
        }
    }
}