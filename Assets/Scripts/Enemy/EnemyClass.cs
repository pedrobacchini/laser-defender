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

        [Title("Enemy Stats")] [OdinSerialize] public float MaxHealth { get; private set; } = 100f;

        [OdinSerialize] public int ScoreValue { get; private set; } = 100;

        [Title("Shooting")] [OdinSerialize] public PrefabTag ShootPrefabTag { get; private set; }
        [OdinSerialize] public float ShootSpeed { get; private set; } = 8;
        [MinMaxSlider(0, 5)] [OdinSerialize] public Vector2 TimeBetweenShoots { get; private set; }

        [Title("Visual Effects")]
        [OdinSerialize]
        public GameObject DeathPrefab { get; private set; }

        [OdinSerialize] public float DurationOfExplosion { get; private set; } = 0.8f;

        [Title("Sound Effects")]
        [OdinSerialize]
        public AudioClip ShootingSound { get; private set; }

        [MinMaxSlider(0, 1)] [OdinSerialize] public Vector2 ShootingVolume { get; private set; }

        [OdinSerialize] public AudioClip DeathSound { get; private set; }

        [MinMaxSlider(0, 1)] [OdinSerialize] public Vector2 DeathSoundVolume { get; private set; }
    }
}