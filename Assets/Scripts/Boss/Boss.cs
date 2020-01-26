using Enemy;
using SingletonScriptableObject;
using UnityEngine;

namespace DefaultNamespace
{
    public class Boss : EnemyBase
    {
        public BossClass BossClass { get; private set; }
        
        public void StartBoss(BossClass bossClass)
        {
            BossClass = bossClass;
            _mainCamera = Camera.main;
            _spriteRenderer.color = _startColor;
            _transform.localScale = bossClass.Size;
            CurrentHealth.Value = bossClass.MaxHealth;
            _spriteRenderer.sprite = bossClass.Sprite;
            _scoreValue = bossClass.ScoreValue;
            _deathPrefab = bossClass.DeathPrefab;
            _durationOfExplosion = bossClass.DurationOfDeathEffect;
            _deathSound = bossClass.DeathSound;
            _deathSoundVolume = bossClass.DeathSoundVolume;
        }

        public override void SelfDestroy()
        {
            base.SelfDestroy();
            GameMaster.FinishBossBattle();
        }
    }
}