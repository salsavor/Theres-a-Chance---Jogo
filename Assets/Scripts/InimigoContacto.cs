using UnityEngine;

public class InimigoContacto : MonoBehaviour
{
    [SerializeField] private string tagPlayer = "Player"; // tag do player

    // dispara quando algo entra no collider trigger do inimigo
    private void OnTriggerEnter(Collider other)
    {
        // só reage ao player
        if (other.CompareTag(tagPlayer))
        {
            // procura o VidaPlayer no objeto que entrou
            VidaPlayer vida = other.GetComponent<VidaPlayer>();
            if (vida != null)
                vida.PerderVida(); // tira uma vida e teletransporta para o checkpoint
        }
    }
}