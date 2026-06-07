using UnityEngine;

public class DetecaoInimigo : MonoBehaviour
{
    [SerializeField] private float dano = 20f;        // dano causado ao player
    [SerializeField] private string tagPlayer = "Player"; // tag do player

    // chamado quando algo ENTRA no trigger (versão 3D, não 2D)
    private void OnTriggerEnter(Collider other)
    {
        // verifica se quem entrou foi o player
        if (other.CompareTag(tagPlayer))
        {
            Atacar(other);
        }
    }

    // opcional: dano contínuo enquanto o player permanece dentro
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(tagPlayer))
        {
            // se quiseres dano por contacto contínuo, descomenta abaixo
            // Atacar(other);
        }
    }

    private void Atacar(Collider player)
    {
        // aqui chamas o sistema de vida do player
        Debug.Log("Inimigo detetou e atacou o player! Dano: " + dano);

        // exemplo: se tiveres um script VidaPlayer no player
        // VidaPlayer vida = player.GetComponent<VidaPlayer>();
        // if (vida != null) vida.ReceberDano(dano);
    }
}