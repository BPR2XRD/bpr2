using System.Collections;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public Transform spawnPoint3;
    public Transform spawnPoint4;

    private float initialSpawnInterval = 15f; // Initial interval
    private float minSpawnInterval = 5f; // Minimum interval
    private float intervalReduction = 1f; // Interval reduction amount
    private float reductionFrequency = 20f; // Time in seconds to reduce interval
    private float waveDuration = 300f; // Duration of each wave (5 minutes)
    private float breakDuration = 15f; // Duration of break between waves

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

            while (elapsedTime < waveDuration)
            {
                Instantiate(objectToSpawn, spawnPoint1.position, spawnPoint1.rotation);
                Instantiate(objectToSpawn, spawnPoint2.position, spawnPoint2.rotation);
                Instantiate(objectToSpawn, spawnPoint3.position, spawnPoint3.rotation);
                Instantiate(objectToSpawn, spawnPoint4.position, spawnPoint4.rotation);

                yield return new WaitForSeconds(currentSpawnInterval);

                elapsedTime += currentSpawnInterval;
                if (elapsedTime % reductionFrequency < currentSpawnInterval && 
                    currentSpawnInterval > minSpawnInterval)
                {
                    currentSpawnInterval -= intervalReduction;
                }
            }

            yield return new WaitForSeconds(breakDuration); // Break between waves
        }
    }
}
