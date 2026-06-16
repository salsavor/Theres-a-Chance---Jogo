using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public int colecionaveisAtual = 0;

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
        Debug.Log("Colecionáveis: " + colecionaveisAtual);
    }

    public void ResetarColecionaveis()
    {
        colecionaveisAtual = 0;
    }

}