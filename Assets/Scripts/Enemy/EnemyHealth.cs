using System;
using System.Diagnostics.CodeAnalysis;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Enemy
{
    public class EnemyHealth
    {
        public FloatReactiveProperty CurrentHealth { get; } = new FloatReactiveProperty();

        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private Color _startColor;
        private SpriteRenderer _spriteRenderer;

        public void Awake(SpriteRenderer spriteRenderer)
        {
            _spriteRenderer = spriteRenderer;
            _startColor = _spriteRenderer.color;
        }

        [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
        public void StartEnemyHealth(GameObject gameObject, float MaxHealth, Action death)
        {
            CurrentHealth.Value = MaxHealth;
            _spriteRenderer.color = _startColor;

            gameObject.OnTriggerEnter2DAsObservable()
                .Select(other => other.gameObject.GetComponent<DamageDealer>())
                .Where(damageDealer => damageDealer != null)
                .Subscribe(damageDealer =>
                {
                    damageDealer.Hit();
                    CurrentHealth.Value -= damageDealer.Damage;
                    // Damage effect
                    _disposables.Clear();
                    gameObject.UpdateAsObservable()
                        .Select(_ => _spriteRenderer.color = Color.red)
                        .Delay(TimeSpan.FromMilliseconds(40))
                        .Subscribe(_ => _spriteRenderer.color = _startColor)
                        .AddTo(_disposables);
                    if (CurrentHealth.Value > 0) return;
                    death();
                });

            gameObject.OnDisableAsObservable()
                .Subscribe(_ => _disposables.Clear());
        }
    }
}