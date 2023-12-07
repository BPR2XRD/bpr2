using System;
using System.Linq;
using System.Collections;
using UnityEngine;

public class SpawnZombie : MonoBehaviour
{
    public GameObject objectToSpawn; // The zombie object to be spawned
    public Transform[] spawnPoints; // Array of spawn points for zombies

    void Start()
    {
        // Start the coroutine for delayed execution when the script starts
        StartCoroutine(DelayedExecution());
    }

    IEnumerator DelayedExecution()
    {
        // Continuous loop to repeatedly execute spawning logic
        while (true)
        {
            // Randomly decide the wait time between 6 to 12 seconds
            float sec = UnityEngine.Random.Range(6, 12);
            // Wait for the determined duration
            yield return new WaitForSeconds(sec);

            // After the wait, check and execute zombie spawning if necessary
            SpawnZombiesIfNecessary();
        }
    }

    void SpawnZombiesIfNecessary()
    {
        // Find all existing zombies that are alive
        GameObject[] aliveZombieBotGameObjects = GameObject
            .FindGameObjectsWithTag("ZombieBot") // Find all objects with the "ZombieBot" tag
            .Where(zombieBot =>
            {
                // Get the ZombieBotController component and check if the zombie is not dead
                var zombieBotController = zombieBot.GetComponent<ZombieBotController>();
                return zombieBotController != null && !zombieBotController.isDead;
            })
            .ToArray();

        // If the number of alive zombies is less than 25
        if (aliveZombieBotGameObjects.Length < 25)
        {
            // Spawn a new zombie at each spawn point
            foreach (Transform spawnPoint in spawnPoints)
            {
                InstantiateZombie(spawnPoint);
            }
        }
    }

    private void InstantiateZombie(Transform spawnPoint)
    {
        // Create a new zombie at the given spawn point's position and rotation
        Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);
    }
}
