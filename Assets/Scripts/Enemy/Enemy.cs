using System.Diagnostics.CodeAnalysis;
using SingletonScriptableObject;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Enemy
{
    public class Enemy : SerializedMonoBehaviour
    {
        private readonly EnemyShooting _enemyShooting = new EnemyShooting();
        private readonly EnemyPathing _enemyPathing = new EnemyPathing();
        private readonly EnemyHealth _enemyHealth = new EnemyHealth();

        private Transform _transform;
        private SpriteRenderer _spriteRenderer;
        private Color _startColor;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _startColor = _spriteRenderer.color;
        }

        [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
        public void StartEnemy(WaveConfig waveConfig)
        {
            _transform.localScale = waveConfig.EnemyClass.Size;
            _spriteRenderer.sprite = waveConfig.EnemyClass.Sprite;
            _spriteRenderer.color = _startColor;
            _enemyPathing.StartEnemyPathing(waveConfig, gameObject, SelfDestroy);
            var enemyClass = waveConfig.EnemyClass;
            _enemyShooting.StartEnemyShooting(enemyClass, gameObject);
            var MaxHealth = waveConfig.EnemyClass.MaxHealth * GameMaster.Level.Value;
            var ScoreValue = waveConfig.EnemyClass.ScoreValue * GameMaster.Level.Value;
            _enemyHealth.StartEnemyHealth(gameObject, MaxHealth, _spriteRenderer, _startColor, ScoreValue,
                enemyClass.DeathPrefab, enemyClass.DurationOfDeathEffect, enemyClass.DeathSound,
                enemyClass.DeathSoundVolume, SelfDestroy);
        }

        private void SelfDestroy()
        {
            EnemyRuntimeSet.Remove(this);
            gameObject.SetActive(false);
        }
    }
}