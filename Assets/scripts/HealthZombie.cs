using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthZombie : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    [SerializeField] private Transform zombieTransform;
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
        if(zombieTransform.transform.CompareTag("Zombie"))
        {
            GetComponent<ZombieBehavior>().enabled = false;
        }
        ragdoll.ActivateRagdoll();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
