using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform forceAddPoint;

    Rigidbody rb;
    HingeJoint h;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // call this function to open the door
    public void OpenDoor(Vector3 forceVector)
    {
        rb.AddForce(forceVector, ForceMode.Force);
    }
}
