using UnityEngine;
using System.Collections;
using TMPro;

public class CheckpointMarker : MonoBehaviour
{

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
        if (other.gameObject.CompareTag("Player"))
        {
            VidaPlayer.checkpointDesafio = transform; // atualiza o checkpoint para o marcador atual
            Debug.Log("Checkpoint atualizado para: " + transform.position);
            texto.gameObject.SetActive(true); // mostra o texto
            StartCoroutine(CheckpointRoutine());
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
}

