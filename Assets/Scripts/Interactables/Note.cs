using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Note : Interactable
{
    public Dialogue dialogue; // this is shown when the player opens the note

    bool opened = false;

    void OnAwake()
    {
        if (dialogue == null)
        {
            Debug.LogError("'Dialogue' of Note.cs is set to null!");
        }
    }
    public override void MousePressed(InputAction.CallbackContext context)
    {
        if (dialogue != null)
        {
            if (!opened)
            {
                PlayerController.Instance.noteUIController.Open(dialogue);

                // switch the action map to "Note UI". Navigation of Note UI is handled
                // in PlayerController.cs
                PlayerInputHandler.Instance.playerInput.SwitchCurrentActionMap("Note UI");
            }
        }
    }

    public override void MouseHeld(InputAction.CallbackContext context) { }
    public override void MouseReleased(InputAction.CallbackContext context) { }

}
