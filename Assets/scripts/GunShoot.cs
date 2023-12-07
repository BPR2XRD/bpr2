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
    [Header("Prefab References")]
    // Prefabs for various visual effects
    //public GameObject bulletPrefab; // Prefab for bullets (currently commented out)
    public GameObject casingPrefab; // Prefab for ejected casings
    public GameObject muzzleFlashPrefab; // Prefab for muzzle flash effect

    [Header("Location References")]
    // References to various parts of the gun for animations and effects
    [SerializeField] private Animator gunAnimator; // Animator for the gun
    [SerializeField] private Transform barrelLocation; // Location where bullets exit
    [SerializeField] private Transform casingExitLocation; // Location where casings are ejected

    [Header("Settings")]
    // Settings for the gun's behavior
    [Tooltip("Specify time to destroy the casing object")]
    [SerializeField] private float destroyTimer = 2f; // Time before destroying casing
    [Tooltip("Casing Ejection Speed")]
    [SerializeField] private float ejectPower = 150f; // Power at which casings are ejected

    [Header("Inputs")]
    // Input references for triggering actions
    [Tooltip("Specify trigger button")]
    [SerializeField] InputActionReference trigger; // Input for gun trigger
    [Tooltip("Specify controller haptics")]
    [SerializeField] InputActionReference haptics; // Input for controller haptics

    [Header("Gun Stats")]
    // Stats for the gun's functionality
    public int damage = 1; // Damage per bullet
    public int headDamage = 5; // Damage for headshots
    public float range = 100f; // Range of the gun
    public float fireRate = 2f; // Rate of fire
    public float impactForce = 6f; // Force applied on impact
    public int maxAmmo = 12; // Maximum ammo capacity
    [Range(1.0f, 5.0f)]
    [Tooltip("Changes how fast the gun fires")]
    public float animFireSpeed = 1f; // Speed of the firing animation

    [Header("Audio")]
    // Audio sources and clips for gun sounds
    public AudioSource audioSource; // Audio source component
    public AudioClip gunFireSound; // Sound for gun firing
    public AudioClip gunReloadSound; // Sound for gun reloading
    public AudioClip gunNoAmmoSound; // Sound when out of ammo

    [Header("Visual Effects")]
    [Tooltip("Used for bullet holes and stuff")]
    public HitEffect hitEffect; // Script for managing hit effects

    [Header("Ammo Indicator")]
    public TextMeshPro ammoText; // Text displaying current ammo

    private float nextTimeToFire = 0f; // Time until the gun can fire again
    private int currentAmmo; // Current ammo count

    private void Start()
    {
        gunAnimator.speed = animFireSpeed; // Set the animation speed
        Reload(); // Reload the gun at start
    }

    void Update()
    {
        // Input handling and shooting logic in the Update loop
        trigger.action.started += ctx =>
        {
            if (currentAmmo > 0)
            {
                //nextTimeToFire = Time.time + 1f / fireRate; // Calculate the next time to fire (currently commented out)
                gunAnimator.SetTrigger("Fire"); // Trigger the fire animation
                // Haptic feedback for the controller
                if (ctx.control.device is XRController device)
                    Rumble(device); // Vibrate the controller
            }
            else audioSource.PlayOneShot(gunNoAmmoSound); // Play sound if out of ammo
        };

        // Reload the gun when it's turned upside down
        if (Vector3.Angle(transform.up, Vector3.up) > 100 && currentAmmo < maxAmmo)
            Reload();

    }

    private void Rumble(InputDevice device)
    {
        // Haptic feedback functionality
        var channel = 1; // Channel for the haptic feedback
        var command = UnityEngine.InputSystem.XR.Haptics.SendHapticImpulseCommand.Create(channel, 1f, 0.1f); // Create the haptic impulse command
        device.ExecuteCommand(ref command); // Execute the haptic feedback command
    }

    void Reload()
    {
        // Reload functionality
        currentAmmo = maxAmmo; // Reset the ammo count
        ammoText.SetText(currentAmmo.ToString()); // Update the ammo text
        audioSource.PlayOneShot(gunReloadSound); // Play reload sound
    }

    // Method called during the shooting animation using an Animation Event
    void Shoot()
    {
        // Shooting functionality
        audioSource.PlayOneShot(gunFireSound); // Play shooting sound
        currentAmmo--; // Decrease ammo count
        if (currentAmmo < 0) currentAmmo = 0; // Prevent negative ammo values
        ammoText.SetText(currentAmmo.ToString()); // Update ammo text

        // Muzzle flash effect
        if (muzzleFlashPrefab)
        {
            // Instantiate and destroy the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);
            Destroy(tempFlash, destroyTimer);
        }

        // Raycast to detect hits
        if (Physics.Raycast(barrelLocation.position, barrelLocation.transform.forward, out RaycastHit hit, range))
        {
            Debug.Log(hit.transform.name); // Log the name of the hit object

            // Different behavior based on the tag of the hit object
            if (hit.transform.CompareTag("Player")) return; // Ignore player hits

            // Handling hits on different surfaces or objects
            if (hit.collider.CompareTag("Ground"))
            {
                hitEffect.ShowHitEffect(hit, HitEffect.Effects.Hole, 4f); // Ground hit effect
            }
            else if (hit.transform.TryGetComponent<HitBox>(out var hitBox))
            {
                // Handle hits on objects with a HitBox component
                if (hit.collider.CompareTag("head")) // Extra damage for headshots
                {
                    hitBox.OnBulletHit(headDamage);
                }
                else
                {
                    hitBox.OnBulletHit(damage);
                }
                hitEffect.ShowHitEffect(hit, HitEffect.Effects.Blood, 3f); // Blood hit effect
            }
            else
            {
                hitEffect.ShowHitEffect(hit, HitEffect.Effects.Impact, 2f); // Default impact effect
            }
            if (hit.rigidbody != null)
            {
                // Apply force on impact
                hit.rigidbody.AddForceAtPosition(-hit.normal * impactForce, hit.point, ForceMode.Impulse);
            }
        }
    }

    public void CasingRelease()
    {
        // Functionality for releasing the casing
        if (!casingExitLocation || !casingPrefab) return; // Check if required references are set

        // Create and eject the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation);
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f, 1f);
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        // Destroy the casing after a set time
        Destroy(tempCasing, destroyTimer);
    }
}
