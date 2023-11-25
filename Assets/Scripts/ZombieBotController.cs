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

    [SerializeField]
    private Transform targetTransform;

    public float rotationSpeed = 125;
    public float movementSpeed = 0.7f;
    public float attackProximity = 0.85f;
    //public float attackDamage = 10f;

    private Animator animator;
    private int isWalkingHash;
    private int isAttackingHash;

    private Rigidbody[] ragdollRigidbodies;
    private CharacterController characterController;

    public float maxHealth = 10f;
    private float currentHealth;
    public AudioSource audioSource;
    public AudioClip ZombieHurt;
    private bool isDead = false;

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
        Vector3 direction = targetTransform.position - transform.position;
        direction.y = 0;
        if (direction.magnitude > attackProximity) // Check if the zombie is a certain distance away from the player before walking
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
        audioSource.PlayOneShot(ZombieHurt);
        currentHealth -= amount;
        Debug.Log("health:" + currentHealth);
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

    private void AttackingBehaviour()
    {
        if (Vector3.Distance(transform.position, targetTransform.position) <= attackProximity)
        {
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
