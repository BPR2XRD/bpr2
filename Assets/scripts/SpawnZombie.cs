using System;
using System.Linq;
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
        StartCoroutine(DelayedExecution());
    }

    IEnumerator DelayedExecution()
    {
        while (true) // Infinite loop
        {
            // Wait for 10 seconds
            yield return new WaitForSeconds(10);

            // Code to execute after the delay
            SpawnZombiesIfNecessary();
        }
    }

    void SpawnZombiesIfNecessary()
    {
        GameObject[] aliveZombieBotGameObjects = GameObject
            .FindGameObjectsWithTag("ZombieBot")
            .Where(zombieBot =>
            {
                var zombieBotController = zombieBot.GetComponent<ZombieBotController>();
                return zombieBotController != null && !zombieBotController.isDead;
            })
            .ToArray();

        if (aliveZombieBotGameObjects.Length < 40)
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                InstantiateZombie(spawnPoint);
            }
        }
    }

    private void InstantiateZombie(Transform spawnPoint)
    {
        Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);
        zombiesCurrentlyOnMap++;
    }
}
