using System.Diagnostics.CodeAnalysis;
using SingletonScriptableObject;
using UnityEngine;

namespace Enemy
{
    public class Enemy : EnemyBase
    {
        private readonly EnemyShooting _enemyShooting = new EnemyShooting();
        private readonly EnemyPathing _enemyPathing = new EnemyPathing();
        private readonly EnemyHealth _enemyHealth = new EnemyHealth();

        private Transform _transform;
        private SpriteRenderer _spriteRenderer;
        private EnemyClass _enemyClass;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _enemyHealth.Awake(_spriteRenderer);
        }

        [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
        public void StartEnemy(WaveConfig waveConfig)
        {
            _enemyClass = waveConfig.EnemyClass;
            _transform.localScale = waveConfig.EnemyClass.Size;
            _spriteRenderer.sprite = waveConfig.EnemyClass.Sprite;
            _enemyPathing.StartEnemyPathing(waveConfig.WaveWayPoints, waveConfig.MoveSpeed, gameObject, Destroy);
            _enemyShooting.StartEnemyShooting(_enemyClass, gameObject);
            _enemyHealth.StartEnemyHealth(gameObject, _enemyClass.MaxHealth, Death);
        }

        private void Death()
        {
            GameMaster.AddScore(_enemyClass.ScoreValue);
            DeathEffect(_enemyClass.DeathPrefab, _enemyClass.DurationOfDeathEffect, 
                _enemyClass.DeathSound, _enemyClass.DeathSoundVolume);
            Destroy();
        }

        private void Destroy()
        {
            EnemyRuntimeSet.Remove(this);
            gameObject.SetActive(false);
        }
    }
}