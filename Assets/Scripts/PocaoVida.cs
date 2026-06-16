using UnityEngine;

public class PocaoVida : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource; // AudioSource num objeto persistente
    [SerializeField] private AudioClip somRecolha;    // som ao apanhar

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (VidaPlayer.vidasAtuais < 3)
            {
                VidaPlayer.vidasAtuais++; // aumenta a vida do jogador

                if (audioSource != null && somRecolha != null)
                    audioSource.PlayOneShot(somRecolha);

                gameObject.SetActive(false);
            }
        }
    }
}