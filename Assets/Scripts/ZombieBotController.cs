using System.Linq;
using UnityEngine;

public class ZombieBotController : MonoBehaviour
{
    private enum ZombieState
    {
        Following,
        Ragdoll,
        Attacking,
    }
    private ZombieState currentState;

    private Transform targetTransform;

    public float rotationSpeed = 125;
    public float movementSpeed = 0.7f;
    public float xzAttackProximity = 0.85f;
    public float yAttackProximity = 1.5f;
    //public float attackDamage = 10f;

    private Animator animator;
    private int isWalkingHash;
    private int isAttackingHash;

    private Rigidbody[] ragdollRigidbodies;
    private CharacterController characterController;

    public float maxHealth = 5f;
    private float currentHealth;
    private bool isDead = false;

    private AudioSource attackAudioSource;
    private float attackSoundCooldown = 2.0f; // 2 seconds cooldown
    private float lastAttackSoundTime = -1.0f; // Time when last attack sound was played

    private AudioSource hurtAudioSource;

    void Awake()
    {
        targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        characterController = GetComponent<CharacterController>();

        currentHealth = maxHealth;
        currentState = ZombieState.Following;

        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isAttackingHash = Animator.StringToHash("isAttacking");

        attackAudioSource = GetComponents<AudioSource>().First(e => e.clip.name == "Zombieattack 1");
        hurtAudioSource = GetComponents<AudioSource>().First(e => e.clip.name == "Zombie Hurt (Nr. 1 Minecraft Sound) - Sound Effect for editing");

        DisableRagdoll();
    }

    void Update()
    {
        switch (currentState)
        {
            case ZombieState.Following:
                FollowingBehaviour();
                break;
            case ZombieState.Ragdoll:
                RagdollBehaviour();
                break;
            case ZombieState.Attacking:
                AttackingBehaviour();
                break;
        }
    }

    private void DisableRagdoll()
    {
        foreach (var rigidbody in ragdollRigidbodies)
            rigidbody.isKinematic = true;

        animator.enabled = true;
        characterController.enabled = true;
    }

    private void EnableRagdoll()
    {
        foreach (var rigidbody in ragdollRigidbodies)
            rigidbody.isKinematic = false;

        animator.enabled = false;
        characterController.enabled = false;
    }

    private void FollowingBehaviour()
    {
        // Calculate the differences in x and z axis
        float deltaX = Mathf.Abs(transform.position.x - targetTransform.position.x);
        float deltaZ = Mathf.Abs(transform.position.z - targetTransform.position.z);

        // Calculate the distance in the XZ plane
        float distanceXZ = Mathf.Sqrt(deltaX * deltaX + deltaZ * deltaZ);

        Vector3 direction = targetTransform.position - transform.position;
        direction.y = 0;
        if (distanceXZ > xzAttackProximity) // Check if the zombie is a certain distance away from the player before walking
        {
            direction.Normalize();
            animator.SetBool(isWalkingHash, true); // Set the isWalking parameter to true
            characterController.Move(direction * movementSpeed * Time.deltaTime); // Move the zombie towards the player

            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool(isWalkingHash, false);
            currentState = ZombieState.Attacking;
        }
    }

    public void TakeDamage(int amount)
    {
        hurtAudioSource.Play();
        currentHealth -= amount;
        if (currentHealth <= 0 && !isDead)
        {
            EnableRagdoll();
            currentState = ZombieState.Ragdoll;
        }
    }

    private void RagdollBehaviour()
    {
        Vector3 hitPoint = new Vector3(0, 0, 0); // Default hit point
        Vector3 force = new Vector3(0, 3, 0); // Default force

        Rigidbody hitRigidbody = ragdollRigidbodies.OrderBy(rigidbody => Vector3.Distance(rigidbody.position, hitPoint)).First();
        hitRigidbody.AddForceAtPosition(force, hitPoint, ForceMode.Impulse);

        if (TryGetComponent(out Disolve disolve))
        {
            disolve.enabled = true;
        }
    }

    private void PlayAttackSound()
    {
        if (Time.time >= lastAttackSoundTime + attackSoundCooldown && !attackAudioSource.isPlaying)
        {
            attackAudioSource.Play();
            lastAttackSoundTime = Time.time;
        }
    }

    private void AttackingBehaviour()
    {
        // Calculate the differences in each axis
        float deltaX = Mathf.Abs(transform.position.x - targetTransform.position.x);
        float deltaY = Mathf.Abs(transform.position.y - targetTransform.position.y);
        float deltaZ = Mathf.Abs(transform.position.z - targetTransform.position.z);

        // Calculate the distance in the XZ plane
        float distanceXZ = Mathf.Sqrt(deltaX * deltaX + deltaZ * deltaZ);

        // Check if the zombie is within the specified proximities
        if (distanceXZ <= xzAttackProximity && deltaY <= yAttackProximity)
        {
            PlayAttackSound();
            animator.SetBool(isAttackingHash, true); // Trigger attack animation
            // Implement attack logic here
            // Example: Reduce player's health

            // After attacking, you might want to switch back to Following or another state
        }
        else
        {
            animator.SetBool(isAttackingHash, false);
            currentState = ZombieState.Following;
        }
    }
}
