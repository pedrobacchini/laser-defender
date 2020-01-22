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
        [OdinSerialize] private bool Looping { get; set; } = false;
        
        private IEnumerator coroutine;

        void Start()
        {
            print("Starting " + Time.time);
            coroutine = StartWaves();
            StartCoroutine(coroutine);
            print("Done " + Time.time);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                StopCoroutine(coroutine);
                print("Stopped " + Time.time);
            }
        }

        private IEnumerator StartWaves()
        {
            do
            {
                yield return StartCoroutine(SpawnAllWaves());
            } while (Looping);
        }

        private IEnumerator SpawnAllWaves()
        {
            for (var waveIndex = StartingWave; waveIndex < WaveConfigs.Count; waveIndex++)
            {
                yield return StartCoroutine(SpawnAllEnemiesInWave(WaveConfigs[waveIndex]));
            }
        }

        private static IEnumerator SpawnAllEnemiesInWave(WaveConfig currentWave)
        {
            for (var enemyCount = 0; enemyCount < currentWave.NumberOfEnemies; enemyCount++)
            {
                var newEnemy = Instantiate(currentWave.EnemyPrefab, currentWave.WaveWayPoints[0].position,
                    Quaternion.identity);
                newEnemy.GetComponent<EnemyPathing>().WaveConfig = currentWave;
                EnemyRuntimeSet.Add(newEnemy.GetComponent<Enemy>());
                yield return new WaitForSeconds(currentWave.TimeBetweenSpawns);
            }
        }
    }
}