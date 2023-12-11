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

    internal IEnumerator DelayedExecution()
    {
        while (true) // Infinite loop
        {
            float sec = UnityEngine.Random.Range(6, 12);
            // Wait for random number of seconds
            yield return new WaitForSeconds(sec);

            // Code to execute after the delay
            SpawnZombiesIfNecessary();
        }
    }

    internal void SpawnZombiesIfNecessary()
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
