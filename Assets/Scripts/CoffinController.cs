using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A new script to be attached to the coffin prefab
public class CoffinController : MonoBehaviour
{
    private Animator animator;
    public float spawnDistance = 0.5f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnCoffinDropped(GameObject zombiePrefab)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        StartCoroutine(CoffinSequence(zombiePrefab));
    }

    private IEnumerator CoffinSequence(GameObject zombiePrefab)
    {
        // Wait for the coffin to settle on the ground
        yield return new WaitUntil(() => Mathf.Approximately(GetComponent<Rigidbody>().velocity.sqrMagnitude, 0));

        // Get the y position of the coffin as it settles
        float settledYPosition = transform.position.y - 3.4f;

        // Trigger the animation to open the coffin door
        animator.SetTrigger("OpenCoffinDoor");

        // Wait for the door to open before spawning the zombie
        yield return new WaitForSeconds(2.2f); // Adjust this duration as necessary

        // Calculate the position in front of the coffin to spawn the zombie
        Vector3 spawnPosition = transform.position + transform.forward * spawnDistance;

        // Adjust the y position to be a little above where the coffin settled
        float yOffset = 0.5f; // This offset raises the zombie slightly above the ground level. Adjust as necessary.
        spawnPosition.y = settledYPosition + yOffset;

        // Instantiate the zombie a bit in front of the coffin's position and slightly above the ground
        Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);

        // Destroy the coffin GameObject after a delay to allow the door opening animation to play
        Destroy(gameObject, 0.05f); // The delay value should be the same or greater than the door opening animation length
    }
}

