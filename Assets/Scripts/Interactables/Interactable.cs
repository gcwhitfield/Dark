using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public abstract class Interactable : MonoBehaviour
{
    public Sprite sprite; // the icon to display when the player looks
    // at this interactable

    public abstract void MousePressed(CallbackContext context);
    public abstract void MouseHeld(CallbackContext context);
    public abstract void MouseReleased(CallbackContext context);
}
