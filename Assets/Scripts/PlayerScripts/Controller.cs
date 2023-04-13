using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class Controller : MonoBehaviour
{
    public bool CanMove { get; private set; } = true; //conditions to be able to move
    private bool IsSprinting => canSprint && Input.GetKey(sprintKey); //conditions to be able to sprint
    private bool ShouldJump => Input.GetKeyDown(jumpKey) && characterController.isGrounded; //conditions to be able to jump
    private bool ShouldCrouch => Input.GetKeyDown(crouchKey) && !duringCrouchAnimation && characterController.isGrounded; //conditions to be able to crouch



    //gives ability to turn them on and off in the inspector 
    [Header("Functional Options")]
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canUseHeadbob = true;
    [SerializeField] private bool willSlide = true;
    [SerializeField] private bool canInteract = true;
    [SerializeField] private bool useFootseps = true;
    [SerializeField] public bool CanLook = true;

    //allows the change of controls in inspector
    [Header("Controls")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode interactKey = KeyCode.Q;


    
    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 3.0f;    //walk speed
    [SerializeField] private float sprintSpeed = 6.0f;  //sprint speed
    [SerializeField] private float crouchSpeed = 1.5f;  //crouch speed
    
    


    //how far the player can look
    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
    [SerializeField, Range(1, 180)] private float upperLookLimit = 80.0f;
    [SerializeField, Range(1, 180)] private float lowerLookLimit = 80.0f;

    //player jump parameters
    [Header("Jumping Parameters")]
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private float gravity = 30.0f;

    //crouch parameters
    [Header("Crouch Parameters")]
    [SerializeField] private float crouchHeight = 0.0f;
    [SerializeField] private float standingHeight = 2f;
    [SerializeField] private float timeToCrouch = 0.25f;
    [SerializeField] private Vector3 crouchingCenter = new Vector3(0, 0.05f, 0);
    [SerializeField] private Vector3 standingCenter = new Vector3(0, 0, 0);
    private bool isCrouching;
    private bool duringCrouchAnimation;

    //headbob parameters
    [Header("Headbob Parameters")]
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float walkBobAmount = 0.05f;
    [SerializeField] private float sprintBobSpeeed = 18f;
    [SerializeField] private float sprintBobAmount = 0.11f;
    [SerializeField] private float crouchBobSpeeed = 8f;
    [SerializeField] private float crouchBobAmount = 0.025f;
    private float defaultYPos = 0f;
    private float timer;

    //footstep parameters
    [Header("Footstep Parameters")]
    [SerializeField] private float baseStepSpeed = 0.5f;
    [SerializeField] private float crouchStepMultiplier = 1.5f;
    [SerializeField] private float sprintStepMultiplier = 0.6f;
    [SerializeField] private AudioSource footstepAudioSource = default;
    [SerializeField] private AudioClip[] groundClips = default;
    private float footstepTimer = 0;
    private float GetCurrentOffset => isCrouching ? baseStepSpeed * crouchStepMultiplier : IsSprinting ? baseStepSpeed * sprintStepMultiplier : baseStepSpeed; //sets speed of footsteps depending on speed of player

    //stamina parameters
    [Header("Stamina Parameters")]
    public float playerStamina = 100f;
    public float maxStamina = 100f;
    public bool hasRegenerated = true;
    public float staminaDrain = 15f;
    public float staminaRegain = 15f;
    public Image staminaProgressUI = null;
    public CanvasGroup sliderCanvasGroup = null;

    [Header("Interaction")]
    [SerializeField] private Vector3 interactionRayPoint = default;
    [SerializeField] private float interactionDistance = default;
    [SerializeField] private LayerMask interactionLayer = default;
    private Interactable currentInteractable;

    private Camera playerCamera;
    private CharacterController characterController;

    private Vector3 moveDirection;
    private Vector2 currentInput;
    private float rotationX = 0;

    public static Controller instance;

    //turns off cursor
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Awake()
    {
        instance = this;
        playerCamera = GetComponentInChildren<Camera>(); //calls camera
        characterController = GetComponent<CharacterController>(); //calls controller
        defaultYPos = playerCamera.transform.localPosition.y; //defaults y posintion to the camera's one
    }
    void Update()
    {
        //checks if boolians are true
        if (CanMove)
        {
            ApplyFinalMovements();
            HandleMovementInput();
           
            if (canJump)
                HandleJump();
            if (canCrouch)
                HandleCrouch();
            if (canUseHeadbob)
                HandleHeadbob();
            if (useFootseps)
                HandleFooststeps();
            if (canInteract)
                HandleInteractionInput();
            if (CanLook)
                HandleMouseLook();
            HandleInteractionCheck();
        }

        if (IsSprinting)
        {
            if (playerStamina > 0)
            {
                Sprinting();

            }
            
        }
    }
    //player movement
    private void HandleMovementInput()
    {
        currentInput = new Vector2((isCrouching ? crouchSpeed : IsSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Vertical"), (isCrouching ? crouchSpeed : IsSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Horizontal"));
        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;

        
        if (!IsSprinting)
        {
            if(playerStamina <= maxStamina - 0.01)
            {
                playerStamina += staminaRegain * Time.deltaTime; //update stamina
                UpdateStamina(1);

                if(playerStamina >= maxStamina)
                {
                    canSprint = true;
                    hasRegenerated = true;
                    sliderCanvasGroup.alpha = 0;
                }
            }
        }
    }

    public void Sprinting()
    {
        //if stamina is full
        if (hasRegenerated)
        {
            IsSprinting.Equals(true); //player is sprinting
            playerStamina -= (staminaDrain * Time.deltaTime); //stamina begins draining
            UpdateStamina(1); //updates stamina bar

            //if player doesnt have stamina
            if (playerStamina <= 0)
            {
                hasRegenerated = false; //stamina has not regenerated
                canSprint = false; //player cannot sprint
                sliderCanvasGroup.alpha = 0; //slider is empty
            }
        }
    }
    void UpdateStamina(int value)
    {
        staminaProgressUI.fillAmount = playerStamina / maxStamina;

        if(value == 0)
        {
            sliderCanvasGroup.alpha = 0;
        }
        else
        {
            sliderCanvasGroup.alpha = 1;
        }
    }

    //camera follows mouse
    private void HandleMouseLook()
    {
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);
    }

    //checks if player is interacting with interactable object
    private void HandleInteractionCheck()
    {
        if (Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance))
        {
            if (hit.collider.gameObject.layer == 6 && (currentInteractable == null || hit.collider.gameObject.GetInstanceID() != currentInteractable.GetInstanceID()))
            {
                hit.collider.TryGetComponent(out currentInteractable);
                if (currentInteractable)
                {
                    currentInteractable.OnFocus(); //looking at interactable object
                }
            }
        }
        else if (currentInteractable)
        {
            currentInteractable.OnLoseFocus();  //not looking at interactable object
            currentInteractable = null;
        }
    }
    //when looking at interactable object and pressing interact button
    private void HandleInteractionInput()
    {
        if (Input.GetKeyDown(interactKey) && currentInteractable != null && Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance, interactionLayer))
        {
            currentInteractable.OnInteract();
        }
    }

    //footsteps
    private void HandleFooststeps()
    {
        if (!characterController.isGrounded) return;
        if (currentInput == Vector2.zero) return;

        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0)
        {
            if (Physics.Raycast(playerCamera.transform.position, Vector3.down, out RaycastHit hit, 3))
            {
                //can add multiple surfaces here
                switch (hit.collider.tag)
                {
                    case "Ground":
                        footstepAudioSource.PlayOneShot(groundClips[Random.Range(0, groundClips.Length - 1)]);
                        break;
                    default:
                        footstepAudioSource.PlayOneShot(groundClips[Random.Range(0, groundClips.Length - 1)]);
                        break;

                }
            }

            footstepTimer = GetCurrentOffset;
        }
    }
    private void ApplyFinalMovements()
    {
        if (!characterController.isGrounded)
            moveDirection.y -= gravity * Time.deltaTime;

        characterController.Move(moveDirection * Time.deltaTime);
    }

    //allows player to jump
    private void HandleJump()
    {
        if (ShouldJump)
            moveDirection.y = jumpForce;
    }

    //allows player to crouch
    private void HandleCrouch()
    {
        if (ShouldCrouch)
            StartCoroutine(CrouchStand());
    }

    //toggles crouch on and off
    private IEnumerator CrouchStand()
    {
        if (isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1f))
            yield break;

        duringCrouchAnimation = true;

        float timeElapsed = 0;
        float targetHeight = isCrouching ? standingHeight : crouchHeight;
        float currentHeight = characterController.height;
        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = characterController.center;

        while (timeElapsed < timeToCrouch)
        {
            characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        characterController.height = targetHeight;
        characterController.center = targetCenter;

        isCrouching = !isCrouching;

        duringCrouchAnimation = false;
    }

    //allows for headbob
    private void HandleHeadbob()
    {
        if (!characterController.isGrounded) return;

        if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
        {
            timer += Time.deltaTime * (isCrouching ? crouchBobSpeeed : IsSprinting ? sprintBobSpeeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3(
            playerCamera.transform.localPosition.x, defaultYPos + Mathf.Sin(timer) * (isCrouching ? crouchBobAmount : IsSprinting ? sprintBobAmount : walkBobAmount), playerCamera.transform.localPosition.z);
        }
    }
    

}
