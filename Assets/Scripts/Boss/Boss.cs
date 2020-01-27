using Enemy;
using SingletonScriptableObject;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace DefaultNamespace
{
    public class Boss : SerializedMonoBehaviour
    {
        [OdinSerialize] private Shield Shield { get; set; }

        public EnemyHealth EnemyHealth { get; } = new EnemyHealth();
        public float MaxHealth { get; private set; }

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
            _spriteRenderer.color = _startColor;
            _transform.localScale = bossClass.Size;
            _spriteRenderer.sprite = bossClass.Sprite;
            _spriteRenderer.color = _startColor;

            MaxHealth = bossClass.MaxHealth * GameMaster.Level.Value;
            _halfHealth = MaxHealth / 2;
            var ScoreValue = bossClass.ScoreValue * GameMaster.Level.Value;
            EnemyHealth.StartEnemyHealth(gameObject, MaxHealth, _spriteRenderer, _startColor, ScoreValue,
                bossClass.DeathPrefab, bossClass.DurationOfDeathEffect, bossClass.DeathSound,
                bossClass.DeathSoundVolume, SelfDestroy);

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


        private void SelfDestroy()
        {
            gameObject.SetActive(false);
            GameMaster.FinishBossBattle();
        }
    }
}