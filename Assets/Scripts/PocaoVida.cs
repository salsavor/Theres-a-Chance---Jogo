using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (VidaPlayer.vidasAtuais < 3)
            {
                VidaPlayer.vidasAtuais++; // aumenta a vida do jogador
                Destroy(gameObject);
            }
        }
    }
}
