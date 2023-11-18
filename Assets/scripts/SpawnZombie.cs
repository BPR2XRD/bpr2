using System;
using System.Collections;
using UnityEngine;
// using Random = UnityEngine.Random;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Transform[] spawnPoints; // Array of spawn points

    private float initialSpawnInterval = 15f; // Initial interval
    private float minSpawnInterval = 5f; // Minimum interval
    private float intervalReduction = 1f; // Interval reduction amount
    private float reductionFrequency = 20f; // Time in seconds to reduce interval
    private float waveDuration = 300f; // Duration of each wave (5 minutes)
    private float breakDuration = 15f; // Duration of break between waves
    public static int zombiesCurrentlyOnMap = 0; // Counter for zombies on the map

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves()
    {
        while (true) // Infinite loop for continuous waves
        {
            float elapsedTime = 0f;
        float currentSpawnInterval = initialSpawnInterval;
        zombiesCurrentlyOnMap = 0; // Reset zombie count at the start of each wave

        while (elapsedTime < waveDuration)
        {
            if (zombiesCurrentlyOnMap < 40)
            {
                foreach (Transform spawnPoint in spawnPoints)
                {
                    InstantiateZombie(spawnPoint);
                }
            }

            // Wait for the next spawn interval or until zombie count drops to 30 or less
            while (zombiesCurrentlyOnMap >= 40)
            {
                yield return null; // Wait indefinitely
            }

            yield return new WaitForSeconds(currentSpawnInterval);

            elapsedTime += currentSpawnInterval;
            if (elapsedTime % reductionFrequency < currentSpawnInterval && 
                currentSpawnInterval > minSpawnInterval)
            {
                currentSpawnInterval -= intervalReduction;
            }
        }

            // Super attack: spawn 30 zombies at the end of the wave
            if (zombiesCurrentlyOnMap < 40)
        {
            int zombiesToSpawn = Math.Min(30, 40 - zombiesCurrentlyOnMap); // Spawn up to the limit of 40
            for (int i = 0; i < zombiesToSpawn; i++)
            {
                Transform selectedSpawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
                InstantiateZombie(selectedSpawnPoint);
            }
        }

        yield return new WaitForSeconds(breakDuration); // Break between waves
        }
    }

    private void InstantiateZombie(Transform spawnPoint)
    {
        Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);
        zombiesCurrentlyOnMap++;
    }
}
