using UnityEngine;
using UnityEngine.UI; // ESSENCIAL para usar o componente Image

public class PocaoVelo : MonoBehaviour
{
    [SerializeField] private float duracao = 10f;
    [SerializeField] private float speedBonus = 3f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip somRecolha;
    [SerializeField] private Image barraTempoImagem; // Continua a ser a imagem que esvazia
    [SerializeField] private GameObject grupoInterfacePocao; // O "Grupo_PocaoVelo" que criámos

    private Player player;
    private float tempoRestante;
    private bool efeitoAtivo = false;

    private Collider meuCollider;
    private Renderer meuRenderer;

    void Start()
    {
        // Pega nos componentes da própria poção para podermos escondê-la na cena
        meuCollider = GetComponent<Collider>();
        meuRenderer = GetComponent<Renderer>();

        // Garante que a barra começa escondida
        if (grupoInterfacePocao != null)
            grupoInterfacePocao.SetActive(false); // Esconde o grupo inteiro ao iniciar
    }

    void OnTriggerEnter(Collider other)
    {
        // Só ativa se for o Player e se o efeito já não estiver a decorrer
        if (other.CompareTag("Player") && !efeitoAtivo)
        {
            player = other.GetComponent<Player>();

            // Aplicar os teus Buffs originais
            Player.speed += speedBonus;
            Player.runSpeed += speedBonus;
            Player.Stamina = 999999999f;
            Player.MaxStamina = 999999999f;

            if (somRecolha != null)
                AudioSource.PlayClipAtPoint(somRecolha, transform.position);

            // Iniciar o Timer
            tempoRestante = duracao;
            efeitoAtivo = true;

            if (grupoInterfacePocao != null)
                grupoInterfacePocao.SetActive(true); // Mostra o grupo com o fundo, máscara e barra

            if (barraTempoImagem != null)
                barraTempoImagem.fillAmount = 1f;

            // Escondemos o modelo 3D e desligamos as colisões (o script continua vivo no mapa)
            if (meuCollider != null) meuCollider.enabled = false;
            if (meuRenderer != null) meuRenderer.enabled = false;

            Debug.Log("Poção de velocidade coletada! Barra de tempo ativa.");
        }
    }

    void Update()
    {
        if (efeitoAtivo)
        {
            // Diminui o tempo restante a cada frame
            tempoRestante -= Time.deltaTime;

            if (barraTempoImagem != null)
            {
                // Calcula a percentagem (vai de 1.0 até 0.0)
                barraTempoImagem.fillAmount = tempoRestante / duracao;
            }

            // Quando o tempo chega a zero, o efeito acaba
            if (tempoRestante <= 0)
            {
                ReverterEfeito();
            }
        }
    }

    private void ReverterEfeito()
    {
        efeitoAtivo = false;
        
        if (grupoInterfacePocao != null)
            grupoInterfacePocao.SetActive(false); // Esconde o grupo todo novamente

        if (player != null)
        {
            Player.speed -= speedBonus;
            Player.runSpeed -= speedBonus;
            Player.Stamina = 100f;
            Player.MaxStamina = 100f;
        }

        Debug.Log("Efeito terminado. Velocidade revertida.");

        // Agora que a barra sumiu e o efeito acabou, o objeto apaga-se da cena com segurança
        Destroy(gameObject);
    }
}