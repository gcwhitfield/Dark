using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

// PlayerInputHanlder is an intermediate script for handling input from player.
// All of the functions in this script are called from PlayerInput,
// which is a script from Unity's new input system. Whene the player provides
// an input that is mapped in InputActions, PLayerInput will invoke
// events (C# functions) that are defined here in PlayerInputHandler
[RequireComponent(typeof(PlayerInput))]
public class PlayerInputHandler : MonoBehaviour
{
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

    public void OnMouseMove(CallbackContext context)
    {
        PlayerController.Instance.OnMouseMove(context);
    }
}
