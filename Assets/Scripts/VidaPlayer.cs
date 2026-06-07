using UnityEngine;

public class VidaPlayer : MonoBehaviour
{
    [SerializeField] private int vidasMaximas = 3;          // total de vidas
    [SerializeField] private Transform checkpointDesafio;   // volta aqui ao perder uma vida
    [SerializeField] private Transform respawnInicial;      // volta aqui quando fica sem vidas

    private int vidasAtuais;                                // vidas atuais
    private CharacterController characterController;

    void Start()
    {
        vidasAtuais = vidasMaximas;
        characterController = GetComponent<CharacterController>();
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