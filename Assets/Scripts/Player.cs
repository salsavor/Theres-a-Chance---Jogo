using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    private Vector3 moveDirection = Vector3.zero;
    public CharacterController characterController;
    public Transform cameraTransform;

    [SerializeField] private float mouseSensitivity = 2.0f;
    [SerializeField] private float verticalClampMin = -60f;
    [SerializeField] private float verticalClampMax = 60f;
    private float verticalRotation = 0f;

    [SerializeField] private float runSpeed = 12.0f;
    [SerializeField] private KeyCode runKey = KeyCode.LeftShift;
    [SerializeField] private float doubleJumpCost = 25f; // custo de stamina para double jump
    private bool isRunning = false;

    private bool canDoubleJump = false;

    private float groundedTimer = 0f;
    private float groundedBuffer = 0.15f;

    [SerializeField] private float animDampTime = 0.1f; // suavização do blend de animações

    private Animator animator;

    public float Stamina = 100f;
    public float MaxStamina = 100f; 
    public float RunCost = 10f; // custo de correr 
    public float RegenRate = 10f; // regeneração/p segundo
    public float RegenDelay = 1.5f; // tempo ate começar a regenerar 
    private float regenTimer = 0f;  // timer ate voltar a regenerar, tipo corre > gasta stamina > para de correr > espera o delay > começa a regenerar, mas se gastar de novo reseta o timer
    
    public Image StaminaBar;


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
        HandleCameraRotation();
        HandleRun();
        HandleMovement();
        AtualizarAnimacoes();
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

        // Timer usado APENAS para as animações
        if (characterController.isGrounded)
            groundedTimer = groundedBuffer;
        else
            groundedTimer -= Time.deltaTime;

        if (characterController.isGrounded)
        {
            moveDirection = desiredDirection * currentSpeed;
            moveDirection.y = -2f; // força constante para baixo — cola ao terreno

            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpSpeed;
                canDoubleJump = true;
                groundedTimer = 0f; // dispara a animação de jump imediatamente
            }
        }
        else
        {
            Vector3 horizontalMovement = desiredDirection * currentSpeed;
            moveDirection.x = horizontalMovement.x;
            moveDirection.z = horizontalMovement.z;

            if (Input.GetButtonDown("Jump") && canDoubleJump)
            {

                // ve a stamina 
                    if (Stamina >= doubleJumpCost)
                    {
                        moveDirection.y = jumpSpeed;
                        canDoubleJump = false;
                        groundedTimer = 0f;

                        // usa 25% da stamina para o double jumpp
                        Stamina -= doubleJumpCost;
                        Stamina = Mathf.Clamp(Stamina, 0f, MaxStamina);
                        regenTimer = 0f; // reset do delay de regen
                        UpdateStaminaBar();
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
        regenTimer = 0f; // reset do delay de regen
    }
    else
    {
        isRunning = false;

        //  delay
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

        // muda de  cor com a stamina verde para amarelo para vermelho :D
        if (Stamina / MaxStamina > 0.5f)
            StaminaBar.color = Color.green;
        else if (Stamina / MaxStamina > 0.25f)
            StaminaBar.color = Color.yellow;
        else
            StaminaBar.color = Color.red;
    }


    private void HandleCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, verticalClampMin, verticalClampMax);

        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    void AtualizarAnimacoes()
    {
        Vector3 velocidadeHorizontal = new Vector3(characterController.velocity.x, 0, characterController.velocity.z);
        float velocidadeAtual = velocidadeHorizontal.magnitude;

        // dampTime suaviza a mudança do parâmetro → blend fluido entre Idle/Walk/Run
        animator.SetFloat("speed", velocidadeAtual, animDampTime, Time.deltaTime);

       

        // groundedTimer evita que jump ative em terreno irregular
        animator.SetBool("jump", !characterController.isGrounded);
    }

    
}
