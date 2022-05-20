using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

// PlayerInputHanlder is an intermediary script for handling input from player.
// All of the functions in this script are called from PlayerInput,
// which is a script from Unity's new input system. Whene the player provides
// an input that is mapped in InputActions, PlayerInput will invoke
// events (C# functions) that are defined here in PlayerInputHandler
[RequireComponent(typeof(PlayerInput))]
public class PlayerInputHandler : Singleton<PlayerInputHandler>
{
    [HideInInspector]
    public PlayerInput playerInput;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    // =====================================================================
    // ======================== "Play" Action Map ==========================
    // =====================================================================
    public void OnMove(CallbackContext context)
    {
        PlayerController.Instance.OnMove(context);
    }

    public void OnSprint(CallbackContext context)
    {
        PlayerController.Instance.OnSprint(context);
    }

    public void OnInteract(CallbackContext context)
    {
        PlayerController.Instance.OnInteract(context);
    }

    // This function is called by mouse movement or by the right stick on
    // a gamepad
    public void OnMouseMove(CallbackContext context)
    {
        PlayerController.Instance.OnMouseMove(context);
    }

    // =====================================================================
    // ====================== "Note UI" Action Map =========================
    // =====================================================================

    public void OnGoToNextPage(CallbackContext context)
    {
        PlayerController.Instance.OnGoToNextPage(context);
    }

    public void OnGoToPreviousPage(CallbackContext context)
    {
        PlayerController.Instance.OnGoToPreviousPage(context);
    }

    public void OnClose(CallbackContext context)
    {
        PlayerController.Instance.OnClose(context);
    }
}
