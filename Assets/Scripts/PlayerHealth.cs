using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microlight.MicroBar;
using System;
using UnityEngine.SceneManagement;
using Q42.HueApi;
using UnityEditor.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool isDead = false;
    public FadeScreen fadeScreen;
    internal MicroBar healthBar;
    internal AudioSource audioSource;
    internal HueLights lights;

    void Start()
    {    
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        isDead = false;
        lights = GameObject.FindGameObjectWithTag("Manager").GetComponent<HueLights>();
        GameData.isPlayerDead = isDead;
        try
        {
            healthBar = GameObject.FindGameObjectWithTag("WhitePistol").GetComponentInChildren<MicroBar>();
            if (healthBar != null)
                healthBar.Initialize(currentHealth);
        }
        catch (Exception)
        {
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

    internal void Die()
    {
        isDead = true;
        GameData.isPlayerDead = isDead;
        if(fadeScreen  != null)
            fadeScreen.FadeOut();
        if (lights != null)
            lights.PlayerDead();

#if UNITY_EDITOR
        // In the editor, use EditorSceneManager to load scenes during edit mode
        EditorSceneManager.OpenScene("Assets/Scenes/EndScene.unity", OpenSceneMode.Single);
#else
        // In play mode, use SceneManager to load scenes
        SceneManager.LoadScene("EndScene");
#endif
    }
}
