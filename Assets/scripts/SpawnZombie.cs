using System;
using System.Linq;
using System.Collections;
using UnityEngine;

public class SpawnZombie : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Transform[] spawnPoints; // Array of spawn points

    void Start()
    {
        StartCoroutine(DelayedExecution());
    }

    IEnumerator DelayedExecution()
    {
        while (true) // Infinite loop
        {
            // Wait for 8 seconds
            yield return new WaitForSeconds(8);

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

        if (aliveZombieBotGameObjects.Length < 25)
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
    }
}
