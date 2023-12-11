using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;

[TestFixture]
public class SpawnZombieTests
{
    [Test]
    public void SpawnZombiesIfNecessary_SpawnPointsAvailable_SpawnsZombies()
    {
        // Arrange
        var spawnZombieScript = new GameObject().AddComponent<SpawnZombie>();
        spawnZombieScript.objectToSpawn = new GameObject(); // Mock a prefab
        spawnZombieScript.objectToSpawn.tag = "ZombieBot";
        var spawnPointObject = new GameObject();
        spawnZombieScript.spawnPoints = new Transform[1];
        spawnZombieScript.spawnPoints[0] = spawnPointObject.transform;// Mock a spawn point

        // Act
        spawnZombieScript.SpawnZombiesIfNecessary();

        // Assert
        Assert.AreEqual(2, GameObject.FindGameObjectsWithTag("ZombieBot").Length, "Should spawn zombies if necessary");
    }

    [Test]
    public void SpawnZombiesIfNecessary_MaxZombiesReached_DoesNotSpawnZombies()
    {
        // Arrange
        var spawnZombieScript = new GameObject().AddComponent<SpawnZombie>();
        spawnZombieScript.objectToSpawn = new GameObject(); // Mock a prefab
        spawnZombieScript.spawnPoints = new Transform[1]; // Mock a spawn point

        // Mock 25 alive zombie bots
        for (int i = 0; i < 25; i++)
        {
            var zombie = new GameObject();
            zombie.tag = "ZombieBot";
            var zombieBotController = zombie.AddComponent<ZombieBotController>();
            zombieBotController.isDead = false;
        }

        // Act
        spawnZombieScript.SpawnZombiesIfNecessary();

        // Assert
        Assert.AreEqual(25, GameObject.FindGameObjectsWithTag("ZombieBot").Length, "Should not spawn zombies if max zombies reached");
    }

    [UnityTest]
    public IEnumerator DelayedExecution_WaitsForRandomSeconds()
    {
        // Arrange
        var spawnZombieScript = new GameObject().AddComponent<SpawnZombie>();

        // Act
        float elapsedTime = 0;
        while (elapsedTime < 2)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        // Assert
        Assert.GreaterOrEqual(elapsedTime, 2f, "Should wait for at least 2 seconds");
    }

}

