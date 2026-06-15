using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporte : MonoBehaviour
{
    [SerializeField] private string nomeScene;
    [SerializeField] private int colecionaveisNecessarios;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.instance.colecionaveisAtual >= colecionaveisNecessarios)
            {
                GameManager.instance.ResetarColecionaveis();
                SceneManager.LoadScene(nomeScene);
            }
            else
            {
                Debug.Log("Precisas de " + colecionaveisNecessarios + " colecionáveis!");
            }
        }
    }
}