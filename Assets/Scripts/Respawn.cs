using UnityEngine;

public class Respawn : MonoBehaviour
{
    public Player player; // referência ao script Player
    [SerializeField] private Transform respawnPoint; // posição onde o player vai reaparecer
    [SerializeField] private AudioSource collide; // som de colisão, editável no inspector

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player")) // se o Player entrou na zona de respawn
        {
            HealthPlayer.life -= 1; // remove 1 vida ao Player
            player.transform.position = respawnPoint.transform.position; // move o Player para o respawnPoint
            player.playerigidbody3D.linearVelocity = Vector3.zero; // reset da velocidade do Rigidbody
            if (collide != null) collide.Play(); // reproduz o som de colisão
        }
    }
}