using UnityEngine;

public class CursedObject : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;   // AudioSource num objeto persistente
    [SerializeField] private AudioClip somAmaldicoado;  // som ao tocar

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            VidaPlayer.vidasAtuais -= 2; // diminui a vida do jogador

            if (audioSource != null && somAmaldicoado != null)
                audioSource.PlayOneShot(somAmaldicoado);

            Destroy(gameObject);
        }
    }
}