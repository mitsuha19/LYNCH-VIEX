using System;
using System.Collections;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public bool CanMove { get; private set; } = true;

    public bool IsSprinting => canSprint && Input.GetKey(sprintKey) && !isCrouching && isMovingForward && currentStamina > 0;

    private bool ShouldJump => Input.GetKeyDown(jumpKey) && characterController.isGrounded && !isCrouching && !duringCrouchAnimation;

    public bool ShouldCrouch => Input.GetKeyDown(crouchKey) && !duringCrouchAnimation  && characterController.isGrounded;

    [Header("Functional Options")]
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canUseHeadbob = true;
    [SerializeField] private bool useFootsteps = true;
    [SerializeField] private bool useStamina = true;
    [SerializeField] private bool isMovingForward;

    [Header("Controls")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float sprintSpeed = 6.0f;
    [SerializeField] private float crouchSpeed = 1.5f;

    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
    [SerializeField, Range(1, 180)] private float upperLookLimit = 80.0f;
    [SerializeField, Range(1, 180)] private float lowerLookLimit = 80.0f;

    [Header("Jumping Parameters")]
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private float gravity = 9.8f;

    [Header("Crouch Parameters")]
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float standingHeight = 2f;
    [SerializeField] private float timeToCrouch = 1f;
    [SerializeField] private Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] private Vector3 standingCenter = new Vector3(0, 0, 0);
    public bool isCrouching;
    private bool duringCrouchAnimation;

    [Header("Headbob Parameters")]
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float walkBobAmount = 0.3f;
    [SerializeField] private float sprintBobSpeed = 18f;
    [SerializeField] private float sprintBobAmount = 0.6f;
    [SerializeField] private float crouchBobSpeed = 8f;
    [SerializeField] private float crouchBobAmount = 0.15f;
    private float defaultYPos = 0;
    private float defaultXPos = 0;
    private float timer;

    [Header("Footsteps Parameters")]
    [SerializeField] private float baseStepSpeed = 0.5f;
    [SerializeField] private AudioClip[] ceramicClips = default;
    private float footstepTimer = 0;

    [Header("Stamina Parameters")]
    [SerializeField] private float maxStamina = 100;
    [SerializeField] private float staminaUseMultiplier = 5;
    [SerializeField] private float staminaRegenDelay = 5;
    [SerializeField] private float staminaValueIncrement = 2;
    [SerializeField] private float staminaTimeIncrement = 0.1f;
    [SerializeField] private float lowStaminaThreshold = 20;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource footstepAudioSource = default;
    [SerializeField] private AudioSource crouchAndStandAudioSource = default;
    [SerializeField] private AudioSource lowStaminaAudioSource = default;

    [Header("SFX")]
    [SerializeField] private AudioClip[] lowStaminaClip;
    [SerializeField] private AudioClip[] crouchClip;
    [SerializeField] private AudioClip[] standClip;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip landClip;

    private bool previouslyGrounded = true;

    private float currentStamina;
    private Coroutine regeneratingStamina;
    public static Action<float> OnStaminaChange;

    private Camera playerCamera;
    private CharacterController characterController;

    private Vector3 moveDirection;
    private Vector2 currentInput;

    private float rotationX = 0;

    public bool isExitingHidingPlace = false;

    void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponentInChildren<CharacterController>();
        defaultYPos = playerCamera.transform.localPosition.y;
        currentStamina = maxStamina;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        previouslyGrounded = characterController.isGrounded;
    }

    void Update()
    {
        if (CanMove)
        {
            HandleMovementInput();
            HandleMouseLook();

            if (canJump)
                HandleJump();
            if (canCrouch)
                HandleCrouch();
            //if (canUseHeadbob)
            //HandleHeadBob(); 
            if (useFootsteps)
                HandleFootsteps();
            if (useStamina)
                HandleStamina();

            HandleLandingSound();

            ApplyFinalMovements();
        }
    }

    private void HandleMovementInput()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        isMovingForward = verticalInput > 0;

        currentInput = new Vector2((isCrouching ? crouchSpeed : IsSprinting && isMovingForward ? sprintSpeed : walkSpeed) * verticalInput,
                              (isCrouching ? crouchSpeed : IsSprinting && isMovingForward ? sprintSpeed : walkSpeed) * horizontalInput);

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) +
                        (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
    }

    private void HandleMouseLook()
    {
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);
    }

    private void HandleJump()
    {
        if (ShouldJump && !isCrouching && !duringCrouchAnimation)
        {
            moveDirection.y = jumpForce;
            footstepAudioSource.PlayOneShot(jumpClip);
        }
    }

    private float lastGroundedYPosition;
    private float fallThreshold = 1f;

    private void HandleLandingSound()
    {
        if (!previouslyGrounded && characterController.isGrounded)
        {
            float fallDistance = lastGroundedYPosition - characterController.transform.position.y;

            if (fallDistance > fallThreshold)
            {
                footstepAudioSource.PlayOneShot(landClip);
            }
        }

        if (characterController.isGrounded)
        {
            lastGroundedYPosition = characterController.transform.position.y;
        }

        previouslyGrounded = characterController.isGrounded;
    }

    public void HandleCrouch()
    {
        if (ShouldCrouch)
            StartCoroutine(CrouchStand());
    }

    private bool wasGrounded;
    private float landBobAmount = 0.075f;
    private float landBobSpeed = 4f;
    private float landBobTimer;

    private void HandleFootsteps()
    {
        if (!characterController.isGrounded || currentInput == Vector2.zero)
            return;

        float movementSpeed = GetMovementSpeed();
        float footstepInterval = GetFootstepInterval(movementSpeed);

        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0)
        {
            if (Physics.Raycast(playerCamera.transform.position, Vector3.down, out RaycastHit hit, 3))
            {
                footstepAudioSource.PlayOneShot(ceramicClips[UnityEngine.Random.Range(0, ceramicClips.Length)]);
            }

            footstepTimer = footstepInterval;
        }
    }

    private float GetMovementSpeed()
    {
        if (isCrouching) return crouchSpeed;
        if (IsSprinting) return sprintSpeed;
        return walkSpeed;
    }

    private float GetFootstepInterval(float speed)
    {
        return baseStepSpeed / speed;
    }

    private void HandleStamina()
    {
        if (IsSprinting && currentInput != Vector2.zero && !isCrouching)
        {
            if (regeneratingStamina != null)
            {
                StopCoroutine(regeneratingStamina);
                regeneratingStamina = null;
            }

            currentStamina -= staminaUseMultiplier * Time.deltaTime;
            currentStamina = Mathf.Max(currentStamina, 0);

            OnStaminaChange?.Invoke(currentStamina);
            Debug.Log("Stamina: " + currentStamina);

        }
        else
        {
            if (regeneratingStamina == null)
            {
                regeneratingStamina = StartCoroutine(RegenerateStamina());
            }
        }

        if (currentStamina <= lowStaminaThreshold && !lowStaminaAudioSource.isPlaying)
        {
            lowStaminaAudioSource.PlayOneShot(lowStaminaClip[UnityEngine.Random.Range(0, lowStaminaClip.Length)]);
        }
    }

    private void ApplyFinalMovements()
    {
        if (characterController.isGrounded)
        {
            if (moveDirection.y < 0)
            {
                moveDirection.y = -2f;
            }
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }

    private bool CheckForObstaclesAbove()
    {
        float bufferHeight = 0.1f;
        float checkHeight = characterController.height + bufferHeight;
        int numberOfRays = 8;
        float radius = characterController.radius;
        Vector3 origin = characterController.transform.position + Vector3.up * (characterController.height / 2);

        if (Physics.Raycast(origin, Vector3.up, checkHeight))
            return true;

        for (int i = 0; i < numberOfRays; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfRays;
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            Vector3 rayOrigin = origin + offset;

            if (Physics.Raycast(rayOrigin, Vector3.up, checkHeight))
                return true;
        }

        return false;
    }

    public IEnumerator CrouchStand()
    {
        if (isCrouching && CheckForObstaclesAbove())
            yield break;

        duringCrouchAnimation = true;
        canSprint = false;

        // Force standing if exiting a hiding place
        bool forceStand = isExitingHidingPlace && isCrouching;
        if (forceStand)
        {
            isCrouching = false;
        }

        float targetHeight = isCrouching ? standingHeight : crouchHeight;
        float currentHeight = characterController.height;

        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = characterController.center;

        float heightVelocity = 0;
        Vector3 centerVelocity = Vector3.zero;

        #region audio
        if (isCrouching)
        {
            crouchAndStandAudioSource.PlayOneShot(crouchClip[UnityEngine.Random.Range(0, crouchClip.Length)]);
        }
        else
        {
            crouchAndStandAudioSource.PlayOneShot(standClip[UnityEngine.Random.Range(0, standClip.Length)]);
        }
        #endregion

        while (Mathf.Abs(characterController.height - targetHeight) > 0.001f ||
               Vector3.Distance(characterController.center, targetCenter) > 0.001f)
        {
            float newHeight = Mathf.SmoothDamp(currentHeight, targetHeight, ref heightVelocity, timeToCrouch);
            Vector3 newCenter = Vector3.SmoothDamp(currentCenter, targetCenter, ref centerVelocity, timeToCrouch);

            characterController.height = newHeight;
            characterController.center = newCenter;

            float deltaHeight = newHeight - currentHeight;
            characterController.Move(Vector3.up * deltaHeight);

            currentHeight = newHeight;
            currentCenter = newCenter;

            yield return null;
        }

        characterController.height = targetHeight;
        characterController.center = targetCenter;

        if (forceStand)
        {
            isCrouching = false;
            isExitingHidingPlace = false; 
        }
        else
        {
            isCrouching = !isCrouching;
        }

        duringCrouchAnimation = false;
        canSprint = true;
    }

private void HandleHeadBob()
    {
        if (!wasGrounded && characterController.isGrounded)
        {
            float fallDistance = lastGroundedYPosition - characterController.transform.position.y;

            if (fallDistance > fallThreshold)
            {
                landBobTimer = 0;
            }
        }

        if (characterController.isGrounded)
        {
            bool isMoving = Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f;

            if (isMoving)
            {
                timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : IsSprinting ? sprintBobSpeed : walkBobSpeed);
            }

            float bobAmount = isCrouching ? crouchBobAmount : IsSprinting ? sprintBobAmount : walkBobAmount;
            float verticalBob = Mathf.Sin(timer) * bobAmount;
            float horizontalBob = Mathf.Cos(timer) * bobAmount * 0.25f;

            float landBobVertical = 0f;
            if (landBobTimer < 1f)
            {
                landBobTimer += Time.deltaTime * landBobSpeed;
                float landBobSin = Mathf.Sin(landBobTimer * Mathf.PI);
                landBobVertical = landBobSin * landBobAmount;
            }

            playerCamera.transform.localPosition = new Vector3(
                defaultXPos + horizontalBob,
                defaultYPos + verticalBob - landBobVertical,
                playerCamera.transform.localPosition.z);
        }
        else
        {
            if (landBobTimer < 1f)
            {
                landBobTimer += Time.deltaTime * landBobSpeed;
                float landBobSin = Mathf.Sin(landBobTimer * Mathf.PI);
                float landBobVertical = landBobSin * landBobAmount;
                float landBobHorizontal = landBobSin * landBobAmount * 0.25f;

                playerCamera.transform.localPosition = new Vector3(
                    defaultXPos + landBobHorizontal,
                    defaultYPos - landBobVertical,
                    playerCamera.transform.localPosition.z);
            }
        }

        wasGrounded = characterController.isGrounded;
    }

    private IEnumerator RegenerateStamina()
    {
        if (currentStamina <= 0)
            yield return new WaitForSeconds(staminaRegenDelay);

        WaitForSeconds timeToWait = new WaitForSeconds(staminaTimeIncrement);
        while (currentStamina < maxStamina)
        {
            if (currentStamina > 0)
            {
                canSprint = true;
                useStamina = true;
            }

            if ((maxStamina - currentStamina) < staminaValueIncrement)
            {
                currentStamina = maxStamina;
            }
            else
            {
                currentStamina += staminaValueIncrement;
            }

            OnStaminaChange?.Invoke(currentStamina);
            Debug.Log("Stamina : " + currentStamina);
            yield return timeToWait;
        }
        regeneratingStamina = null;
    }

}