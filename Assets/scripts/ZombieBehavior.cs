using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ZombieBehavior : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isRunningHash;
    public AudioSource source1;
    public AudioClip audioClipAttack;
    public AudioSource source2;
    public AudioClip audioClipScream;
    ZombieControl input;

    Vector2 currentMovement;

    bool movementPressed;
    bool runPressed;
    private bool isAttacking = false;
    private bool isScreaming = false;

    [SerializeField]
    private float animationFinishTime = 0.9f;

    private void Awake()
    {
        input = new ZombieControl();

        input.ZombieMovement.Movement.performed += ctx =>
        {
            currentMovement = ctx.ReadValue<Vector2>();
            movementPressed = currentMovement.magnitude > 0.1f; // Check if the joystick is moved enough
        };

        input.ZombieMovement.Run.performed += ctx => runPressed = true;
        input.ZombieMovement.Run.canceled += ctx => runPressed = false;

        input.ZombieMovement.Attack.performed += ctx => Attack();
        input.ZombieMovement.Scream.performed += ctx => Scream();
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");


    }

    // Update is called once per frame
    void Update()
    {
        // Assuming "isAttacking" is on layer 1. Change the index if it's on another layer.
        if (isAttacking && animator.GetCurrentAnimatorStateInfo(1).normalizedTime >= animationFinishTime)
        {
            isAttacking = false;
        }
        handleMovement();
        handleRotation();
    }

    void handleRotation()
    {
        float rotationSpeed = 100f; // Adjust to your needs
        float rotationThreshold = 0.3f; // Dead zone threshold

        if (currentMovement.x > rotationThreshold)
        {
            // Rotate to the right
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
        else if (currentMovement.x < -rotationThreshold)
        {
            // Rotate to the left
            transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime);
        }
    }
    void Attack()
    {
        Debug.Log("Attack button pressed");
        if (!isAttacking && AnimatorHasParameter("isAttacking", animator))
        {
            if(!source2.isPlaying){
                source1.PlayOneShot(audioClipAttack);
            }
            Debug.Log("Character Attacks");
            animator.SetTrigger("isAttacking");
            isAttacking = true;
        }
        else if (isAttacking)
        {
            Debug.Log("Character is already attacking");
        }
        else if (!AnimatorHasParameter("isAttacking", animator))
        {
            Debug.Log("Animator does not have the 'isAttacking' parameter");
        }
    }
    void Scream(){
        source2.PlayOneShot(audioClipScream);
    }

    IEnumerator InitialiseAttack()
    {
        yield return new WaitForSeconds(0.1f);
        isAttacking = true;
    }
    private bool AnimatorHasParameter(string paramName, Animator animator)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName) return true;
        }
        return false;
    }
    void handleMovement()
    {
        bool isRunning = animator.GetBool(isRunningHash);
        bool isWalking = animator.GetBool(isWalkingHash);

        if (movementPressed)
        {
            if (currentMovement.y > 0)
            {
                // Forward movement
                transform.Translate(Vector3.forward * Time.deltaTime);
            }
            else if (currentMovement.y < 0)
            {
                // Backward movement
                transform.Translate(Vector3.back * Time.deltaTime);
            }

            // Set the animations
            if (!runPressed && !isWalking)
            {
                animator.SetBool(isWalkingHash, true);
                animator.SetBool(isRunningHash, true);
            }
            else if (runPressed && !isRunning)
            {
                animator.SetBool(isWalkingHash, false);
                animator.SetBool(isRunningHash, true);
            }
        }
        else
        {
            animator.SetBool(isWalkingHash, false);
            animator.SetBool(isRunningHash, false);
        }
    }
    private void OnEnable()
    {
        input.ZombieMovement.Enable();
    }

    void OnDisable()
    {
        input.ZombieMovement.Disable();
    }
}
