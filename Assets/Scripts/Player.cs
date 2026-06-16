using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] private KeyCode runKey = KeyCode.LeftShift;
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

    // ---------- ÁUDIO ----------
    [Header("Audio")]
    [SerializeField] private AudioSource footstepsSource; // AudioSource em loop para os passos
    [SerializeField] private AudioClip walkClip;          // som de andar
    [SerializeField] private AudioClip runClip;           // som de correr
    [SerializeField] private AudioSource jumpSource;      // AudioSource para o salto (one-shot)
    [SerializeField] private AudioClip jumpClip;          // som de saltar
    // ---------------------------

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
        HandleFootsteps(); // gere o som dos passos a cada frame
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 desiredDirection = (forward * vertical + right * horizontal).normalized;
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

            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpSpeed;
                canDoubleJump = true;
                groundedTimer = 0f;
                TocarSalto(); // toca o som do salto
            }
        }
        else
        {
            Vector3 horizontalMovement = desiredDirection * currentSpeed;
            moveDirection.x = horizontalMovement.x;
            moveDirection.z = horizontalMovement.z;

            if (Input.GetButtonDown("Jump") && canDoubleJump)
            {
                if (Stamina >= doubleJumpCost)
                {
                    moveDirection.y = jumpSpeed;
                    canDoubleJump = false;
                    groundedTimer = 0f;

                    Stamina -= doubleJumpCost;
                    Stamina = Mathf.Clamp(Stamina, 0f, MaxStamina);
                    regenTimer = 0f;
                    UpdateStaminaBar();
                    TocarSalto(); // toca o som também no double jump
                }
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void HandleRun()
    {
        bool wantsToRun = Input.GetKey(runKey);
        bool isMoving = new Vector3(characterController.velocity.x, 0, characterController.velocity.z).magnitude > 0.1f;

        if (wantsToRun && isMoving && Stamina > 0f)
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

        if (Stamina / MaxStamina > 0.5f)
            StaminaBar.color = Color.green;
        else if (Stamina / MaxStamina > 0.25f)
            StaminaBar.color = Color.yellow;
        else
            StaminaBar.color = Color.red;
    }

    void AtualizarAnimacoes()
    {
        Vector3 velocidadeHorizontal = new Vector3(characterController.velocity.x, 0, characterController.velocity.z);
        float velocidadeAtual = velocidadeHorizontal.magnitude;

        animator.SetFloat("speed", velocidadeAtual, animDampTime, Time.deltaTime);
        animator.SetBool("jump", !characterController.isGrounded);
    }

    // ---------- MÉTODOS DE ÁUDIO ----------

    // toca o som de salto (uma vez por salto)
    private void TocarSalto()
    {
        if (jumpSource != null && jumpClip != null)
            jumpSource.PlayOneShot(jumpClip);
    }

    // gere o som dos passos consoante o estado: parado / andar / correr / no ar
    private void HandleFootsteps()
    {
        if (footstepsSource == null) return;

        // está em movimento no chão?
        bool isMoving = new Vector3(characterController.velocity.x, 0, characterController.velocity.z).magnitude > 0.1f;
        bool grounded = characterController.isGrounded;

        if (isMoving && grounded)
        {
            // escolhe o clip certo (correr ou andar)
            AudioClip clipDesejado = isRunning ? runClip : walkClip;

            // se mudou de estado (andar<->correr) ou não está a tocar, troca e arranca
            if (footstepsSource.clip != clipDesejado || !footstepsSource.isPlaying)
            {
                footstepsSource.clip = clipDesejado;
                footstepsSource.loop = true;
                footstepsSource.Play();
            }
        }
        else
        {
            // parado ou no ar → pára os passos
            if (footstepsSource.isPlaying)
                footstepsSource.Stop();
        }
    }

    // --------------------------------------

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Lava"))
        {
            GetComponent<VidaPlayer>().PerderVida();
        }
    }
}