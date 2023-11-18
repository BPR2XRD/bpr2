using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    public GameObject bulletHole;
    public GameObject impactEffect;
    public GameObject[] bloodPrefabs;
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
                int randomIndex = Random.Range(0, bloodPrefabs.Length);
                var direction = hit.normal;
                float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + 180;
                var instance = Instantiate(bloodPrefabs[randomIndex], hit.point, Quaternion.Euler(0, angle + 90, 0));
                //instance.GetComponent<BFX_BloodSettings>().GroundHeight = 
                Destroy(instance, time);
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
