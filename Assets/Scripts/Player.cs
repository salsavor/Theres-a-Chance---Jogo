using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    private Vector3 moveDirection = Vector3.zero;
    public CharacterController characterController;
    public Transform cameraTransform; // Referência à câmara principal
    public bool grounded = false;
    public static Animator myAnimation;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        myAnimation = GetComponent<Animator>();

        // Se não for arrastada no Inspector, tentamos encontrá-la automaticamente
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }
    void Update()
    {
        if (characterController.isGrounded)
        {

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // Direcção base no espaço da câmara
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            // Ignorar rotação vertical da câmara
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            // Direcção final com base no input e orientação da câmara
            Vector3 desiredDirection = (forward * vertical + right * horizontal).normalized;
            moveDirection = desiredDirection * speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;

            }

        }

        // Aplicar gravidade
        moveDirection.y -= gravity * Time.deltaTime;

        // Mover personagem
        characterController.Move(moveDirection * Time.deltaTime);

        AtualizarAnimacoes();


        void AtualizarAnimacoes()
        {
            // Criamos um vetor horizontal (apenas X e Z) para saber a velocidade real de movimento
            Vector3 velocidadeHorizontal = new Vector3(characterController.velocity.x, 0, characterController.velocity.z);

            // .magnitude dá-nos o "comprimento" do vetor (um número como 0, 3.5, 6.0, etc.)
            float velocidadeAtual = velocidadeHorizontal.magnitude;

            // Passa esse valor para o parâmetro "speed" do teu Animator
            myAnimation.SetFloat("speed", velocidadeAtual);
        }

    }

}
