using UnityEngine;

namespace Enemy
{
    public class Enemy : EnemyBase
    {
        public void StartEnemy(EnemyClass enemyClass)
        {
            _mainCamera = Camera.main;
            _spriteRenderer.color = _startColor;
            _transform.localScale = enemyClass.Size;
            CurrentHealth.Value = enemyClass.MaxHealth;
            _spriteRenderer.sprite = enemyClass.Sprite;
            _scoreValue = enemyClass.ScoreValue;
            _deathPrefab = enemyClass.DeathPrefab;
            _durationOfExplosion = enemyClass.DurationOfDeathEffect;
            _deathSound = enemyClass.DeathSound;
            _deathSoundVolume = enemyClass.DeathSoundVolume;
        }

        public override void SelfDestroy()
        {
            EnemyRuntimeSet.Remove(this);
            base.SelfDestroy();
        }
    }
}