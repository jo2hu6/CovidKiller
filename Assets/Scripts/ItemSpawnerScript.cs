using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnerScript : MonoBehaviour
{

    [SerializeField] GameObject checkpointPrefab;
    [SerializeField] int checkpointSpawnDelay = 20;
    [SerializeField] float spwanRadius = 30;
    [SerializeField] GameObject[] powerUpPrefab;
    [SerializeField] int powerUpSpawnDelay = 30;

    void Start()
    {
        StartCoroutine(SpawnCheckpointRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    IEnumerator SpawnCheckpointRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkpointSpawnDelay);
            Vector2 randomPosition = Random.insideUnitCircle * spwanRadius;
            Instantiate(checkpointPrefab, randomPosition, Quaternion.identity);
        }
        
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(powerUpSpawnDelay);
            Vector2 randomPosition = Random.insideUnitCircle * spwanRadius;
            int random = Random.Range(0, powerUpPrefab.Length);
            Instantiate(powerUpPrefab[random], randomPosition, Quaternion.identity);
        }
    }

}
