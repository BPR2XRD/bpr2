using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        isDead = false;
    }

    void Update()
    {
        
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
        else
        {
            Debug.Log("Player took damage. Current Health: " + currentHealth);
        }
    }

    void Die()
    {
        Debug.Log("Player has died!");
        isDead = true;
    }
}
