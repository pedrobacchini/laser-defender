using System.Collections.Generic;
using System.Linq;
using SingletonScriptableObject;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "New Boss Class", menuName = "Enemy/Boss Class")]
    public class BossClass : SerializedScriptableObject
    {
        [Title("Boss Base")] [OdinSerialize] public Sprite Sprite { get; private set; }
        [OdinSerialize] public Vector3 Size { get; private set; }

        [Title("Boss Stats")] [OdinSerialize] private float _maxHealth = 100f;
        public float MaxHealth => _maxHealth * GameMaster.Level.Value;
        [OdinSerialize] private int _scoreValue = 100;
        public int ScoreValue => _scoreValue * GameMaster.Level.Value;
        [OdinSerialize] private float _maxHealthShield = 100f;
        public float MaxHealthShield => _maxHealthShield * GameMaster.Level.Value;

        [Title("Boss Movement")]
        [OdinSerialize]
        public float MoveSpeed { get; private set; } = 2f;
        [OdinSerialize] private GameObject PathPrefab { get; set; }
        [OdinSerialize] public int StartLoopPath { get; private set; }

        [Title("Death Effects")]
        [OdinSerialize]
        public GameObject DeathPrefab { get; private set; }
        [OdinSerialize] public float DurationOfDeathEffect { get; private set; } = 0.8f;
        [OdinSerialize] public AudioClip DeathSound { get; private set; }
        [MinMaxSlider(0, 1)] [OdinSerialize] public Vector2 DeathSoundVolume { get; private set; }
        public List<Transform> BossWayPoints => PathPrefab.transform.Cast<Transform>().ToList();
    }
}