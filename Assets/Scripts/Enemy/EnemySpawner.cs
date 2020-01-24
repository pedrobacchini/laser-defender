using System.Collections;
using System.Collections.Generic;
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
        [OdinSerialize] public GameObject EnemyPrefabBase { get; private set; }

        private int _waveIndex;
        private bool _spawnWaveIsRunning;

        private void Start()
        {
            _waveIndex = StartingWave;
        }

        private void Update()
        {
            if (EnemyRuntimeSet.Items.Count != 0 || _spawnWaveIsRunning) return;
            if (GameMaster.CurrentScore.Value >= GameMaster.PointsBossStage)
            {
                Debug.Log("Boss Battle");
            }
            else
            {
                NextWave();   
            }
        }

        private void NextWave()
        {
            StartCoroutine(SpawnAllEnemiesInWave(WaveConfigs[_waveIndex]));
            _waveIndex = (_waveIndex + 1) % WaveConfigs.Count;
        }

        private IEnumerator SpawnAllEnemiesInWave(WaveConfig currentWave)
        {
            _spawnWaveIsRunning = true;
            for (var enemyCount = 0; enemyCount < currentWave.NumberOfEnemies; enemyCount++)
            {
                var newEnemy = Instantiate(EnemyPrefabBase, currentWave.WaveWayPoints[0].position,
                    Quaternion.identity);
                newEnemy.GetComponent<Enemy>().EnemyClass = currentWave.EnemyClass;
                newEnemy.GetComponent<EnemyShooting>().EnemyClass = currentWave.EnemyClass;
                newEnemy.GetComponent<EnemyPathing>().WaveConfig = currentWave;
                EnemyRuntimeSet.Add(newEnemy.GetComponent<Enemy>());
                yield return new WaitForSeconds(currentWave.TimeBetweenSpawns);
            }

            _spawnWaveIsRunning = false;
        }
    }
}