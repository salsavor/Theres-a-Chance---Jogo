using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float movementSpeed; // velocidade de movimento, editável no inspector
    [SerializeField] private float jumPower; // força de salto, editável no inspector
    [SerializeField] private AudioSource jumpsound; // som de salto, editável no inspector

    public bool grounded = false; // identifica se o player está no chão
    public bool canDoubleJump; // identifica se o player pode fazer duplo salto
    public Rigidbody playerigidbody3D; // referência ao Rigidbody 3D do player
    public static Animator myAnimation; // referência ao Animator, static para ser acedido externamente

    void Start()
    {
        playerigidbody3D = GetComponent<Rigidbody>(); // obtém o Rigidbody do GameObject Player
        myAnimation = GetComponent<Animator>(); // obtém o Animator do GameObject Player
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal"); // obtém o valor do eixo horizontal (-1 a 1)
        float vertical = Input.GetAxis("Vertical"); // obtém o valor do eixo vertical (-1 a 1)
        HandleMovement(horizontal, vertical); // chama a função de movimento
        Flip(horizontal); // chama a função de orientação
    }

    void Update()
    {
        Jump(); // chama a função de salto a cada frame
    }

    private void HandleMovement(float horizontal, float vertical)
    {
        // cria um vetor de movimento com os valores horizontal e vertical multiplicados pela velocidade
        Vector3 movement = new Vector3(horizontal, 0f, vertical) * movementSpeed;
        movement.y = playerigidbody3D.linearVelocity.y; // mantém a velocidade Y atual (gravidade)
        playerigidbody3D.linearVelocity = movement; // aplica o movimento ao Rigidbody
        // atualiza o parâmetro speed no Animator com a soma dos valores absolutos de horizontal e vertical
        myAnimation.SetFloat("speed", Mathf.Abs(horizontal) + Mathf.Abs(vertical));
    }

    private void Flip(float horizontal)
    {
        if (horizontal > 0) // se o player se move para a direita
        {
            Vector3 theScale = transform.localScale; // obtém a escala atual do player
            theScale.x = Mathf.Abs(theScale.x); // garante que o valor X é positivo (direita)
            transform.localScale = theScale; // aplica a nova escala
        }
        else if (horizontal < 0) // se o player se move para a esquerda
        {
            Vector3 theScale = transform.localScale; // obtém a escala atual do player
            theScale.x = -Mathf.Abs(theScale.x); // garante que o valor X é negativo (esquerda)
            transform.localScale = theScale; // aplica a nova escala
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump")) // se o botão de salto foi pressionado
        {
            if (grounded) // se o player está no chão
            {
                playerigidbody3D.AddForce(Vector3.up * jumPower); // aplica força para cima
                canDoubleJump = true; // desbloqueia o duplo salto
                if (jumpsound != null) jumpsound.Play(); // reproduz o som de salto
            }
            else if (canDoubleJump) // se o player está no ar e pode fazer duplo salto
            {
                playerigidbody3D.AddForce(Vector3.up * jumPower); // aplica força para cima
                canDoubleJump = false; // bloqueia o duplo salto
            }
        }

        if (!grounded) // se o player não está no chão
            myAnimation.SetBool("jump", true); // ativa a animação de salto
        else
            myAnimation.SetBool("jump", false); // desativa a animação de salto
    }
}