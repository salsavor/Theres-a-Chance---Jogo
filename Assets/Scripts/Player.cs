using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class Player : MonoBehaviour
{
    public static float speed = 6.0f;
    public static float jumpSpeed = 8.0f;
    public static float gravity = 20.0f;
    private Vector3 moveDirection = Vector3.zero;
    public CharacterController characterController;
    public Transform cameraTransform;

    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] public static float runSpeed = 12.0f;
    [SerializeField] private float doubleJumpCost = 25f;
    private bool isRunning = false;
    private bool canDoubleJump = false;
    private float groundedTimer = 0f;
    private float groundedBuffer = 0.15f;
    [SerializeField] private float animDampTime = 0.1f;
    private Animator animator;

    public static float Stamina = 100f;
    public static float MaxStamina = 100f;
    public float RunCost = 10f;
    public float RegenRate = 10f;
    public float RegenDelay = 1.5f;
    private float regenTimer = 0f;
    public Image StaminaBar;

    // Input
    private PlayerControls controls;
    private Vector2 moveInput;
    private bool jumpPressed;
    private bool runHeld;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip walkClip;
    [SerializeField] private AudioClip runClip;
    [SerializeField] private AudioClip jumpClip;

    void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Jump.performed += ctx => jumpPressed = true;

        controls.Player.Sprint.performed += ctx => runHeld = true;
        controls.Player.Sprint.canceled += ctx => runHeld = false;
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleRun();
        HandleMovement();
        AtualizarAnimacoes();
        HandleFootsteps();
        jumpPressed = false; // reset no fim do frame
    }

    private void HandleMovement()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 desiredDirection = (forward * moveInput.y + right * moveInput.x).normalized;
        float currentSpeed = isRunning ? runSpeed : speed;

        if (desiredDirection.sqrMagnitude > 0.01f)
        {
            Quaternion rotacaoAlvo = Quaternion.LookRotation(desiredDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacaoAlvo, Time.deltaTime * rotationSpeed);
        }

        if (characterController.isGrounded)
            groundedTimer = groundedBuffer;
        else
            groundedTimer -= Time.deltaTime;

        if (characterController.isGrounded)
        {
            moveDirection = desiredDirection * currentSpeed;
            moveDirection.y = -2f;

            if (jumpPressed)
            {
                moveDirection.y = jumpSpeed;
                canDoubleJump = true;
                groundedTimer = 0f;
                TocarSalto();
            }
        }
        else
        {
            Vector3 horizontalMovement = desiredDirection * currentSpeed;
            moveDirection.x = horizontalMovement.x;
            moveDirection.z = horizontalMovement.z;

            if (jumpPressed && canDoubleJump && Stamina >= doubleJumpCost)
            {
                moveDirection.y = jumpSpeed;
                canDoubleJump = false;
                groundedTimer = 0f;
                Stamina -= doubleJumpCost;
                Stamina = Mathf.Clamp(Stamina, 0f, MaxStamina);
                regenTimer = 0f;
                UpdateStaminaBar();
                TocarSalto();
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void HandleRun()
    {
        bool isMoving = new Vector3(characterController.velocity.x, 0, characterController.velocity.z).magnitude > 0.1f;

        if (runHeld && isMoving && Stamina > 0f)
        {
            isRunning = true;
            Stamina -= RunCost * Time.deltaTime;
            Stamina = Mathf.Clamp(Stamina, 0f, MaxStamina);
            regenTimer = 0f;
        }
        else
        {
            isRunning = false;
            regenTimer += Time.deltaTime;
            if (regenTimer >= RegenDelay && Stamina < MaxStamina)
            {
                Stamina += RegenRate * Time.deltaTime;
                Stamina = Mathf.Clamp(Stamina, 0f, MaxStamina);
            }
        }

        UpdateStaminaBar();
    }

    private void UpdateStaminaBar()
    {
        if (StaminaBar == null) return;
        StaminaBar.fillAmount = Stamina / MaxStamina;
        if (Stamina / MaxStamina > 0.5f) StaminaBar.color = Color.green;
        else if (Stamina / MaxStamina > 0.25f) StaminaBar.color = Color.yellow;
        else StaminaBar.color = Color.red;
    }

    void AtualizarAnimacoes()
    {
        Vector3 velocidadeHorizontal = new Vector3(characterController.velocity.x, 0, characterController.velocity.z);
        animator.SetFloat("speed", velocidadeHorizontal.magnitude, animDampTime, Time.deltaTime);
        animator.SetBool("jump", !characterController.isGrounded);
    }

    private void TocarSalto()
    {
        if (jumpClip != null && sfxSource != null)
            sfxSource.PlayOneShot(jumpClip);
    }

    private void HandleFootsteps()
    {
        if (audioSource == null) return;
        bool isMoving = new Vector3(characterController.velocity.x, 0, characterController.velocity.z).magnitude > 0.1f;

        if (isMoving && characterController.isGrounded)
        {
            AudioClip clipDesejado = isRunning ? runClip : walkClip;
            if (audioSource.clip != clipDesejado || !audioSource.isPlaying)
            {
                audioSource.clip = clipDesejado;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying) audioSource.Stop();
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Lava"))
            GetComponent<VidaPlayer>().PerderVida();
    }
}