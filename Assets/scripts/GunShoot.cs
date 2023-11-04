using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.OpenXR.Input;

public class GunShoot : MonoBehaviour
{
    [Header("Prefab Refrences")]
    //public GameObject bulletPrefab;
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
    [Tooltip("Specify controller haptics")][SerializeField] InputActionReference haptics;

    [Header("Gun stats")]
    public int damage = 1;
    public int headDamage = 5;
    public float range = 100f;
    public float fireRate = 2f;
    public float impactForce = 6f;
    public int maxAmmo = 12; 
    [Range(1.0f, 5.0f)]
    [Tooltip("Changes how fast the gun fires")]public float animFireSpeed = 1f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip gunFireSound;
    public AudioClip gunReloadSound;
    public AudioClip gunNoAmmoSound;

    [Header("Visual effects")]
    [Tooltip("Used for bullet holes and stuff")] public HitEffect hitEffect;

    [Header("Ammo indicator")]
    public TextMeshPro ammoText;


    private float nextTimeToFire = 0f;
    private int currentAmmo;

    private void Start()
    {
        gunAnimator.speed = animFireSpeed;
        Reload();
    }

    // Update is called once per frame
    void Update()
    {
        triger.action.started += ctx =>
        {
            if (currentAmmo > 0)
            {
                //nextTimeToFire = Time.time + 1f / fireRate;
                gunAnimator.SetTrigger("Fire");
                //OpenXRInput.SendHapticImpulse(haptics, 1f, 4f, XRController.rightHand); //Right Hand Haptic Impulse, not working, found alternative
                if (ctx.control.device is XRController device)
                    Rumble(device);//vibrate
            }
            else audioSource.PlayOneShot(gunNoAmmoSound);
        };

        //Turn the gun to reload
        if (Vector3.Angle(transform.up, Vector3.up) > 100 && currentAmmo < maxAmmo)
            Reload();

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

    private void Rumble(InputDevice device)
    {
        //2-thumb area?, 1- trigger, 0 - whole body
        var channel = 1;
        var command = UnityEngine.InputSystem.XR.Haptics.SendHapticImpulseCommand.Create(channel, 1f, 0.1f);
        device.ExecuteCommand(ref command);
    }

    void Reload()
    {
        currentAmmo = maxAmmo;
        ammoText.SetText(currentAmmo.ToString()); //Change ammo indicator
        audioSource.PlayOneShot(gunReloadSound);
    }

    //Note: method is called inside the animation, using an Animation event
    void Shoot()
    {
        audioSource.PlayOneShot(gunFireSound);
        currentAmmo--;
        ammoText.SetText(currentAmmo.ToString()); //Change ammo indicator

        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }
        if (Physics.Raycast(barrelLocation.position, barrelLocation.transform.forward, out RaycastHit hit, range))
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
            else if (hit.transform.TryGetComponent<HitBox>(out var hitBox))
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
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f, 1f);
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy casing after X seconds
        Destroy(tempCasing, destroyTimer);
    }

    //void OnGUI()
    //{
    //    //Create a Label in Game view for the Slider
    //    GUI.Label(new Rect(0, 25, 40, 60), "Speed");
    //    //Create a horizontal Slider to control the speed of the Animator. Drag the slider to 1 for normal speed.

    //    animationSpeed = GUI.HorizontalSlider(new Rect(45, 25, 200, 60), animationSpeed, 0.0F, 3.0F);
    //    //Make the speed of the Animator match the Slider value
    //    gunAnimator.speed = animationSpeed;
    //}



}
