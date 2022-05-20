using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.UI;

public class PlayerController : Singleton<PlayerController>
{
    // when set to true, the player cannot control the character
    bool disableControls = false;

    // ==============================
    // ========== movement ==========
    // ==============================
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

    // when set to true, the player cannot move the camera with the mouse movement
    // this is used to disable mouse movement when the player is opening doors
    bool disableMouseMovement = false;

    // ==============================
    // =========== sound ============
    // ==============================
    float footstepsAudioCountdownTimer = 0.3f; // used in Update

    // ==============================
    // ======= interactables ========
    // ==============================
    [Header("Crosshair Icon Display")]
    public Image crosshairIcon;
    Sprite defaultCrossHairIcon;
    Interactable currInteractable;
    bool isMouseHeld = false;

    // ==============================
    // ======= user interface =======
    // ==============================
    [Header("User Interface")]
    public GameObject noteUI;

    private void Start()
    {
        playerCamera = Camera.main;
        cc = GetComponent<CharacterController>();

        // lock the player's cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // the default crosshair icon is the one that is initially set
        // as the "sprite" on the crossharIcon gameObject
        defaultCrossHairIcon = crosshairIcon.sprite;
    }

    // =====================================================================
    // ========================== input ====================================
    // The following functions are called from PlayerInputHandler.cs when
    // the player provides a specific input
    // =====================================================================
    public void OnMove(CallbackContext context)
    {
        if (disableControls) return;

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
        if (disableControls) return;

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
        if (currInteractable)
        {
            if (context.started)
            {
                isMouseHeld = true;
                currInteractable.MousePressed(context);
            } 
            else if (context.phase == InputActionPhase.Canceled) {
                isMouseHeld = false;
                currInteractable.MouseReleased(context);
            }
        }
    }

    // Reads mouse input data and rotates the camera accordingly.
    Quaternion oldRotation;
    public void OnMouseMove(CallbackContext context)
    {
        if (context.performed) return;
        if (disableControls) return;

        // execute MouseHeld interaction event for the current interactable
        if (isMouseHeld)
        {
            if (currInteractable)
            {
                currInteractable.MouseHeld(context);
            }
        }

        float camLookMaxAngle = 60; // maximum euler angle x for looking

        float mousex = context.ReadValue<Vector2>().x;
        if (!Mathf.Approximately(mousex, 0))
        {
            if (!disableMouseMovement)
            {
                gameObject.transform.Rotate(0, mousex * lookSpeed, 0);
            }
        }

        float mousey = context.ReadValue<Vector2>().y;
        if (!Mathf.Approximately(mousey, 0))
        {
            int inv = invertMouseYAxis ? 1 : -1;
            oldRotation = playerCamera.transform.rotation;
            if (!disableMouseMovement)
            {
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
    // =====================================================================
    // =====================================================================

    void Update()
    {
        if (disableControls) return;

        { // Show icon of interactable objet when the player looks at it
            // only update the icon if the mouse button is NOT being held
            if (!isMouseHeld)
            {
                Ray r = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                RaycastHit hit;
                if (Physics.Raycast(playerCamera.transform.position, r.direction * 10, out hit))
                {
                    Interactable i = hit.transform.GetComponent<Interactable>();
                    if (i != null)
                    {
                        // show interactable icon
                        crosshairIcon.sprite = i.sprite;
                        crosshairIcon.gameObject.SetActive(true);

                        currInteractable = i;
                    } else
                    { // show default icon
                        crosshairIcon.sprite = defaultCrossHairIcon;
                        crosshairIcon.gameObject.SetActive(false);
                    }
                } else // show default icon
                {
                    crosshairIcon.sprite = defaultCrossHairIcon;
                    currInteractable = null;
                    crosshairIcon.gameObject.SetActive(false);
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

        { // play footsteps audio every so often
            if (footstepsAudioCountdownTimer < 0)
            {
                // edit the following line to change how often footsteps play
                footstepsAudioCountdownTimer = Random.Range(0.3f, 0.4f);

                FMOD.Studio.EventInstance evt = AudioManager.Instance.SetupAudio(AudioManager.Instance.footstep, gameObject);
                evt.setParameterByName("Tile Footstep Variation", Random.Range(0, 7));
                evt.setVolume(0.1f);
                evt.start();
                evt.release();
            }
            else if (movement.x != 0 || movement.z != 0)
            {
                footstepsAudioCountdownTimer -= Time.deltaTime;
            }
        }
    }

    public void DisableMouseMovement()
    {
        disableMouseMovement = true;
    }

    public void EnableMouseMovement()
    {
        disableMouseMovement = false;
    }
    // =====================================================================
    // ======================= user interface ==============================
    // this function is called when the player clicks on a note
    public void OpenNoteUI()
    {
        noteUI.SetActive(true);
        disableControls = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseNoteUI()
    {
        noteUI.SetActive(false);
        disableControls = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    // =====================================================================
    // =====================================================================
}