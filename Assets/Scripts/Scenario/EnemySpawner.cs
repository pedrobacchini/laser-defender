using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DefaultNamespace;
using GameSystem.ObjectPool;
using SingletonScriptableObject;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Enemy
{
    public class EnemySpawner : SerializedMonoBehaviour
    {
        [OdinSerialize] private List<WaveConfig> WaveConfigs { get; set; }
        [OdinSerialize] private int StartingWave { get; set; }
        [OdinSerialize] public PrefabTag EnemyBasePrefabTag { get; private set; }
        [OdinSerialize] public Boss Boss { get; private set; }
        [OdinSerialize] public BossClass BossClass { get; private set; }

        private int _waveIndex;
        private bool _isPlayable = true;
        private bool _isSpawnEnemies = false;

        private void Start()
        {
            _waveIndex = StartingWave;
            GameEventManager.StartGameOver += () => _isPlayable = false;
            if (GameMaster.CurrentStage.Value == GameStage.BossBattle) SpawnBoss();
        }

        private void Update()
        {
            if (EnemyRuntimeSet.Items.Count != 0 || GameMaster.CurrentStage.Value == GameStage.BossBattle) return;
            if (GameMaster.LevelScore.Value >= GameMaster.PointsBossStage)
            {
                GameMaster.InitBossBattle();
                SpawnBoss();
            }

            if (GameMaster.CurrentStage.Value == GameStage.Enemies && !_isSpawnEnemies)
            {
                StartCoroutine(SpawnAllEnemiesInWave(WaveConfigs[_waveIndex]));
            }
        }

        private void SpawnBoss()
        {
            Boss.gameObject.SetActive(true);
            Boss.StartBoss(BossClass);
        }

        [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
        private IEnumerator SpawnAllEnemiesInWave(WaveConfig currentWave)
        {
            _isSpawnEnemies = true;
            for (var enemyCount = 0; enemyCount < currentWave.NumberOfEnemies && _isPlayable; enemyCount++)
            {
                var newEnemy = ObjectPooler.SpawnFromPool(EnemyBasePrefabTag, currentWave.WaveWayPoints[0].position,
                    Quaternion.identity).GetComponent<Enemy>();
                newEnemy.StartEnemy(currentWave);
                EnemyRuntimeSet.Add(newEnemy);
                yield return new WaitForSeconds(currentWave.TimeBetweenSpawns);
            }

            _waveIndex = (_waveIndex + 1) % WaveConfigs.Count;
            _isSpawnEnemies = false;
        }
    }
}