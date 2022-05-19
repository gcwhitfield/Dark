using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Note : Interactable
{
    public override void MousePressed(InputAction.CallbackContext context)
    {
        PlayerController.Instance.OpenNoteUI();
    }

    public override void MouseHeld(InputAction.CallbackContext context)
    {

    }

    public override void MouseReleased(InputAction.CallbackContext context)
    {

    }
}
