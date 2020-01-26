using System;
using SingletonScriptableObject;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace DefaultNamespace
{
    public class Shield : SerializedMonoBehaviour
    {
        [OdinSerialize] private Sprite SecondTurnSprite { get; set; }
        private Sprite _firstTurnSprite;
        private float _currentHealth;
        private SpriteRenderer _spriteRenderer;
        private Color _startColor;
        private float _healthSecondTurn;

        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _startColor = _spriteRenderer.color;
            _firstTurnSprite = _spriteRenderer.sprite;
        }

        public void StartShield(float maxHealthShield)
        {
            gameObject.SetActive(true);
            _currentHealth = maxHealthShield * GameMaster.Level.Value;
            _healthSecondTurn = _currentHealth / 2;
            _spriteRenderer.sprite = _firstTurnSprite;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var damageDealer = other.gameObject.GetComponent<DamageDealer>();
            if (damageDealer) ProcessHit(damageDealer);
        }

        private void ProcessHit(DamageDealer damageDealer)
        {
            damageDealer.Hit();
            _currentHealth -= damageDealer.Damage;
            if (_currentHealth <= _healthSecondTurn) _spriteRenderer.sprite = SecondTurnSprite;

                _disposables.Clear();
            this.UpdateAsObservable()
                .Select(_ => _spriteRenderer.color = Color.red)
                .Delay(TimeSpan.FromMilliseconds(40))
                .Subscribe(_ => _spriteRenderer.color = _startColor)
                .AddTo(_disposables);
            if (_currentHealth <= 0) gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _disposables.Clear();
        }
    }
}