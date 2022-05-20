using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Note : Interactable
{
    public Dialogue dialogue; // this is shown when the player opens the note

    bool opened = false;

    public override void MousePressed(InputAction.CallbackContext context)
    {
        if (!opened)
        {
            PlayerController.Instance.noteUIController.Open();
        } else
        {
            PlayerController.Instance.noteUIController.GoToNextPage();
        }
    }

    public override void MouseHeld(InputAction.CallbackContext context)
    {

    }

    public override void MouseReleased(InputAction.CallbackContext context)
    {

    }

}
