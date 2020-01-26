﻿using System.Collections;
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
        [OdinSerialize] public GameObject Boss { get; private set; }
        [OdinSerialize] public BossClass BossClass { get; private set; }
        
        private int _waveIndex;
        private bool _isPlayable = true;

        private void Start()
        {
            _waveIndex = StartingWave;
            GameEventManager.StartGameOver += () => _isPlayable = false;
        }

        private void Update()
        {
            if (EnemyRuntimeSet.Items.Count != 0 || GameMaster.CurrentStage.Value == GameStage.BossBattle) return;
            if (GameMaster.LevelScore.Value >= GameMaster.PointsBossStage)
            {
                GameMaster.InitBossBattle();
                Boss.SetActive(true);
                Boss.GetComponent<Boss>().StartBoss(BossClass);
            }

            if (GameMaster.CurrentStage.Value == GameStage.Enemies)
            {
                StartCoroutine(SpawnAllEnemiesInWave(WaveConfigs[_waveIndex]));
            }
        }

        [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
        private IEnumerator SpawnAllEnemiesInWave(WaveConfig currentWave)
        {
            for (var enemyCount = 0; enemyCount < currentWave.NumberOfEnemies && _isPlayable; enemyCount++)
            {
                var newEnemy = ObjectPooler.SpawnFromPool(EnemyBasePrefabTag, currentWave.WaveWayPoints[0].position,
                    Quaternion.identity);
                newEnemy.GetComponent<Enemy>().StartEnemy(currentWave.EnemyClass);
                newEnemy.GetComponent<EnemyShooting>().StartEnemyShooting(currentWave.EnemyClass);
                newEnemy.GetComponent<EnemyPathing>().StartEnemyPathing(currentWave);
                EnemyRuntimeSet.Add(newEnemy.GetComponent<Enemy>());
                yield return new WaitForSeconds(currentWave.TimeBetweenSpawns);
            }

            _waveIndex = (_waveIndex + 1) % WaveConfigs.Count;
        }
    }
}