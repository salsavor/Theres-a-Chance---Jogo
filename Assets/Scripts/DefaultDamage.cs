using UnityEngine;

public class DefaultDamage : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            VidaPlayer.vidasAtuais--; // diminui a vida do jogador
            Destroy(gameObject);
        }
    }
}
