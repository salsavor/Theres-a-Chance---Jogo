using UnityEngine;
using UnityEngine.AI;

public class ControladorInimigo : MonoBehaviour
{
    [SerializeField] private Transform player;            // referência ao player
    [SerializeField] private float velocidadePerseguir = 8f; // velocidade na perseguição

    [SerializeField] private float alcanceContacto = 3.5f;   // distância considerada "toque"
    [SerializeField] private float intervaloContacto = 0.5f; // evita disparar várias vezes no mesmo toque
    private float tempoUltimoContacto = 0f;                  // cooldown do contacto

    [SerializeField] private float distanciaAmostragem = 50f; // alcance para projetar o player no NavMesh

    [SerializeField] private float velocidadeRotacao = 8f; // suavidade da rotação
    [SerializeField] private float offsetRotacaoY = 0f;    // ajuste do "forward" do modelo

    [SerializeField] private bool capturarAlturaInicial = true; // usar a altura da cena automaticamente
    [SerializeField] private float alturaFlutuacao = 2f;   // usado só se não capturar a inicial
    [SerializeField] private LayerMask camadaChao;         // layer do terreno real
    private float alturaAlvo;                              // altura final acima do terreno

    private Animator animator;        // animação da patrulha (keyframes)
    private NavMeshAgent agent;       // agente de navegação para a perseguição
    private bool aPerseguir = false;  // estado atual: a perseguir ou em patrulha

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // configura o agente
        agent.speed = velocidadePerseguir;
        agent.updateRotation = false; // rotação controlada por nós
        agent.autoBraking = false;    // não trava ao aproximar-se
        agent.stoppingDistance = 0f;  // tenta sempre chegar ao player
        agent.baseOffset = 0f;        // altura tratada por código
        agent.enabled = false;        // começa desligado (estamos em patrulha)

        // define a altura de flutuação
        alturaAlvo = alturaFlutuacao;

        if (capturarAlturaInicial)
        {
            Vector3 origem = transform.position + Vector3.up * 50f;
            RaycastHit hitInicial;
            if (Physics.Raycast(origem, Vector3.down, out hitInicial, 100f, camadaChao))
            {
                alturaAlvo = transform.position.y - hitInicial.point.y;
            }
        }
    }

    void Update()
    {
        if (aPerseguir && agent.enabled && agent.isOnNavMesh)
        {
            // projeta a posição do player sobre o NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(player.position, out hit, distanciaAmostragem, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }

            RodarComMovimento();

            // contacto: ao tocar no player, tira-lhe uma vida
            float distancia = Vector3.Distance(transform.position, player.position);
            if (distancia <=    alcanceContacto && Time.time >= tempoUltimoContacto + intervaloContacto)
            {
                VidaPlayer vida = player.GetComponent<VidaPlayer>();
                if (vida != null)
                    vida.PerderVida();

                tempoUltimoContacto = Time.time;
            }
        }
    }

    void LateUpdate()
    {
        if (!aPerseguir) return;

        Vector3 origem = transform.position + Vector3.up * 50f;
        RaycastHit hitChao;
        if (Physics.Raycast(origem, Vector3.down, out hitChao, 100f, camadaChao))
        {
            Vector3 pos = transform.position;
            pos.y = hitChao.point.y + alturaAlvo;
            transform.position = pos;
        }
    }

    private void RodarComMovimento()
    {
        Vector3 direcao = agent.velocity;
        direcao.y = 0f;

        if (direcao.sqrMagnitude > 0.1f)
        {
            Quaternion rotacaoAlvo = Quaternion.LookRotation(direcao) * Quaternion.Euler(0f, offsetRotacaoY, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacaoAlvo, Time.deltaTime * velocidadeRotacao);
        }
    }

    public void IniciarPerseguicao()
    {
        aPerseguir = true;
        animator.enabled = false;
        agent.enabled = true;
    }

    public void PararPerseguicao()
    {
        aPerseguir = false;
        agent.enabled = false;
        animator.enabled = true;
    }
}