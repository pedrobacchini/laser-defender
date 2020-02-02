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
        [OdinSerialize] private Shield Shield { get; set; }
        public FloatReactiveProperty CurrentHealth => _enemyHealth.CurrentHealth;


        private readonly EnemyHealth _enemyHealth = new EnemyHealth();
        public float MaxHealth { get; private set; }

        private BossClass _bossClass;
        private readonly BossPathing _bossPathing = new BossPathing();
        private Transform _transform;
        private SpriteRenderer _spriteRenderer;
        private float _halfHealth;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _enemyHealth.Awake(_spriteRenderer);
        }

        public void StartBoss(BossClass bossClass)
        {
            _bossClass = bossClass;
            _transform.localScale = bossClass.Size;
            _spriteRenderer.sprite = bossClass.Sprite;

            MaxHealth = bossClass.MaxHealth;
            _halfHealth = MaxHealth / 2;

            _bossPathing.StartBossPathing(bossClass, gameObject);
            _enemyHealth.StartEnemyHealth(gameObject, MaxHealth, Death);

            Shield.StartShield(bossClass.MaxHealthShield);

            this.UpdateAsObservable()
                .Where(_ => _enemyHealth.CurrentHealth.Value < _halfHealth)
                .Subscribe(_ =>
                {
                    Shield.StartShield(bossClass.MaxHealthShield);
                    _disposables.Clear();
                })
                .AddTo(_disposables);
        }
        
        private void Death()
        {
            GameMaster.AddScore(_bossClass.ScoreValue);
            DeathEffect(_bossClass.DeathPrefab, _bossClass.DurationOfDeathEffect, 
                _bossClass.DeathSound, _bossClass.DeathSoundVolume);
            Destroy();
        }


        private void Destroy()
        {
            _disposables.Clear();
            GameMaster.FinishBossBattle();
            gameObject.SetActive(false);
        }
    }
}