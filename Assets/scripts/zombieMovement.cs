using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class zombieMovement : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isRunningHash;

    ControllerInput input;

    Vector2 currentMovement;

    bool movementPressed;
    bool runPressed;

    private void Awake()
    {
        input = new ControllerInput();

        input.ZombieControls.Movement.performed += ctx =>
        {
            currentMovement = ctx.ReadValue<Vector2>();
            movementPressed = currentMovement.x != 0 || currentMovement.y != 0;
        };

        input.ZombieControls.Run.performed += ctx => runPressed = ctx.ReadValueAsButton();

        //input.ZombieControl.Movement.performed += ctx => Debug.Log(ctx.ReadValueAsObject());
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
        handleMovement();
        handleRotation();
    }

    void handleRotation()
    {
        Vector3 currentPosition = transform.position;

        Vector3 newPosition = new Vector3(currentMovement.x, 0, currentMovement.y);

        Vector3 positionToLookAt = currentPosition + newPosition;

        transform.LookAt(positionToLookAt);
    }

    void handleMovement()
    {
        bool isRunning = animator.GetBool(isRunningHash);
        bool isWalking = animator.GetBool(isWalkingHash);

        if (movementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);

        }

        if (!movementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);

        }

        if ((movementPressed && runPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);

        }
        if ((!movementPressed && !runPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);

        }
    }
    private void OnEnable()
    {
        input.ZombieControls.Enable();
    }

    void OnDisable()
    {
        input.ZombieControls.Disable();
    }
}
