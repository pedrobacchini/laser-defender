using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "New Boss Class", menuName = "Enemy/Boss Class")]
    public class BossClass : SerializedScriptableObject
    {
        [Title("Enemy Base")] [OdinSerialize] public Sprite Sprite { get; private set; }
        [OdinSerialize] public Vector3 Size { get; private set; }
        [Title("Enemy Stats")] [OdinSerialize] public float MaxHealth { get; private set; } = 100f;
        [OdinSerialize] public int ScoreValue { get; private set; } = 100;

        [Title("Death Effects")]
        [OdinSerialize]
        public GameObject DeathPrefab { get; private set; }
        [OdinSerialize] public float DurationOfDeathEffect { get; private set; } = 0.8f;
        [OdinSerialize] public AudioClip DeathSound { get; private set; }
        [MinMaxSlider(0, 1)] [OdinSerialize] public Vector2 DeathSoundVolume { get; private set; }
    }
}