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

        public EnemyHealth EnemyHealth { get; } = new EnemyHealth();
        public float MaxHealth { get; private set; }

        private BossClass _bossClass;
        private readonly BossPathing _bossPathing = new BossPathing();
        private Transform _transform;
        private SpriteRenderer _spriteRenderer;
        private Color _startColor;
        private float _halfHealth;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _startColor = _spriteRenderer.color;
        }

        public void StartBoss(BossClass bossClass)
        {
            _bossClass = bossClass;
            _transform.localScale = bossClass.Size;
            _spriteRenderer.sprite = bossClass.Sprite;
            _spriteRenderer.color = _startColor;

            MaxHealth = bossClass.MaxHealth;
            _halfHealth = MaxHealth / 2;

            _bossPathing.StartBossPathing(bossClass, gameObject);
            EnemyHealth.StartEnemyHealth(gameObject, MaxHealth, _spriteRenderer, _startColor);

            Shield.StartShield(bossClass.MaxHealthShield);

            this.UpdateAsObservable()
                .Where(_ => EnemyHealth.CurrentHealth.Value < _halfHealth)
                .Subscribe(_ =>
                {
                    Shield.StartShield(bossClass.MaxHealthShield);
                    _disposables.Clear();
                })
                .AddTo(_disposables);
        }

        private void Update()
        {
            if (EnemyHealth.CurrentHealth.Value > 0) return;
            Die(_bossClass.ScoreValue, _bossClass.DeathPrefab, _bossClass.DurationOfDeathEffect,
                _bossClass.DeathSound, _bossClass.DeathSoundVolume);
            Destroy();
        }


        private void Destroy()
        {
            gameObject.SetActive(false);
            GameMaster.FinishBossBattle();
        }
    }
}