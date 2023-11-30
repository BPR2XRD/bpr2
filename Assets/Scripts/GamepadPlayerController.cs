using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadPlayerController : MonoBehaviour
{
    private enum GamepadPlayerState
    {
        Selecting,
        Playing,
    }
    private GamepadPlayerState currentState;

    private enum GamepadPlayerPerspectiveState
    {
        FirstPerson,
        ThirdPerson,
        TransitioningToFirstPerson,
        TransitioningToThirdPerson
    }
    private GamepadPlayerPerspectiveState currentPerspectiveState;

    private PlayerInput playerInput;
    private GamepadInputActions gamepadInputActions;
    private Camera camera;

    private GameObject? selectedZombieBot = null;
    private ZombieBotController? selectedZombieBotController = null;
    private GameObject? lastSelectedZombieBot = null;

    public float cameraPositioningSmoothFactor = 1f;
    public float cameraRotationalSmoothFactor = 2f;

    private float perspectiveTransitionDuration = 1.0f;
    private float perspectiveTransitionTimer = 0.0f;
    private Vector3 perspectiveTransitionStartPosition;
    private Vector3 perspectiveTransitionEndPosition;
    private Quaternion perspectiveTransitionStartRotation;
    private Quaternion perspectiveTransitionEndRotation;

    private void Awake()
    {
        gamepadInputActions = new GamepadInputActions();
    }

    void Start()
    {
        currentState = GamepadPlayerState.Selecting;
        currentPerspectiveState = GamepadPlayerPerspectiveState.ThirdPerson;

        playerInput = GetComponent<PlayerInput>();
        camera = GetComponentInChildren<Camera>();
    }

    private void OnEnable()
    {
        var moveAction = gamepadInputActions.ZombieBotGameplay.Move;
        moveAction.performed += OnMovePerformed;
        moveAction.Enable();
    }

    private void OnDisable()
    {
        var moveAction = gamepadInputActions.ZombieBotGameplay.Move;
        moveAction.performed -= OnMovePerformed;
        moveAction.Disable();
    }

    private void OnNewZombieBotSelection()
    {
        if (currentState != GamepadPlayerState.Selecting)
            return;

        SelectRandomAliveNotControlledZombieBot();
    }

    private void OnChooseZombieBotSelection()
    {
        if (
            currentState != GamepadPlayerState.Selecting &&
            selectedZombieBot != null &&
            selectedZombieBotController != null &&
            !selectedZombieBotController.isDead &&
            selectedZombieBotController.currentState != ZombieBotController.ZombieState.Controlled
        )
            return;

        playerInput.SwitchCurrentActionMap("ZombieBotGameplay");
        selectedZombieBotController.StartControlling();
        currentState = GamepadPlayerState.Playing;
    }

    private void OnTogglePerspective()
    {
        if (currentState != GamepadPlayerState.Playing || selectedZombieBot == null || selectedZombieBotController == null || selectedZombieBotController.isDead)
            return;

        // Ignore toggle requests during transitions
        if (currentPerspectiveState == GamepadPlayerPerspectiveState.TransitioningToFirstPerson || currentPerspectiveState == GamepadPlayerPerspectiveState.TransitioningToThirdPerson)
            return;

        if (currentPerspectiveState == GamepadPlayerPerspectiveState.FirstPerson)
        {
            // Transition to Third-Person
            currentPerspectiveState = GamepadPlayerPerspectiveState.TransitioningToThirdPerson;
            perspectiveTransitionStartPosition = camera.transform.position;
            perspectiveTransitionStartRotation = camera.transform.rotation;
            perspectiveTransitionEndPosition = selectedZombieBot.transform.position - selectedZombieBot.transform.forward * 10 + Vector3.up * 5;
            perspectiveTransitionEndRotation = Quaternion.LookRotation(selectedZombieBot.transform.position - perspectiveTransitionEndPosition);
        }
        else
        {
            // Transition to First-Person
            currentPerspectiveState = GamepadPlayerPerspectiveState.TransitioningToFirstPerson;
            perspectiveTransitionStartPosition = camera.transform.position;
            perspectiveTransitionStartRotation = camera.transform.rotation;
            Vector3 eyePositionOffset = new Vector3(0, 1.6f, 0); // Adjust as needed
            perspectiveTransitionEndPosition = selectedZombieBot.transform.position + eyePositionOffset;
            perspectiveTransitionEndRotation = selectedZombieBot.transform.rotation;
        }

        perspectiveTransitionTimer = 0f;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        if (context.control.device != playerInput.devices.FirstOrDefault())
            return;

        if (currentState != GamepadPlayerState.Playing || selectedZombieBot == null || selectedZombieBotController == null || selectedZombieBotController.isDead)
            return;
        
        selectedZombieBotController.controlledMovementVector = context.ReadValue<Vector2>();
    }

    private void OnRun()
    {
        if (currentState != GamepadPlayerState.Playing || selectedZombieBot == null || selectedZombieBotController == null || selectedZombieBotController.isDead)
            return;

        selectedZombieBotController.OnControlledRun();
    }

    private void OnAttack()
    {
        if (currentState != GamepadPlayerState.Playing || selectedZombieBot == null || selectedZombieBotController == null || selectedZombieBotController.isDead)
            return;

        selectedZombieBotController.OnControlledAttack();
    }

    void Update()
    {
        if (selectedZombieBot != null && selectedZombieBotController != null && !selectedZombieBotController.isDead)
        {
            if (currentPerspectiveState == GamepadPlayerPerspectiveState.TransitioningToFirstPerson || currentPerspectiveState == GamepadPlayerPerspectiveState.TransitioningToThirdPerson)
            {
                perspectiveTransitionTimer += Time.deltaTime;
                float progress = perspectiveTransitionTimer / perspectiveTransitionDuration;

                camera.transform.position = Vector3.Lerp(perspectiveTransitionStartPosition, perspectiveTransitionEndPosition, progress);
                camera.transform.rotation = Quaternion.Lerp(perspectiveTransitionStartRotation, perspectiveTransitionEndRotation, progress);

                if (perspectiveTransitionTimer >= perspectiveTransitionDuration)
                {
                    currentPerspectiveState = (currentPerspectiveState == GamepadPlayerPerspectiveState.TransitioningToFirstPerson) ? GamepadPlayerPerspectiveState.FirstPerson : GamepadPlayerPerspectiveState.ThirdPerson;
                }
            }
            else if (currentPerspectiveState == GamepadPlayerPerspectiveState.FirstPerson)
            {
                // First-person camera logic
                Vector3 eyePositionOffset = new Vector3(0, 1.65f, 0); // Approximate eye level height
                camera.transform.position = selectedZombieBot.transform.position + eyePositionOffset;
                camera.transform.rotation = selectedZombieBot.transform.rotation;
            }
            else if (currentPerspectiveState == GamepadPlayerPerspectiveState.ThirdPerson)
            {
                // Third-person camera logic
                Vector3 desiredPosition = selectedZombieBot.transform.position - selectedZombieBot.transform.forward * 10 + Vector3.up * 5;
                Vector3 smoothedPosition = Vector3.Lerp(camera.transform.position, desiredPosition, cameraPositioningSmoothFactor * Time.deltaTime);
                camera.transform.position = smoothedPosition;

                Quaternion desiredRotation = Quaternion.LookRotation(selectedZombieBot.transform.position - camera.transform.position);
                Quaternion smoothedRotation = Quaternion.Slerp(camera.transform.rotation, desiredRotation, cameraRotationalSmoothFactor * Time.deltaTime);
                camera.transform.rotation = smoothedRotation;
            }
        }
        else
        {
            currentPerspectiveState = GamepadPlayerPerspectiveState.ThirdPerson;
            currentState = GamepadPlayerState.Selecting;
            SelectRandomAliveNotControlledZombieBot();
        }
    }

    private void SelectRandomAliveNotControlledZombieBot()
    {
        GameObject[] aliveZombieBotGameObjects = GameObject
            .FindGameObjectsWithTag("ZombieBot")
            .Where(zombieBot =>
            {
                var zombieBotController = zombieBot.GetComponent<ZombieBotController>();
                return zombieBotController != null && !zombieBotController.isDead && zombieBotController.currentState != ZombieBotController.ZombieState.Controlled;
            })
            .ToArray();

        if (aliveZombieBotGameObjects.Length > 1)
        {
            GameObject toBeSelectedZombieBot;
            do
            {
                toBeSelectedZombieBot = aliveZombieBotGameObjects[Random.Range(0, aliveZombieBotGameObjects.Length)];
            } while (toBeSelectedZombieBot == lastSelectedZombieBot); // Ensure toBeSelectedZombieBot is different from the last one

            lastSelectedZombieBot = toBeSelectedZombieBot;
            selectedZombieBot = toBeSelectedZombieBot;
            selectedZombieBotController = toBeSelectedZombieBot.GetComponent<ZombieBotController>();
        }
        else if (aliveZombieBotGameObjects.Length == 1)
        {
            // If there's only one zombie bot select it
            lastSelectedZombieBot = aliveZombieBotGameObjects[0];
            selectedZombieBot = aliveZombieBotGameObjects[0];
            selectedZombieBotController = aliveZombieBotGameObjects[0].GetComponent<ZombieBotController>();
        }
    }

    private void OnDestroy()
    {
        // If gamepad gets disconnected and the controlled zombie bot is still alive set the state of the zombie bot to ai walking
        if (
            currentState == GamepadPlayerState.Playing &&
            selectedZombieBot != null &&
            selectedZombieBotController != null &&
            !selectedZombieBotController.isDead
        )
            selectedZombieBotController.StartAiWalking();
    }
}
