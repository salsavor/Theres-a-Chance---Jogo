using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private string nomeScene;
    [SerializeField] private int colecionaveisNecessarios;
    [SerializeField] private int videoIndex;

    [Header("Texto")]
    [SerializeField] private TextMeshProUGUI texto;
    [SerializeField] private float tempoFadeIn = 1f;
    [SerializeField] private float tempoVisivel = 2f;
    [SerializeField] private float tempoFadeOut = 1f;

    void Start()
    {
        texto.gameObject.SetActive(false); // esconde o texto inicialmente
    }

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
                texto.gameObject.SetActive(true); // mostra o texto
                StartCoroutine(CheckpointRoutine());
            }
        }
    }

    private IEnumerator CheckpointRoutine()
    {
        // fade in
        float t = 0f;
        while (t < tempoFadeIn)
        {
            t += Time.deltaTime;
            texto.alpha = Mathf.Lerp(0f, 1f, t / tempoFadeIn);
            yield return null;
        }

        // visível
        yield return new WaitForSeconds(tempoVisivel);

        // fade out
        t = 0f;
        while (t < tempoFadeOut)
        {
            t += Time.deltaTime;
            texto.alpha = Mathf.Lerp(1f, 0f, t / tempoFadeOut);
            yield return null;
        }

        texto.gameObject.SetActive(false);
    }

}