using UnityEngine;
using System.Collections;

public class AtivarAnimacao : MonoBehaviour
{
    [SerializeField] private string tagPlayer = "Player";    // tag do player
    [SerializeField] private Animator animatorAlvo;          // Animator do OUTRO objeto
    [SerializeField] private string nomeTrigger = "Ativar";  // parâmetro Trigger no Animator

    [Header("Audio")]
    [SerializeField] private AudioSource murmuriosSource;    // murmúrios (Play On Awake + Loop)
    [SerializeField] private AudioSource soundtrackSource;   // soundtrack do nível
    [SerializeField] private float duracaoAnimacao = 2.6f;   // duração da animação em segundos (campo Length da clip)
    [SerializeField] private float fadeOutMurmurios = 0.5f;  // tempo de fade-out dos murmúrios

    private bool jaAtivou = false;

    private void OnTriggerEnter(Collider other)
    {
        // evita disparar mais do que uma vez
        if (jaAtivou) return;

        // só reage ao player
        if (other.CompareTag(tagPlayer))
        {
            if (animatorAlvo != null)
            {
                animatorAlvo.SetTrigger(nomeTrigger); // dispara a animação no outro objeto
                jaAtivou = true;
                StartCoroutine(TrocarAudio());        // trata da transição de som
            }
        }
    }

    private IEnumerator TrocarAudio()
    {
        // espera a animação desenrolar até ao fim
        yield return new WaitForSeconds(duracaoAnimacao);

        // fade-out suave dos murmúrios e depois para
        if (murmuriosSource != null && murmuriosSource.isPlaying)
        {
            float volumeInicial = murmuriosSource.volume;
            float t = 0f;

            while (t < fadeOutMurmurios)
            {
                t += Time.deltaTime;
                murmuriosSource.volume = Mathf.Lerp(volumeInicial, 0f, t / fadeOutMurmurios);
                yield return null;
            }

            murmuriosSource.Stop();
            murmuriosSource.volume = volumeInicial; // repõe o volume original
        }

        // arranca a soundtrack que acompanha o player até ao fim do nível
        if (soundtrackSource != null)
        {
            soundtrackSource.loop = true;
            soundtrackSource.Play();
        }
    }
}