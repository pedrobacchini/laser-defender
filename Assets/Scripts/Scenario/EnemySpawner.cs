using System;
using System.Collections.Generic;
using DefaultNamespace;
using GameSystem.ObjectPool;
using SingletonScriptableObject;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UniRx;
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
                SpawnBoss();
            }

            if (GameMaster.CurrentStage.Value == GameStage.Enemies && !_isSpawnEnemies)
            {
                SpawnWave();
            }
        }

        private void SpawnWave()
        {
            _isSpawnEnemies = true;
            var waveConfig = WaveConfigs[_waveIndex];
            Observable.Interval(TimeSpan.FromSeconds(waveConfig.TimeBetweenSpawns))
                .Take(waveConfig.NumberOfEnemies)
                .Where(_ => _isPlayable)
                .Subscribe(_ => SpawnEnemy(waveConfig), () =>
                {
                    _waveIndex = (_waveIndex + 1) % WaveConfigs.Count;
                    _isSpawnEnemies = false;
                })
                .AddTo(this);
        }

        private void SpawnEnemy(WaveConfig waveConfig)
        {
            var newEnemy = ObjectPooler.SpawnFromPool(EnemyBasePrefabTag,
                waveConfig.WaveWayPoints[0].position,
                Quaternion.identity).GetComponent<Enemy>();
            newEnemy.StartEnemy(waveConfig);
            EnemyRuntimeSet.Add(newEnemy);
        }

        private void SpawnBoss()
        {
            GameMaster.StartBossBattle();
            Boss.gameObject.SetActive(true);
            Boss.StartBoss(BossClass);
        }
    }
}