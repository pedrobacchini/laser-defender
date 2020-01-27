using System;
using System.Diagnostics.CodeAnalysis;
using SingletonScriptableObject;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemyHealth
    {
        public FloatReactiveProperty CurrentHealth { get; } = new FloatReactiveProperty();

        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeCameraMain")]
        public void StartEnemyHealth(GameObject gameObject, float MaxHealth, SpriteRenderer spriteRenderer,
            Color startColor, int scoreValue, GameObject deathPrefab, float durationOfExplosion, AudioClip deathSound,
            Vector2 deathSoundVolume, Action selfDestroy)
        {
            var mainCamera = Camera.main;
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
                    if (CurrentHealth.Value > 0) return;
                    //Die 
                    GameMaster.AddScore(scoreValue);
                    selfDestroy();
                    var explosion = Object.Instantiate(deathPrefab, gameObject.transform.position,
                        gameObject.transform.rotation);
                    Object.Destroy(explosion, durationOfExplosion);
                    AudioSource.PlayClipAtPoint(deathSound, mainCamera.transform.position,
                        Random.Range(deathSoundVolume.x, deathSoundVolume.y));
                });

            gameObject.OnDisableAsObservable()
                .Subscribe(_ => _disposables.Clear());
        }
    }
}