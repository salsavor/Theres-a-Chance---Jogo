using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player")) // se o Player colidiu com o objeto de vida
        {
            if (HealthPlayer.life < 3) // só recupera vida se não estiver no máximo
                HealthPlayer.life += 1; // incrementa 1 vida
            Destroy(gameObject); // destrói o objeto de vida
        }
    }
}