using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public Transform forceAddPoint;
    Rigidbody rb;
    HingeJoint h;

    FMOD.Studio.EventInstance evtInstance;
    bool opened = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // call this function to open the door
    public void OpenDoor(Vector3 forceVector)
    {
        if (!opened)
        {
            AudioManager.Instance.PlayAudio(AudioManager.Instance.normalDoorOpen, gameObject);
            opened = true;
        }
        rb.AddForce(forceVector, ForceMode.Force);
    }
}
