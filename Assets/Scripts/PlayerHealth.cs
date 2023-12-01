using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microlight.MicroBar;
using System;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool isDead = false;
    private MicroBar healthBar;
    private AudioSource audioSource;

    void Start()
    {    
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        isDead = false;
        GameData.isPlayerDead = isDead;
        try
        {
            healthBar = GameObject.FindGameObjectWithTag("WhitePistol").GetComponentInChildren<MicroBar>();
            if (healthBar != null)
                healthBar.Initialize(currentHealth);
        }
        catch (Exception)
        {
            Debug.Log("Left Pistol not initialized");
        }
    }

    void Update()
    {
        if (healthBar == null)
        {
            try
            {
                healthBar = GameObject.FindGameObjectWithTag("WhitePistol").GetComponentInChildren<MicroBar>();
                healthBar.Initialize(currentHealth);
            }
            catch (Exception)
            {
                Debug.Log("Left Pistol not initialized");
            }
        }

    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (!audioSource.isPlaying)
              audioSource.Play();
        if (healthBar != null)
            healthBar.UpdateHealthBar(currentHealth);

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        GameData.isPlayerDead = isDead;
        SceneManager.LoadSceneAsync("EndScene");
    }
}
