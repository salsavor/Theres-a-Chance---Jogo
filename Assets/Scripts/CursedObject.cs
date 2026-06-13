using UnityEngine;

public class CursedObject : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
                VidaPlayer.vidasAtuais -= 2; // diminui a vida do jogador
                Destroy(gameObject);
        }
    }
}
