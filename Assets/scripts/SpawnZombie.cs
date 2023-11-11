using System.Collections;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectToSpawn; // Assign your prefab to this in the Inspector

    // Transforms for where the objects will be instantiated
    public Transform spawnPoint1; // Assign your empty GameObject here in the Inspector
    public Transform spawnPoint2; // Assign your empty GameObject here in the Inspector

    // Time interval for spawning objects
    private float spawnInterval = 15f; // 15 seconds

    // Total time for which objects will be spawned
    private float totalTime = 900f; // 15 minutes

    // Start is called before the first frame update
    void Start()
    {
        // Start the Coroutine for spawning objects
        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        float elapsedTime = 0f;

        // Spawn objects until the total time has been reached
        while (elapsedTime < totalTime)
        {
            // Instantiate objects at both positions
            Instantiate(objectToSpawn, spawnPoint1.position, spawnPoint1.rotation);
            Instantiate(objectToSpawn, spawnPoint2.position, spawnPoint2.rotation);
            
            // Wait for the spawn interval
            yield return new WaitForSeconds(spawnInterval);

            // Increment the elapsed time by the spawn interval
            elapsedTime += spawnInterval;
        }
    }
}
