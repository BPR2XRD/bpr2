using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    public GameObject bulletHole;
    public GameObject impactEffect;
    public GameObject bloodEffect;
 
    public void ShowHitEffect(RaycastHit hit, Effects effect, float time)
    {
        switch (effect)
        {
            case Effects.Hole:
                InstantiateAndDestroy(hit, bulletHole, time); 
                break;
            case Effects.Impact:
                InstantiateAndDestroy(hit, impactEffect, time); 
                break;
            case Effects.Blood:
                InstantiateAndDestroy(hit, bloodEffect, time);
                break;
        }
    }

    void InstantiateAndDestroy(RaycastHit hit, GameObject gameObject, float time)
    {
        GameObject tmpBulletHole = Instantiate(gameObject, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(tmpBulletHole, time);
    }



    public enum Effects
    {
        Hole,
        Impact,
        Blood
    }
}
