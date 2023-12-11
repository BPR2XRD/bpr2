using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Microlight.MicroBar;

public class PlayerHealthTests 
{
    private GameObject playerGameObject;
    private PlayerHealth playerHealth;

    [SetUp]
    public void SetUp()
    {
        // Create a test player GameObject
        playerGameObject = new GameObject();
        
        // Add PlayerHealth component to the GameObject
        playerHealth = playerGameObject.AddComponent<PlayerHealth>();

        // Mocking the required components
        playerHealth.fadeScreen = playerGameObject.AddComponent<FadeScreen>();
        playerHealth.audioSource = playerGameObject.AddComponent<AudioSource>();
        playerHealth.lights = playerGameObject.AddComponent<HueLights>();

        //setting to null, as they are not relevant for the main functionality
        playerHealth.fadeScreen = null;
        playerHealth.healthBar = null;
        playerHealth.lights = null;

    }

    [TearDown]
    public void TearDown()
    {
        // Clean up after the test
        Object.DestroyImmediate(playerGameObject);
    }

    [UnityTest]
    public IEnumerator PlayerHealth_TakeDamage_ReducesHealth()
    {
        int damageAmount = 20;

        // Save the initial health for comparison
        int initialHealth = playerHealth.currentHealth;

        // Apply damage to the player
        playerHealth.TakeDamage(damageAmount);

        // Wait for one frame to allow for updates
        yield return null;

        // Assert that the health has been reduced
        Assert.AreEqual(initialHealth - damageAmount, playerHealth.currentHealth);
    }

    [UnityTest]
    public IEnumerator PlayerHealth_Die_SetsIsDead()
    {
        // Set the player's health to zero
        playerHealth.currentHealth = 0;

        // Call the Die method
        playerHealth.Die();

        // Wait for one frame to allow for updates
        yield return null;

        // Assert that isDead is true
        Assert.IsTrue(playerHealth.isDead);
    }

    [UnityTest]
    public IEnumerator PlayerHealth_Die_LoadsEndScene()
    {
        // Set the player's health to zero
        playerHealth.currentHealth = 0;

        // Call the Die method
        playerHealth.Die();

        // Wait for the EndScene to be loaded
        yield return null; 

        // Assert that the current scene is EndScene
        Assert.AreEqual("EndScene", SceneManager.GetActiveScene().name);
    }
}
