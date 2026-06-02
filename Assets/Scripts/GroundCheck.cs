using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public Player player; // referência ao script Player do parent

    void Start()
    {
        // obtém o script Player do GameObject pai
        player = gameObject.GetComponentInParent<Player>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // se colidiu com um objeto com tag Ground
            player.grounded = true; // informa o Player que está no chão
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // se saiu da colisão com um objeto com tag Ground
            player.grounded = false; // informa o Player que está no ar
    }
}