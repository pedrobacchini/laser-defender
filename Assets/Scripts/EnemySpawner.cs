using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class EnemySpawner : SerializedMonoBehaviour
{
    [OdinSerialize] private List<WaveConfig> WaveConfigs { get; set; }

    private const int StartingWave = 0;

    private void Start()
    {
        var currentWave = WaveConfigs[StartingWave];
        StartCoroutine(SpawnAllEnemiesInWave(currentWave));
    }

    private static IEnumerator SpawnAllEnemiesInWave(WaveConfig currentWave)
    {
        for (var enemyCount = 0; enemyCount < currentWave.NumberOfEnemies; enemyCount++)
        {
            Instantiate(currentWave.EnemyPrefab, currentWave.WaveWayPoints[0].position, Quaternion.identity);
            yield return new WaitForSeconds(currentWave.TimeBetweenSpawns);   
        }
    }
}
