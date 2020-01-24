using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(menuName = "Enemy Wave Config")]
    public class WaveConfig : SerializedScriptableObject
    {
        [OdinSerialize] public EnemyClass EnemyClass { get; private set; }
    
        [OdinSerialize] private GameObject PathPrefab { get; set; }

        [OdinSerialize] public float TimeBetweenSpawns { get; private set; } = 0.5f;

        [OdinSerialize] public float SpawnRandomFactor { get; private set; } = 0.3f;

        [OdinSerialize] public int NumberOfEnemies { get; private set; } = 5;

        [OdinSerialize] public float MoveSpeed { get; private set; } = 2f;
        public List<Transform> WaveWayPoints => PathPrefab.transform.Cast<Transform>().ToList();
    }
}
