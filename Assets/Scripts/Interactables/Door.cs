using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class Door : Interactable
{
    Rigidbody rb;
    HingeJoint h;
    public Vector3 torqueVector;

    FMOD.Studio.EventInstance evtInstance;
    bool opened = false;

    public override void MousePressed(InputAction.CallbackContext context)
    {
        PlayerController.Instance.DisableMouseMovement();
    }

    public override void MouseHeld(InputAction.CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>();
        float forceAmt = 5;
        OpenDoor(forceAmt * movement.y);
    }

    public override void MouseReleased(InputAction.CallbackContext context)
    {
        PlayerController.Instance.EnableMouseMovement();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // call this function to open the door
    public void OpenDoor(float torqueAmt)
    {
        if (!opened)
        {
            AudioManager.Instance.PlayAudio(AudioManager.Instance.normalDoorOpen, gameObject);
            opened = true;
        }
        rb.AddTorque(transform.localToWorldMatrix * torqueVector.normalized * torqueAmt, ForceMode.Force);
    }
}
