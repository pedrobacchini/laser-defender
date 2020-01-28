using SingletonScriptableObject;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(fileName = "New Enemy Class", menuName = "Enemy/Enemy Class")]
    public class EnemyClass : SerializedScriptableObject
    {
        [Title("Enemy Base")] [OdinSerialize] public Sprite Sprite { get; private set; }
        [OdinSerialize] public Vector3 Size { get; private set; }
        
        [Title("Enemy Stats")] [OdinSerialize] private float _maxHealth = 100f;
        public float MaxHealth => _maxHealth * GameMaster.Level.Value;
        [OdinSerialize] private int _scoreValue = 100;
        public int ScoreValue => _scoreValue * GameMaster.Level.Value;

        [Title("Death Effects")]
        [OdinSerialize]
        public GameObject DeathPrefab { get; private set; }
        [OdinSerialize] public float DurationOfDeathEffect { get; private set; } = 0.8f;
        [OdinSerialize] public AudioClip DeathSound { get; private set; }
        [MinMaxSlider(0, 1)] [OdinSerialize] public Vector2 DeathSoundVolume { get; private set; }

        [Title("Shooting")] [OdinSerialize] public PrefabTag ShootPrefabTag { get; private set; }
        [OdinSerialize] public float ShootSpeed { get; private set; } = 8;
        [MinMaxSlider(0, 5)] [OdinSerialize] public Vector2 TimeBetweenShoots { get; private set; }
        [OdinSerialize] public AudioClip ShootingSound { get; private set; }
        [MinMaxSlider(0, 1)] [OdinSerialize] public Vector2 ShootingVolume { get; private set; }
    }
}