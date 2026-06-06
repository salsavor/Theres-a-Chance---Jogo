using UnityEngine;

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
    private bool isRunning = false;

    private bool canDoubleJump = false;

    private float groundedTimer = 0f;
    private float groundedBuffer = 0.15f;

    [SerializeField] private float animDampTime = 0.1f; // suavização do blend de animações

    private Animator animator;

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
                moveDirection.y = jumpSpeed;
                canDoubleJump = false;
                groundedTimer = 0f; // garante que a animação continua em jump
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void HandleRun()
    {
        if (Input.GetKey(runKey))
            isRunning = true;
        else
            isRunning = false;
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

        animator.SetBool("isRunning", isRunning);

        // groundedTimer evita que jump ative em terreno irregular
        animator.SetBool("jump", groundedTimer <= 0f);
    }
}