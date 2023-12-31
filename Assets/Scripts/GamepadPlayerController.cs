using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Color = UnityEngine.Color;

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

    void Start()
    {
        currentState = GamepadPlayerState.Selecting;
        currentPerspectiveState = GamepadPlayerPerspectiveState.ThirdPerson;

        playerInput = GetComponent<PlayerInput>();
        camera = GetComponentInChildren<Camera>();
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
            currentState != GamepadPlayerState.Selecting ||
            selectedZombieBot == null ||
            selectedZombieBotController == null ||
            selectedZombieBotController.isDead ||
            selectedZombieBotController.currentState == ZombieBotController.ZombieState.Controlled
        )
            return;

        playerInput.SwitchCurrentActionMap("ZombieBotGameplay");
        selectedZombieBotController.StartControlling();
        currentState = GamepadPlayerState.Playing;
        if ( TryGetComponent(out Outline outline))
        {
            outline.enabled = true;
            outline.OutlineColor = playerInput.playerIndex switch
            {
                0 => Color.red,
                1 => Color.green,
                2 => Color.yellow,
                3 => Color.blue,
                _ => Color.magenta,
            };
        }
        OnTogglePerspective(); //transition to First-person after selecting

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
            ShowHead();
            // Transition to Third-Person
            currentPerspectiveState = GamepadPlayerPerspectiveState.TransitioningToThirdPerson;
            perspectiveTransitionStartPosition = camera.transform.position;
            perspectiveTransitionStartRotation = camera.transform.rotation;
            perspectiveTransitionEndPosition = selectedZombieBot.transform.position - selectedZombieBot.transform.forward * 7 + Vector3.up * 5;
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

    public void OnMove(InputValue inputValue)
    {
        selectedZombieBotController.controlledMovementVector = inputValue.Get<Vector2>();
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
                    if (currentPerspectiveState == GamepadPlayerPerspectiveState.FirstPerson)
                    {
                        HideHead();
                    }
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
                Vector3 desiredPosition = selectedZombieBot.transform.position - selectedZombieBot.transform.forward * 7 + Vector3.up * 5;
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
            playerInput.SwitchCurrentActionMap("ZombieBotSelection");
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
            if (lastSelectedZombieBot != null)
            {
                var zombieBotController = lastSelectedZombieBot.GetComponent<ZombieBotController>();
                if (!zombieBotController.isDead && zombieBotController.currentState  != ZombieBotController.ZombieState.Controlled) 
                    lastSelectedZombieBot.GetComponent<Outline>().enabled = false;

            }
            lastSelectedZombieBot = toBeSelectedZombieBot;
            selectedZombieBot = toBeSelectedZombieBot;
            selectedZombieBotController = toBeSelectedZombieBot.GetComponent<ZombieBotController>();       
            var outline = selectedZombieBot.GetComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = playerInput.playerIndex switch
                 {
                     0 => Color.red,
                     1 => Color.green,
                     2 => Color.yellow,
                     3 => Color.blue,
                     _ => Color.magenta,
                 };
            outline.OutlineWidth = 5f;
            outline.enabled = true;
        }
        else if (aliveZombieBotGameObjects.Length == 1)
        {
            // If there's only one zombie bot select it
            lastSelectedZombieBot = aliveZombieBotGameObjects[0];
            selectedZombieBot = aliveZombieBotGameObjects[0];
            selectedZombieBotController = aliveZombieBotGameObjects[0].GetComponent<ZombieBotController>();
        }
    }

    private void ShowHead()
    {
        var zombie = selectedZombieBot.transform.Find("Head");
        zombie.gameObject.layer = LayerMask.NameToLayer("Default");
        camera.cullingMask |= 1 << LayerMask.NameToLayer(GetZombieLayer(playerInput.playerIndex));
    }

    private void HideHead()
    {
        var zombie = selectedZombieBot.transform.Find("Head");
        zombie.gameObject.layer = LayerMask.NameToLayer(GetZombieLayer(playerInput.playerIndex));
        camera.cullingMask &= ~(1 << LayerMask.NameToLayer(GetZombieLayer(playerInput.playerIndex)));
    }

    private string GetZombieLayer(int index)
    {
       return index switch
                 {
                     0 => "ZombieHead1",
                     1 => "ZombieHead2",
                     2 => "ZombieHead3",
                     3 => "ZombieHead4",
                     _ => "ZombieHead1",
                 };
}


    private void OnDestroy()
    {
        if (selectedZombieBot != null && selectedZombieBot.TryGetComponent(out Outline outline))
        { 
            outline.enabled=false;
        }
        // If gamepad gets disconnected and the controlled zombie bot is still alive set the state of the zombie bot to ai walking
        if (
            currentState == GamepadPlayerState.Playing &&
            selectedZombieBot != null &&
            selectedZombieBotController != null &&
            !selectedZombieBotController.isDead
        )
        {
            selectedZombieBotController.StartAiWalking();
        }
    }
}
