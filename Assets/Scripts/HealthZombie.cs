using UnityEngine;

public class HealthZombie : MonoBehaviour
{
    public float maxHealth = 5;
    public float currentHealth;
    public GameObject zombiePrefab; // Assign the zombie prefab in the Inspector
    public Transform spawnPoint; // Assign a spawn point in the Inspector
    Ragdoll ragdoll;
    public AudioSource audioSource;
    public AudioClip ZombieHurt;
    bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        ragdoll = GetComponent<Ragdoll>();
        currentHealth = maxHealth;
        isDead = false;
    }

    public void TakeDamage(int amount)
    {
        audioSource.PlayOneShot(ZombieHurt);
        currentHealth -= amount;
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        if (transform.CompareTag("Zombie"))
        {
            GetComponent<ZombieBehavior>().enabled = false;
            InstantiateNewZombie();
        }
        isDead = true;
        ragdoll.ActivateRagdoll();
        if (TryGetComponent(out Disolve disolve))
        {
            disolve.enabled = true;
        }
        Destroy(gameObject, 3.5f); // Adjust the time as needed before destroying the zombie
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