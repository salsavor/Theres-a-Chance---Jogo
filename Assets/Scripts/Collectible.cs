using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource; // AudioSource num objeto persistente
    [SerializeField] private AudioClip somRecolha;    // som ao apanhar

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.AdicionarColecionavel();

            // toca o som através do AudioSource externo (sobrevive ao SetActive)
            if (audioSource != null && somRecolha != null)
                audioSource.PlayOneShot(somRecolha);

            gameObject.SetActive(false);
        }
    }
}