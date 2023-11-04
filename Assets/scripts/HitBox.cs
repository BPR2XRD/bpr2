using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public HealthZombie health;
   
    public void OnBulletHit(int damage)
    {
        health.TakeDamage(damage);
    }
}
