using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microlight.MicroBar;
using System;

public class PlayerHealth : MonoBehaviour
{
    public GameObject canvas1;
    public GameObject canvas2;
    public GameObject canvas3;
    public GameObject Locomotion;
    private EndSceen endScreen1;
    private EndSceen endScreen2;
    private EndSceen endScreen3;
    public int maxHealth = 100;
    public int currentHealth;
    public bool isDead = false;
    private MicroBar healthBar;
    private AudioSource audioSource;

    public Transform TopViewCamera;
    public Transform PlayersCamera;


    void Start()
    {
        Time.timeScale = 1;
        endScreen1 = canvas1.GetComponent<EndSceen>();
        endScreen2 = canvas2.GetComponent<EndSceen>();
        // endScreen3 = canvas3.GetComponent<EndSceen>();
        

        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        isDead = false;
        try
        {
            healthBar = GameObject.FindGameObjectWithTag("WhitePistol").GetComponentInChildren<MicroBar>();
            if (healthBar != null)
                healthBar.Initialize(currentHealth);
        }
        catch (System.Exception)
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
            catch (System.Exception)
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
            Debug.Log("canvas: " + canvas1);
            Debug.Log("endScreen: " + endScreen1);
            Debug.Log("Locomotion: " + Locomotion);
            Debug.Log("PlayersCamera: " + PlayersCamera);
            Debug.Log("TopViewCamera: " + TopViewCamera);

            canvas1.SetActive(true);
            canvas2.SetActive(true);
            endScreen1.Lose();
            endScreen2.Lose();
            Locomotion.SetActive(false);

            Die();

            Time.timeScale = 0;
        }
    }

    void Die()
    {
        isDead = true;
    }
}
