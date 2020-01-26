using Enemy;
using SingletonScriptableObject;
using Sirenix.Serialization;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace DefaultNamespace
{
    public class Boss : EnemyBase
    {
        [OdinSerialize] public BossClass BossClass { get; private set; }
        [OdinSerialize] private Shield Shield { get; set; }

        private float _halfHealth;
        
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public void StartBoss(BossClass bossClass)
        {
            BossClass = bossClass;
            _mainCamera = Camera.main;
            _spriteRenderer.color = _startColor;
            _transform.localScale = bossClass.Size;
            CurrentHealth.Value = bossClass.MaxHealth * GameMaster.Level.Value;
            _halfHealth = CurrentHealth.Value / 2; 
            _spriteRenderer.sprite = bossClass.Sprite;
            _scoreValue = bossClass.ScoreValue * GameMaster.Level.Value;
            _deathPrefab = bossClass.DeathPrefab;
            _durationOfExplosion = bossClass.DurationOfDeathEffect;
            _deathSound = bossClass.DeathSound;
            _deathSoundVolume = bossClass.DeathSoundVolume;
            Shield.StartShield(bossClass.MaxHealthShield);
            
            
            this.UpdateAsObservable()
                .Where(_ => CurrentHealth.Value < _halfHealth)
                .Subscribe(_ =>
                {
                    Shield.StartShield(BossClass.MaxHealthShield);
                    _disposables.Clear();
                })
                .AddTo(_disposables);
        }


        public override void SelfDestroy()
        {
            base.SelfDestroy();
            GameMaster.FinishBossBattle();
        }
    }
}