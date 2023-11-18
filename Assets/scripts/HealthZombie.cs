using System.Linq;
using UnityEngine;

public class HealthZombie : MonoBehaviour
{
    public float maxHealth = 5;
    public float currentHealth;
    Ragdoll ragdoll;
    public AudioSource audioSource;
    public AudioClip ZombieHurt;
    bool isDead = false;
    // public ControllerPlayerRespawn respawn;
    GameObject respawn;

    // Start is called before the first frame update
    void Start()
    {
        ragdoll = GetComponent<Ragdoll>();
        currentHealth = maxHealth;
        isDead = false;
       respawn = GameObject.FindGameObjectWithTag("ControllerPlayerSpawn");
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
            if(respawn!= null)
                  respawn.GetComponent<ControllerPlayerRespawn>().InstantiateNewZombie();
        }
        isDead = true;
        ragdoll.ActivateRagdoll();
        if (TryGetComponent(out Disolve disolve))
        {
            disolve.enabled = true;
        }
        Destroy(gameObject, 3.5f); // Adjust the time as needed before destroying the zombie
    }

}
