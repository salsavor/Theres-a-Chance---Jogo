using UnityEngine;

public class VidaPlayer : MonoBehaviour
{
    [SerializeField] private int vidasMaximas = 3;          // total de vidas
    [SerializeField] private Transform checkpointDesafio;   // volta aqui ao perder uma vida
    [SerializeField] private Transform respawnInicial;      // volta aqui quando fica sem vidas

    private int vidasAtuais;                                // vidas atuais
    private CharacterController characterController;

    [SerializeField] private KeyCode respawnKey = KeyCode.R; // tecla para respawn manual (para testes)
    [SerializeField] private Transform colliderIlhaFora;

    void Start()
    {
        vidasAtuais = vidasMaximas;
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // tecla de respawn manual (para testes)
        if (Input.GetKeyDown(respawnKey))
        {
            Debug.Log("Respawn manual ativado.");
            PerderVida();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == colliderIlhaFora)
        {
            Debug.Log("Colidiu com ilha fora dos limites.");
            PerderVida();
        }
    }

    // chamado pelo inimigo quando toca no player
    public void PerderVida()
    {
        vidasAtuais--;
        Debug.Log("Vidas restantes: " + vidasAtuais);

        if (vidasAtuais > 0)
        {
            // ainda tem vidas → volta ao checkpoint do desafio
            Teletransportar(checkpointDesafio);
        }
        else
        {
            // sem vidas → reinicia as vidas e volta ao inicio do nivel
            vidasAtuais = vidasMaximas;
            Teletransportar(respawnInicial);
            Debug.Log("Sem vidas — de volta ao inicio do nivel.");
        }
    }

    private void Teletransportar(Transform destino)
    {
        if (destino == null)
        {
            Debug.LogWarning("Destino de teletransporte nao atribuido no VidaPlayer.");
            return;
        }

        // desliga o CharacterController para reposicionar sem conflitos
        characterController.enabled = false;
        transform.position = destino.position;
        characterController.enabled = true;
    }
}