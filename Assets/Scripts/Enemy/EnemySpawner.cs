using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Enemy
{
    public class EnemySpawner : SerializedMonoBehaviour
    {
        [OdinSerialize] private List<WaveConfig> WaveConfigs { get; set; }
        [OdinSerialize] private int StartingWave { get; set; }

        private int _waveIndex;
        private bool _spawnWaveIsRunning;

        private void Start()
        {
            _waveIndex = StartingWave;
        }

        private void Update()
        {
            if (EnemyRuntimeSet.Items.Count != 0 || _spawnWaveIsRunning) return;
            NextWave();
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
                var newEnemy = Instantiate(currentWave.EnemyPrefab, currentWave.WaveWayPoints[0].position,
                    Quaternion.identity);
                newEnemy.GetComponent<EnemyPathing>().WaveConfig = currentWave;
                EnemyRuntimeSet.Add(newEnemy.GetComponent<Enemy>());
                yield return new WaitForSeconds(currentWave.TimeBetweenSpawns);
            }

            _spawnWaveIsRunning = false;
        }
    }
}