using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunShoot : MonoBehaviour
{
    [Header("Prefab Refrences")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location Refrences")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destory the casing object")][SerializeField] private float destroyTimer = 2f;
    [Tooltip("Casing Ejection Speed")][SerializeField] private float ejectPower = 150f;

    [Header("Inputs")]
    [Tooltip("Specify trigger button")][SerializeField] InputActionReference triger;

    public float damage = 10f;
      public float headDamage = 35f;
      public float range = 100f;
    public float fireRate = 2f;
    public float impactForce = 6f;

    private float nextTimeToFire = 0f;
    private AudioSource gunFireSound;

    public Camera mainCamera;
    public ParticleSystem muzzleFlash;

    public HitEffect hitEffect;



    private void Start()
    {
        gunFireSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        triger.action.started += ctx =>
        {
            if (Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                gunAnimator.SetTrigger("Fire");
                Debug.Log("Trigger button is pressed");
            }
        };

        //if (triger.action.IsPressed() && Time.time >= nextTimeToFire) 
        //{
        //    nextTimeToFire = Time.time + 1f / fireRate;
        //    gunAnimator.SetTrigger("Fire");
        //    Debug.Log("Trigger button is pressed");
        //}


        //if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        //{
        //    nextTimeToFire = Time.time + 1f / fireRate;
        //    Shoot();
        //}
    }

    void Shoot()
    {
        //if (!muzzleFlash.isPlaying) muzzleFlash.Play();
        //gunFireSound.PlayOneShot(gunFireSound.clip);

        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }
        RaycastHit hit;
        if (Physics.Raycast(barrelLocation.position, barrelLocation.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.CompareTag("Player")) return;

            if (hit.transform.TryGetComponent<Target>(out var target))
            {
                if (hit.collider.CompareTag("head")) // for extra damage
                {
                    target.TakeDamage(headDamage);
                }
                else
                {
                    target.TakeDamage(damage);
                }
                hitEffect.ShowHitEffect(hit, HitEffect.Effects.Impact, 1f);
            }
            else if(hit.transform.TryGetComponent<HitBox>(out var hitBox))
            {
                if (hit.collider.CompareTag("head")) // for extra damage
                {
                    hitBox.OnBulletHit(headDamage);
                }
                else
                {
                    hitBox.OnBulletHit(damage);
                }
                hitEffect.ShowHitEffect(hit, HitEffect.Effects.Blood, 1f);
            }
            else
            {
                hitEffect.ShowHitEffect(hit, HitEffect.Effects.Hole, 4f);
            }
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForceAtPosition(-hit.normal * impactForce, hit.point, ForceMode.Impulse);
            }
        }
    }

    public void CasingRelease()
    {
        //Cancels function if ejection slot hasn't been set or there's no casing
        if (!casingExitLocation || !casingPrefab)
        { return; }

        //Create the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy casing after X seconds
        Destroy(tempCasing, destroyTimer);
    }



}
