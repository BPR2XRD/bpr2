using System.Collections;
using UnityEngine;

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
                foreach (Transform spawnPoint in spawnPoints)
                {
                    InstantiateZombie(spawnPoint);
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
            for (int i = 0; i < 30; i++)
            {
                Transform selectedSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                InstantiateZombie(selectedSpawnPoint);
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
