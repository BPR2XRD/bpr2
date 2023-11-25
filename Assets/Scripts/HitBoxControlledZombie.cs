using UnityEngine;

public class HitBoxControlledZombie : MonoBehaviour
{
    public HealthZombie healthZombie;

    void Awake()
    {
        healthZombie = GetComponentInParent<HealthZombie>();
    }
   
    public void OnBulletHit(int damage)
    {
        healthZombie.TakeDamage(damage);
    }
}
