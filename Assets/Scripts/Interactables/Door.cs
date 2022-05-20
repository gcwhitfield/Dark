using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class Door : Interactable
{
    HingeJoint hingeJoint;
    Rigidbody rb;
    HingeJoint h;
    public Vector3 torqueVector;

    public AnimationCurve doorOpenAmountMappingCurve;
    public AnimationCurve doorVelocityMappingCurve;

    FMOD.Studio.EventInstance evtInstance;
    bool isPlayingAudio = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        hingeJoint = GetComponent<HingeJoint>();
    }

    public override void MousePressed(InputAction.CallbackContext context)
    {
        PlayerController.Instance.DisableMouseMovement();
    }

    public override void MouseHeld(InputAction.CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>();
        float forceAmt = 1;
        OpenDoor(forceAmt * movement.y);
    }

    public override void MouseReleased(InputAction.CallbackContext context)
    {
        PlayerController.Instance.EnableMouseMovement();
    }

    private void Update()
    {
        // set the audio parameters for the door. the door should
        // not play any sounds when it is moving very slowly. when
        // the player grabs the door, the door should start to creak.
        // if the player slams the door, then it will play the slam sound
        FMOD.Studio.PLAYBACK_STATE audioState;
        evtInstance.getPlaybackState(out audioState);
        if (audioState != FMOD.Studio.PLAYBACK_STATE.STOPPED)
        {
            float doorOpenAmt = (hingeJoint.angle - hingeJoint.limits.min) / hingeJoint.limits.max;
            float doorVelocity = Mathf.Clamp(Mathf.Abs(hingeJoint.velocity) / 360.0f, 0, 1);
            doorOpenAmt = doorOpenAmountMappingCurve.Evaluate(doorOpenAmt);
            doorVelocity = doorVelocityMappingCurve.Evaluate(doorVelocity);
            print("Door Open Amt: " + doorOpenAmt.ToString());
            print("Door Velocity: " + doorVelocity.ToString());
            evtInstance.setParameterByName("Door Open Amount", doorOpenAmt);
            evtInstance.setParameterByName("Door Velocity", doorVelocity);
        }
    }
    // call this function to open the door
    public void OpenDoor(float torqueAmt)
    {
        FMOD.Studio.PLAYBACK_STATE audioState;
        evtInstance.getPlaybackState(out audioState);
        if (audioState == FMOD.Studio.PLAYBACK_STATE.STOPPED)
        {
            evtInstance = AudioManager.Instance.PlayAudio(AudioManager.Instance.normalDoorOpen, gameObject);
        }
        rb.AddTorque(transform.localToWorldMatrix * torqueVector.normalized * torqueAmt, ForceMode.Force);
    }
}
