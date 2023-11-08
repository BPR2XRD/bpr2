using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthZombie : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    Ragdoll ragdoll;
    // Start is called before the first frame update
    void Start()
    {
        ragdoll = GetComponent<Ragdoll>();
        currentHealth = maxHealth;
    }
    public void TakeDamage(int amount){
        currentHealth -= amount;
        if(currentHealth <= 0)
        {
            Die();

        }
    }
    private void Die(){
        if(transform.CompareTag("Zombie"))
        {
            GetComponent<ZombieBehavior>().enabled = false;
        }
        ragdoll.ActivateRagdoll();
        Destroy(gameObject, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
