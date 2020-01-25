using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;

namespace GameSystem.ObjectPool
{
    [CreateAssetMenu(fileName = "Singleton Object Pooler", menuName = "Singletons/Object Pooler")]
    public class ObjectPooler : SingletonScriptableObject<ObjectPooler>
    {
        #region Variables and Properties

        // Dictionary
        private Dictionary<PrefabTag, Queue<GameObject>> poolDictionary;

        // Objects
        [System.Serializable]
        public class Pool
        {
            public PrefabTag tag;
            [AssetsOnly] public GameObject prefab;
            public int size;
        }

        [BoxGroup("Object Pooler", centerLabel: true)]
        [OdinSerialize]
        private List<Pool> Pools { get; set; } = null;

        #endregion

        /// <summary>
        /// Spawn Prefab with Tag at new Position and new Rotation.
        /// </summary>
        /// <param name="Tag"></param>
        /// <param name="Position"></param>
        /// <param name="Rotation"></param>
        /// <returns></returns>
        [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
        public static GameObject SpawnFromPool(PrefabTag Tag, Vector3 Position, Quaternion Rotation)
        {
            if (!Instance.poolDictionary.ContainsKey(Tag))
            {
                Debug.LogWarning("Pool with name: \"" + Tag + "\" doesn't exist.");
                return null;
            }

            var objectToSpawn = Instance.poolDictionary[Tag].Dequeue();

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = Position;
            objectToSpawn.transform.rotation = Rotation;

            // Call object on spawn method
            var objectPools = objectToSpawn.GetComponents<IPooledObjects>();

            objectPools?.ForEach(Object => Object.OnObjectSpawn());

            Instance.poolDictionary[Tag].Enqueue(objectToSpawn);

            return objectToSpawn;
        }

        [UsedImplicitly]
        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            GameEventManager.FinishGameOver += ResetPool;

            Instance.poolDictionary = new Dictionary<PrefabTag, Queue<GameObject>>();

            // Spawn objects
            Instance.Pools.ForEach(pool =>
            {
                var objectPool = new Queue<GameObject>();
                for (var poolIndex = 0; poolIndex < pool.size; poolIndex++)
                {
                    var obj = Instantiate(pool.prefab);
                    DontDestroyOnLoad(obj);

                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }

                Instance.poolDictionary.Add(pool.tag, objectPool);
            });
        }

        private static void ResetPool()
        {
            Instance.poolDictionary.ForEach(pair => pair.Value.ForEach(gameObject => gameObject.SetActive(false)));
        }
    }
}