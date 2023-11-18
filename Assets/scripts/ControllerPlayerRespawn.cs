using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPlayerRespawn : MonoBehaviour
{
     public GameObject zombiePrefab;
     public Transform spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     public void InstantiateNewZombie()
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
