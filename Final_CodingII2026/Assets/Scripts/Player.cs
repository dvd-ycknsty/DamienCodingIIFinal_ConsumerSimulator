using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpHeight = 5f;

    public Transform cameraTransform;
    public float lookSensetivity = 1f;

    private CharacterController cc;

    [Header("Camera Options")]
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float verticalVelocity;//current upward/downward speed
    private float gravity = -20f;//CONSTANT downward acceleration
    private float pitch;


    [Header("Interaction Variables")]
    private GameObject currentTarget;
    public Image reticleImage;
    private bool interactPressed;
    //this is our event that the other scripts will be listening for
    public static event Action<NPCData> OnDialogueRequested;
    private Interactable currentInteractable;

    private bool isRunning;
    private bool isJumping;

    private bool isPaused = false;


    void Awake()
    {
        cc = GetComponent<CharacterController>();

        //OPTIONAL CURSOR LOCK
        //Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //find reticle
        reticleImage = GameObject.Find("Reticle").GetComponent<Image>();
        reticleImage.color = new Color(0, 0, 0, .7f);
    }

    void Update()
    {
        HandleLook();
        HandleMovement();
        CheckInteract();
        HandleInteract();
    }

    private void HandleLook()
    {
        //rotates player
        float yaw = lookInput.x * lookSensetivity;
        //vertical rotation of cam
        float pitchDelta = lookInput.y * lookSensetivity;

        transform.Rotate(Vector3.up * yaw);

        pitch -= pitchDelta;
        //clamp so we dont go upside down
        pitch = Mathf.Clamp(pitch, -90, 90);

        cameraTransform.localRotation = Quaternion.Euler(pitch, 0, 0);
    }

    private void HandleMovement()
{
    bool grounded = cc.isGrounded;

    // Apply gravity every frame
    verticalVelocity += gravity * Time.deltaTime;

    // Reset velocity when grounded to prevent velocity accumulation
    if (grounded && verticalVelocity < 0)
    {
        verticalVelocity = -2f; // Small downward force to keep grounded
    }

    float currentSpeed = isRunning ? runSpeed : walkSpeed;

    Vector3 move = transform.right * moveInput.x * currentSpeed + transform.forward * moveInput.y * currentSpeed;

    // Jump logic - only jump if grounded
    if (isJumping && grounded)
    {
        verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        isJumping = false; // Reset immediately after jumping
    }

    Vector3 velocity = Vector3.up * verticalVelocity;
    cc.Move((move + velocity) * Time.deltaTime);
}

    void CheckInteract()
    {
        if (reticleImage != null) reticleImage.color = new Color(0, 0, 0, .7f);
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        //RaycastHit hit;
        // bool didHit = Physics.Raycast(ray, out hit, 3);
        //if (!didHit) return;
        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            currentInteractable = hit.collider.GetComponentInParent<Interactable>();
            if (currentInteractable != null && reticleImage != null)
            {
                reticleImage.color = Color.red;
                Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 3, Color.blue);
            }
            else
            {
                Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 3, Color.blue);
            }
        }


    }

    void HandleInteract()
    {
        if (!interactPressed) return;
        interactPressed = false;
        if (currentInteractable == null) return;
        currentInteractable.Interact(this);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed) isJumping = true;
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        isRunning = context.ReadValueAsButton();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed) interactPressed = true;
        //Debug.Log("OnInteract fired. performed=" + context.performed);
    }

    public void OnApplicationPause(bool isPaused)
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = true;
        }
        else
        {
            isPaused = false;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.Log("CC Collided with: " + hit.gameObject.name);
    }

    public void RequestDialogue(NPCData npcData)
    {
        OnDialogueRequested?.Invoke(npcData);
    }
}