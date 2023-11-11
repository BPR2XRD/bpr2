using UnityEngine;

public class HealthZombie : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public GameObject zombiePrefab; // Assign the zombie prefab in the Inspector
    public Transform spawnPoint; // Assign a spawn point in the Inspector
    Ragdoll ragdoll;
    public AudioSource audioSource;
    public AudioClip ZombieHurt;

    // Start is called before the first frame update
    void Start()
    {
        ragdoll = GetComponent<Ragdoll>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        audioSource.PlayOneShot(ZombieHurt);
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (transform.CompareTag("Zombie"))
        {
            GetComponent<ZombieBehavior>().enabled = false;
        }
        ragdoll.ActivateRagdoll();
        InstantiateNewZombie();
        Destroy(gameObject, 10f); // Adjust the time as needed before destroying the zombie
    }

    private void InstantiateNewZombie()
    {
        if (zombiePrefab != null && spawnPoint != null)
        {
            Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogError("Zombie prefab or spawn point not set!");
        }
    }
}
