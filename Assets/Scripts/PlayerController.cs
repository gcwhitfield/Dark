using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : Singleton<PlayerController>
{
    [Header("Input")]
    [Range(0.5f, 10)] public float moveSpeed = 1;
    [Range(1, 3)] public float sprintMovementIncreaseFactor = 1;
    [Range(0.1f, 1)] public float lookSpeed = 1;
    public bool invertMouseYAxis = false;

    private Camera playerCamera;
    private Vector3 velocity; // this vector gets set from OnPlayerMove and will move the
    // player in Update
    private Vector3 movement;
    private CharacterController cc;
    private float sprintFactor = 1;

    private void Start()
    {
        playerCamera = Camera.main;
        cc = GetComponent<CharacterController>();

        // lock the player's cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    // =====================================================================
    // =====================================================================
    // The following functions are called from PlayerInputHandler.cs when
    // the player provides a specific input
    // =====================================================================
    public void OnMove(CallbackContext context)
    {
        if (context.performed || context.started)
        {
            velocity.x = context.ReadValue<Vector2>().x * moveSpeed * sprintFactor;
            velocity.z = context.ReadValue<Vector2>().y * moveSpeed * sprintFactor;
        } else if (context.phase == InputActionPhase.Canceled || context.phase == InputActionPhase.Disabled)
        {
            velocity.x = 0;
            velocity.z = 0;
        } else
        {
            Debug.LogError("Impossible case in OnMove of PlayerController.cs");
        }
    }

    // This function is called from PlayerInputHandler.cs
    public void OnSprint(CallbackContext context)
    {
        if (context.started || context.performed)
        {
            sprintFactor = sprintMovementIncreaseFactor;
        } else if (context.phase == InputActionPhase.Canceled || context.phase == InputActionPhase.Disabled)
        {
            sprintFactor = 1;
        } else
        {
            Debug.LogError("Context in Sprint function of PlayerController was not started or performed!?");
        }
    }

    public void OnInteract(CallbackContext context)
    {

    }
    // =====================================================================
    // =====================================================================

    void Update()
    {
        { // Player can interact with objects when they look at interactables
            Ray r = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, r.direction * 10, out hit))
            {
                //Debug.Log("Object hit: " + hit.transform.name);
                Door d = hit.transform.GetComponent<Door>();
                if (d != null)
                {
                    Debug.Log("Door is not null");
                    d.OpenDoor((hit.point - transform.position).normalized * Vector3.Dot(r.direction, hit.transform.forward) * 100);
                }
            }
        }

        { // Move the player based on input
            // "movement" vector is set inside of OnPlayerMove
            movement += velocity * Time.deltaTime;
            cc.Move(movement.x*transform.right + movement.y*Vector3.up + movement.z*transform.forward);
            // apply gravity
            if (!cc.isGrounded)
            {
                velocity += Physics.gravity * Time.deltaTime;
            } else
            {
                velocity.y = 0;
            }
            // smoothly stop the player's horizontal movement
            movement.x /= 1.5f;
            movement.z /= 1.5f;
            float epsilon = 0.01f;
            if (Mathf.Abs(movement.x) < epsilon)
            {
                movement.x = 0;
            }
            if (Mathf.Abs(movement.z) < epsilon)
            {
                movement.z = 0;
            }
        }
    }

    // Reads mouse input data and rotates the camera accordingly.
    Quaternion oldRotation;
    public void OnMouseMove(CallbackContext context)
    {
        float camLookMaxAngle = 60; // maximum euler angle x for looking

        float mousex = context.ReadValue<Vector2>().x;
        if (!Mathf.Approximately(mousex, 0))
        {
            gameObject.transform.Rotate(0, mousex * lookSpeed, 0);
        }

        float mousey = context.ReadValue<Vector2>().y;
        if (!Mathf.Approximately(mousey, 0))
        {
            int inv = invertMouseYAxis ? 1 : -1;
            oldRotation = playerCamera.transform.rotation;
            playerCamera.transform.Rotate(new Vector3(mousey * lookSpeed * inv, 0, 0), Space.Self); ;
            // clamp rotation if camera is rotated out of
            Vector3 eulers = playerCamera.transform.eulerAngles;
            float upperBound = 360 - camLookMaxAngle;
            float lowerBound = camLookMaxAngle;

            // if out of bounds
            if (eulers.x < upperBound && eulers.x > lowerBound)
            {
                playerCamera.transform.rotation = oldRotation;
            }           
        }
    }
}
