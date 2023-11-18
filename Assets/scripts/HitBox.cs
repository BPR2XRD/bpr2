using UnityEngine;

public class HitBox : MonoBehaviour
{
    public ZombieBotController zombieBotController;

    void Awake()
    {
        zombieBotController = GetComponentInParent<ZombieBotController>();
    }
   
    public void OnBulletHit(int damage)
    {
        zombieBotController.TakeDamage(damage);
    }
}
