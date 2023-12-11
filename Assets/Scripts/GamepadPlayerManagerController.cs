using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadPlayerManagerController : MonoBehaviour
{
    internal void Start()
    {
        // Find all pre-connected gamepads and spawn a gamepad player for each one
        var gamepads = Gamepad.all;
        if (gamepads.Count < PlayerInputManager.instance.maxPlayerCount + 1)
        {
            foreach (var gamepad in gamepads)
                TryJoinPlayer(gamepad);
        }
    }

    internal void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChanged;
    }

    internal void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChanged;
    }

    private void OnPlayerJoined(PlayerInput player)
    {
        // Debug.Log("Joined " + player.playerIndex + " - " + player.devices[0].displayName);
    }

    private void OnPlayerLeft(PlayerInput player)
    {
        // Debug.Log("Disconnected " + player.playerIndex);
    }

    internal void OnDeviceChanged(InputDevice device, InputDeviceChange change)
    {
        // If the newly added device is not a gamepad exit early
        if (!(device is Gamepad))
            return;

        switch (change)
        {
            case InputDeviceChange.Added:
                TryJoinPlayer(device);
                break;
            case InputDeviceChange.Removed:
                TryRemovePlayer(device);
                break;
            default:
                break;
        }
    }

    internal void TryJoinPlayer(InputDevice device)
    {
        if (!HasGamepadAlreadyJoined(device))
            PlayerInputManager.instance.JoinPlayer(-1, -1, null, device);
    }

    internal bool HasGamepadAlreadyJoined(InputDevice device)
    {
        return PlayerInput.all.Any(playerInput => playerInput.devices.Contains(device));
    }

    internal void TryRemovePlayer(InputDevice device)
    {
        var playerInput = PlayerInput.all.FirstOrDefault(playerInput => playerInput.devices.Contains(device));
        if (playerInput != null)
        {
            // Destroy the prefab associated with the player input
            Destroy(playerInput.gameObject);
        }
    }
}
