using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int colecionaveisAtual = 0;

    [SerializeField] private TextMeshProUGUI contadorUI; // texto na UI (opcional)

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AdicionarColecionavel()
    {
        colecionaveisAtual++;
        //AtualizarUI();
        Debug.Log("Colecionáveis: " + colecionaveisAtual);
    }

    public void ResetarColecionaveis()
    {
        colecionaveisAtual = 0;
        //AtualizarUI();
    }

    // private void AtualizarUI()
    // {
    //     if (contadorUI != null)
    //         contadorUI.text = "Colecionáveis: " + colecionaveisAtual;
    // }
}