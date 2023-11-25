using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ZombieBotController : MonoBehaviour
{
    private enum ZombieState
    {
        Walking,
        Running,
        Attacking,
        Ragdoll,
    }
    private ZombieState currentState;

    private Transform targetTransform;
    private NavMeshAgent navMeshAgent;
    private NavMeshPath navMeshPath;

    public float rotationSpeed = 125;
    public float walkingSpeed => Random.Range(0.4f, 0.78f);
    public float runningSpeed => Random.Range(0.78f, 1f);
    public float runningOuterProximity = 6f;
    public float xzAttackProximity = 1.32f;
    public float yAttackProximity = 1.5f;
    //public float attackDamage = 10f;

    private Animator animator;
    private int isWalkingHash;
    private int isRunningHash;
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
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        navMeshAgent.SetDestination(targetTransform.position);
        navMeshPath = new NavMeshPath();

        runningOuterProximity = Random.Range(3f, 9f);
        navMeshAgent.speed = walkingSpeed;
        navMeshAgent.angularSpeed = rotationSpeed;
        navMeshAgent.stoppingDistance = xzAttackProximity;

        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        characterController = GetComponent<CharacterController>();

        currentHealth = maxHealth;
        currentState = ZombieState.Walking;

        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isAttackingHash = Animator.StringToHash("isAttacking");

        attackAudioSource = GetComponents<AudioSource>().First(e => e.clip.name == "Zombieattack 1");
        hurtAudioSource = GetComponents<AudioSource>().First(e => e.clip.name == "ZombieHurt");

        DisableRagdoll();
    }

    void Update()
    {
        switch (currentState)
        {
            case ZombieState.Walking:
                WalkingBehaviour();
                break;
            case ZombieState.Running:
                RunningBehaviour();
                break;
            case ZombieState.Attacking:
                AttackingBehaviour();
                break;
            case ZombieState.Ragdoll:
                RagdollBehaviour();
                break;
        }
    }

    private void DisableRagdoll()
    {
        foreach (var rigidbody in ragdollRigidbodies)
            rigidbody.isKinematic = true;

        navMeshAgent.enabled = true;
        animator.enabled = true;
        characterController.enabled = true;
    }

    private void EnableRagdoll()
    {
        foreach (var rigidbody in ragdollRigidbodies)
            rigidbody.isKinematic = false;

        navMeshAgent.enabled = false;
        animator.enabled = false;
        characterController.enabled = false;
    }

    private bool HasClearPath()
    {
        NavMesh.CalculatePath(transform.position, targetTransform.position, NavMesh.AllAreas, navMeshPath);

        // Check if the path is simple (mostly straight) or not
        if (navMeshPath.status == NavMeshPathStatus.PathComplete)
        {
            // You can define 'simplicity' based on the number of corners
            return navMeshPath.corners.Length <= 2 || IsPathMostlyStraight();
        }
        return false;
    }

    private bool IsPathMostlyStraight()
    {
        // This method can be refined based on your game's needs.
        // A simple implementation could be to check the angle between each corner.
        for (int i = 0; i < navMeshPath.corners.Length - 2; i++)
        {
            Vector3 directionToNextCorner = (navMeshPath.corners[i + 1] - navMeshPath.corners[i]).normalized;
            Vector3 directionToCornerAfterNext = (navMeshPath.corners[i + 2] - navMeshPath.corners[i + 1]).normalized;

            if (Vector3.Angle(directionToNextCorner, directionToCornerAfterNext) > 45) // Threshold angle
            {
                return false; // Path is not mostly straight
            }
        }
        return true; // Path is mostly straight
    }

    private void StartWalking()
    {
        animator.SetBool(isAttackingHash, false);
        animator.SetBool(isRunningHash, false);
        animator.SetBool(isWalkingHash, true);
        navMeshAgent.speed = walkingSpeed;
        currentState = ZombieState.Walking;
    }

    private void WalkingBehaviour()
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);
        if (distanceToTarget > navMeshAgent.stoppingDistance)
        {
            if (distanceToTarget > runningOuterProximity && HasClearPath())
            {
                StartRunning();
            }
            else
            {
                navMeshAgent.SetDestination(targetTransform.position);
            }
        }
        else
            StartAttacking();
    }

    private void StartRunning()
    {
        animator.SetBool(isAttackingHash, false);
        animator.SetBool(isWalkingHash, true);
        animator.SetBool(isRunningHash, true);
        navMeshAgent.speed = runningSpeed;
        currentState = ZombieState.Running;
    }

    private void RunningBehaviour()
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);
        if (distanceToTarget > navMeshAgent.stoppingDistance)
        {
            if (!(distanceToTarget > runningOuterProximity && HasClearPath()))
            {
                StartWalking();
            }
            else
            {
                navMeshAgent.SetDestination(targetTransform.position);
            }
        }
        else
            StartAttacking();
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

    private void StartAttacking()
    {
        animator.SetBool(isRunningHash, false);
        animator.SetBool(isWalkingHash, false);
        animator.SetBool(isAttackingHash, true);
        navMeshAgent.speed = walkingSpeed;
        currentState = ZombieState.Attacking;
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

            // After attacking, you might want to switch back to Walking or another state
        }
        else
            StartWalking();
    }
}
