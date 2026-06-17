using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class InimigoHumanoide : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float raioDetecao = 12f;
    [SerializeField] private float alcanceAtaque = 2.5f;   // distância para começar a atacar
    [SerializeField] private float alcanceDano = 2.5f;     // distância a que o golpe acerta
    [SerializeField] private float velocidadeCorrer = 10f;
    [SerializeField] private float intervaloAtaque = 1.5f;
    [SerializeField] private float tempoAteDano = 1.5f;    // espera antes do dano (deixa a animação tocar)
    [SerializeField] private float toleranciaOrigem = 0.5f;

    [SerializeField] private Animator animator;
    private NavMeshAgent agent;

    private Vector3 posicaoOrigem;
    private Quaternion rotacaoOrigem;

    private float tempoUltimoAtaque = -999f;
    private bool estaAtacando = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        agent.speed = velocidadeCorrer;

        posicaoOrigem = transform.position;
        rotacaoOrigem = transform.rotation;
    }

    void Update()
    {
        if (player == null || !agent.isOnNavMesh) return;

        // durante o ataque, só se vira para o player (não interrompe a coroutine)
        if (estaAtacando)
        {
            VirarParaPlayer();
            return;
        }

        float distanciaPlayer = Vector3.Distance(transform.position, player.position);

        if (distanciaPlayer <= alcanceAtaque)
        {
            agent.isStopped = true;
            agent.ResetPath();
            VirarParaPlayer();

            if (Time.time >= tempoUltimoAtaque + intervaloAtaque)
                StartCoroutine(RotinaAtaque());
        }
        else if (distanciaPlayer <= raioDetecao)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            Regressar();
        }

        animator.SetFloat("speed", agent.velocity.magnitude);
    }

    private IEnumerator RotinaAtaque()
    {
        estaAtacando = true;
        tempoUltimoAtaque = Time.time;

        agent.isStopped = true;
        agent.ResetPath();

        if (animator != null)
        {
            animator.SetFloat("speed", 0f);
            animator.ResetTrigger("attack");
            animator.SetTrigger("attack");
        }

        // espera a animação desenrolar ANTES de aplicar o dano
        yield return new WaitForSeconds(tempoAteDano);

        // só acerta se o player ainda estiver perto no fim da animação
        if (player != null)
        {
            float distancia = Vector3.Distance(transform.position, player.position);
            if (distancia <= alcanceDano)
            {
                VidaPlayer vida = player.GetComponent<VidaPlayer>();
                if (vida != null)
                    vida.PerderVida();
            }
        }

        estaAtacando = false;
    }

    private void VirarParaPlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(dir), Time.deltaTime * 10f);
    }

    private void Regressar()
    {
        agent.isStopped = false;

        if (Vector3.Distance(transform.position, posicaoOrigem) > toleranciaOrigem)
        {
            agent.SetDestination(posicaoOrigem);
        }
        else
        {
            agent.ResetPath();
            transform.rotation = Quaternion.Slerp(transform.rotation,
                rotacaoOrigem, Time.deltaTime * 5f);
        }
    }

    public void ResetarInimigo()
    {
        StopAllCoroutines();
        estaAtacando = false;
        tempoUltimoAtaque = -999f;

        if (animator != null)
        {
            animator.ResetTrigger("attack");
            animator.SetFloat("speed", 0f);
        }

        if (agent != null && agent.isOnNavMesh)
        {
            agent.ResetPath();
            agent.isStopped = false;
        }
    }
}