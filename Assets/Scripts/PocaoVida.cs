using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (VidaPlayer.vidasAtuais < 3)
            {
                VidaPlayer.vidasAtuais++; // aumenta a vida do jogador
                Destroy(gameObject);
            }
        }
    }
}
