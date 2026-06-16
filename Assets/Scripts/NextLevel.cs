using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private string nomeScene;
    [SerializeField] private int colecionaveisNecessarios;
    [SerializeField] private int videoIndex;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.instance.colecionaveisAtual >= colecionaveisNecessarios)
            {
                GameManager.instance.ResetarColecionaveis();
                LoadingData.sceneDestino = nomeScene;
                LoadingData.videoIndex = videoIndex;
                SceneManager.LoadScene("LoadingScreen");
            }
            else
            {
                Debug.Log("Precisas de " + colecionaveisNecessarios + " colecionáveis!");
            }
        }
    }
}