using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VidaPlayer : MonoBehaviour
{

    [SerializeField] private GameObject life1, life2, life3;

    [SerializeField] public static Transform checkpointDesafio = null;   // volta aqui ao perder uma vida
    [SerializeField] private Transform respawnInicial;      // volta aqui quando fica sem vidas

    public static int vidasAtuais;                         // vidas atuais
    private CharacterController characterController;

    [SerializeField] private KeyCode respawnKey = KeyCode.R; // tecla para respawn manual (para testes)

    void Start()
    {
        vidasAtuais = 3;
        life1.gameObject.SetActive(true);
        life2.gameObject.SetActive(true);
        life3.gameObject.SetActive(true);
        characterController = GetComponent<CharacterController>();

    }

    private void saude()
    {
        switch (vidasAtuais)
        {
            case 3:
                life1.gameObject.SetActive(true);
                life2.gameObject.SetActive(true);
                life3.gameObject.SetActive(true);
                Debug.Log("Vida: " + vidasAtuais);
                break;
            case 2:
                life1.gameObject.SetActive(true);
                life2.gameObject.SetActive(true);
                life3.gameObject.SetActive(false);
                Debug.Log("Vida: " + vidasAtuais);
                break;
            case 1:
                life1.gameObject.SetActive(true);
                life2.gameObject.SetActive(false);
                life3.gameObject.SetActive(false);
                Debug.Log("Vida: " + vidasAtuais);
                break;
            case 0:
                life1.gameObject.SetActive(true);
                life2.gameObject.SetActive(false);
                life3.gameObject.SetActive(false);
                break;
        }
    }


    void Update()
    {
        saude();
        // tecla de respawn manual (para testes)
        if (Input.GetKeyDown(respawnKey))
        {
            Debug.Log("Respawn manual ativado.");
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
            if (checkpointDesafio == null)
            {
                Debug.LogWarning("Checkpoint do desafio nao definido. Teletransportando para o respawn inicial.");
                Teletransportar(respawnInicial);
            }
            else
            {
                Debug.Log("Teletransportando para o checkpoint do desafio.");
                Teletransportar(checkpointDesafio);
            }
            Player.Stamina = Player.MaxStamina; // restaura a stamina
        }
        else
        {
            // sem vidas ent reinicia as vidas e volta ao inicio do nivel
            vidasAtuais = 3;
            life1.gameObject.SetActive(true);
            life2.gameObject.SetActive(true);
            life3.gameObject.SetActive(true);
            Player.Stamina = Player.MaxStamina; // restaura stamina
            Teletransportar(respawnInicial);
            Debug.Log("Sem vidas — de volta ao inicio do nivel.");
            checkpointDesafio = null; // reseta o checkpoint para null
            ReativarPocoes();
            DeathCounter.deathCount++;
        }
    }

    private void ReativarPocoes()
    {
        PocaoVelo[] pocoesVelo = FindObjectsByType<PocaoVelo>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (PocaoVelo pocao in pocoesVelo)
            pocao.gameObject.SetActive(true);

        PocaoVida[] pocoesVida = FindObjectsByType<PocaoVida>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (PocaoVida pocao in pocoesVida)
            pocao.gameObject.SetActive(true);
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