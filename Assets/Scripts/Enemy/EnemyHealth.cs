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

        [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
        public void StartEnemyHealth(GameObject gameObject, float MaxHealth, SpriteRenderer spriteRenderer,
            Color startColor)
        {
            CurrentHealth.Value = MaxHealth;

            gameObject.OnTriggerEnter2DAsObservable()
                .Select(other => other.gameObject.GetComponent<DamageDealer>())
                .Subscribe(damageDealer =>
                {
                    damageDealer.Hit();
                    CurrentHealth.Value -= damageDealer.Damage;
                    // Damage effect
                    _disposables.Clear();
                    gameObject.UpdateAsObservable()
                        .Select(_ => spriteRenderer.color = Color.red)
                        .Delay(TimeSpan.FromMilliseconds(40))
                        .Subscribe(_ => spriteRenderer.color = startColor)
                        .AddTo(_disposables);
                });

            gameObject.OnDisableAsObservable()
                .Subscribe(_ => _disposables.Clear());
        }
    }
}