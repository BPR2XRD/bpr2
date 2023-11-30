using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ZombieBotController : MonoBehaviour
{
    public enum ZombieState
    {
        AiWalking,
        AiRunning,
        AiAttacking,
        Controlled,
        Ragdoll,
    }
    public ZombieState currentState;

    private Transform targetTransform;
    private PlayerHealth playerHealth;
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
    public float controlledMovementSpeed = 2.0f;
    private float currentHealth;
    public bool isDead = false;

    private AudioSource attackAudioSource;
    private float attackSoundCooldown = 2.0f; // 2 seconds cooldown
    private float lastAttackSoundTime = -1.0f; // Time when last attack sound was played
    private AudioSource hurtAudioSource;

    public Vector2 controlledMovementVector;
    private bool isControlledRunning;

    void Awake()
    {
        targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = targetTransform.GetComponent<PlayerHealth>();
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        navMeshAgent.SetDestination(targetTransform.position);
        navMeshPath = new NavMeshPath();

        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        characterController = GetComponent<CharacterController>();

        attackAudioSource = GetComponents<AudioSource>().First(e => e.clip.name == "Zombieattack 1");
        hurtAudioSource = GetComponents<AudioSource>().First(e => e.clip.name == "ZombieHurt");

        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isAttackingHash = Animator.StringToHash("isAttacking");

        runningOuterProximity = Random.Range(3f, 9f);
        currentHealth = maxHealth;

        StartAiWalking();
    }

    void Update()
    {
        switch (currentState)
        {
            case ZombieState.AiWalking:
                AiWalkingBehaviour();
                break;
            case ZombieState.AiRunning:
                RunningAiBehaviour();
                break;
            case ZombieState.AiAttacking:
                AiAttackingBehaviour();
                break;
            case ZombieState.Controlled:
                ControllingBehaviour();
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

    public void StartAiWalking()
    {
        DisableRagdoll();
        animator.SetBool(isAttackingHash, false);
        animator.SetBool(isRunningHash, false);
        animator.SetBool(isWalkingHash, true);
        navMeshAgent.enabled = true;
        navMeshAgent.speed = walkingSpeed;
        navMeshAgent.angularSpeed = rotationSpeed;
        navMeshAgent.stoppingDistance = xzAttackProximity;

        currentState = ZombieState.AiWalking;
    }

    private void AiWalkingBehaviour()
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);
        if (distanceToTarget > navMeshAgent.stoppingDistance)
        {
            if (distanceToTarget > runningOuterProximity && HasClearPath())
            {
                StartAiRunning();
            }
            else
            {
                navMeshAgent.SetDestination(targetTransform.position);
            }
        }
        else if (!playerHealth.isDead)
        {
            StartAiAttacking();
        }
    }

    private void StartAiRunning()
    {
        animator.SetBool(isAttackingHash, false);
        animator.SetBool(isWalkingHash, true);
        animator.SetBool(isRunningHash, true);
        navMeshAgent.speed = runningSpeed;
        navMeshAgent.enabled = true;
        navMeshAgent.speed = walkingSpeed;
        navMeshAgent.angularSpeed = rotationSpeed;
        navMeshAgent.stoppingDistance = xzAttackProximity;

        currentState = ZombieState.AiRunning;
    }

    private void RunningAiBehaviour()
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);
        if (distanceToTarget > navMeshAgent.stoppingDistance)
        {
            if (!(distanceToTarget > runningOuterProximity && HasClearPath()))
            {
                StartAiWalking();
            }
            else
            {
                navMeshAgent.SetDestination(targetTransform.position);
            }
        }
        else if (!playerHealth.isDead)
        {
            StartAiAttacking();
        }
    }

    private void AttackAction()
    {
        if (Time.time >= lastAttackSoundTime + attackSoundCooldown && !attackAudioSource.isPlaying)
        {
            attackAudioSource.Play();
            lastAttackSoundTime = Time.time;
            playerHealth.TakeDamage(5); //reduce player health
        }
    }

    private void StartAiAttacking()
    {
        animator.SetBool(isRunningHash, false);
        animator.SetBool(isWalkingHash, false);
        animator.SetBool(isAttackingHash, true);

        navMeshAgent.enabled = true;
        navMeshAgent.speed = walkingSpeed;
        navMeshAgent.angularSpeed = rotationSpeed;
        navMeshAgent.stoppingDistance = xzAttackProximity;

        currentState = ZombieState.AiAttacking;
    }

    private void AiAttackingBehaviour()
    {
        // Calculate the differences in each axis
        float deltaX = Mathf.Abs(transform.position.x - targetTransform.position.x);
        float deltaY = Mathf.Abs(transform.position.y - targetTransform.position.y);
        float deltaZ = Mathf.Abs(transform.position.z - targetTransform.position.z);

        // Calculate the distance in the XZ plane
        float distanceXZ = Mathf.Sqrt(deltaX * deltaX + deltaZ * deltaZ);

        // Check if the zombie is within the specified proximities
        if (distanceXZ <= xzAttackProximity && deltaY <= yAttackProximity && !playerHealth.isDead)
        {
            AttackAction();
            animator.SetBool(isAttackingHash, true); // Trigger attack animation
            // After AiAttacking, you might want to switch back to AiWalking or another state
        }
        else
            StartAiWalking();
    }

    public void StartControlling()
    {
        animator.SetBool(isRunningHash, false);
        animator.SetBool(isWalkingHash, false);
        animator.SetBool(isAttackingHash, false);

        animator.enabled = true;
        characterController.enabled = true;
        navMeshAgent.enabled = false;

        currentState = ZombieState.Controlled;
    }

    private void ControllingBehaviour()
    {
        // Calculate the direction the zombie should move in based on input
        Vector3 moveDirection = transform.forward * controlledMovementVector.y + transform.right * controlledMovementVector.x;
        moveDirection.Normalize(); // Ensure the movement speed is consistent

        // Apply movement speed
        moveDirection *= controlledMovementSpeed;

        if (moveDirection.magnitude > 0.1f) // Deadzone to prevent jitter from small inputs
        {
            bool isWalking = animator.GetBool(isWalkingHash);
            bool isRunning = animator.GetBool(isRunningHash);

            // Move the zombie using the CharacterController component
            characterController.Move(moveDirection * Time.deltaTime);

            // Calculate a target rotation based on the move direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            // Slerp to the target rotation at the specified rotation speed
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Set the animations
            if (!isWalking)
            {
                animator.SetBool(isWalkingHash, true);
            }
            else if (isControlledRunning && !isRunning && isWalking)
            {
                animator.SetBool(isRunningHash, true);
            }
        }
        else
        {
            // Stop if there's no movement
            animator.SetBool(isWalkingHash, false);
            animator.SetBool(isRunningHash, false);
            isControlledRunning = false;
        }
    }

    public void OnControlledRun()
    {
        isControlledRunning = true;
    }

    public void OnControlledAttack()
    {
        animator.SetBool(isAttackingHash, true);
        AttackAction();
    }

    public void TakeDamage(int amount)
    {
        hurtAudioSource.Play();
        currentHealth -= amount;
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
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
}
