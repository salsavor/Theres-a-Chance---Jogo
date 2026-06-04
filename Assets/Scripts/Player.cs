using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    private Vector3 moveDirection = Vector3.zero;
    public CharacterController characterController;
    public Transform cameraTransform; // referência à câmara principal

    [SerializeField] private float mouseSensitivity = 2.0f; // sensibilidade do rato, editável no inspector
    [SerializeField] private float verticalClampMin = -60f; // limite mínimo de rotação vertical da câmara
    [SerializeField] private float verticalClampMax = 60f;  // limite máximo de rotação vertical da câmara
    private float verticalRotation = 0f; // valor acumulado da rotação vertical da câmara

    private Animator animator;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // se não for arrastada no inspector, encontra automaticamente
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked; // esconde e bloqueia o cursor no centro do ecrã
        Cursor.visible = false; // torna o cursor invisível
    }

    void Update()
    {
        HandleCameraRotation(); // chama a função de rotação da câmara

        if (characterController.isGrounded)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // direção base no espaço da câmara
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            // ignorar rotação vertical da câmara
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            // direção final com base no input e orientação da câmara
            Vector3 desiredDirection = (forward * vertical + right * horizontal).normalized;
            moveDirection = desiredDirection * speed;

            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;
        }

        // aplicar gravidade
        moveDirection.y -= gravity * Time.deltaTime;

        // mover personagem
        characterController.Move(moveDirection * Time.deltaTime);

        AtualizarAnimacoes();
    }

    private void HandleCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity; // movimento horizontal do rato
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity; // movimento vertical do rato

        // roda o player no eixo Y com o movimento horizontal do rato
        transform.Rotate(Vector3.up * mouseX);

        // acumula a rotação vertical e limita entre o mínimo e máximo definidos
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, verticalClampMin, verticalClampMax);

        // aplica a rotação vertical apenas à câmara
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    void AtualizarAnimacoes()
    {
        // cria um vetor horizontal (apenas X e Z) para saber a velocidade real de movimento
        Vector3 velocidadeHorizontal = new Vector3(characterController.velocity.x, 0, characterController.velocity.z);

        // magnitude dá o comprimento do vetor (0, 3.5, 6.0, etc.)
        float velocidadeAtual = velocidadeHorizontal.magnitude;

        // passa o valor para o parâmetro speed do Animator
        animator.SetFloat("speed", velocidadeAtual);
    }
}