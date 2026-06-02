using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] private AudioSource auch; // som de dano, editável no inspector

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player")) // se o Player entrou na zona de dano
        {
            HealthPlayer.life -= 1; // remove 1 vida ao Player
            Player.myAnimation.SetBool("damage", true); // ativa a animação de dano
            if (auch != null) auch.Play(); // reproduz o som de dano
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player")) // se o Player saiu da zona de dano
        {
            Player.myAnimation.SetBool("damage", false); // desativa a animação de dano
        }
    }
}