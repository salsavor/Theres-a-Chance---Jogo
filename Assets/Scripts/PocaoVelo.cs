using UnityEngine;

public class PocaoVelo : MonoBehaviour
{
    [SerializeField] private float duracao = 10f;
    [SerializeField] private float speedBonus = 3f;

    private Player player;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<Player>();

            Player.speed += speedBonus;
            Player.runSpeed += speedBonus;
            Player.Stamina = 999999999f;
            Player.MaxStamina = 999999999f;

            // Invoke no próprio script antes de destruir o objeto
            Invoke("ReverterEfeito", duracao);

            // esconde a poção mas não destrói ainda
            gameObject.SetActive(false);
            Debug.Log("Poção de velocidade coletada! Efeito aplicado por " + duracao + " segundos.");
        }
    }

    private void ReverterEfeito()
    {
        if (player == null) return;

        Player.speed -= speedBonus;
        Player.runSpeed -= speedBonus;
        Player.Stamina = 100f;
        Player.MaxStamina = 100f;
        Debug.Log("Efeito da poção de velocidade terminou. Velocidade revertida.");
    }
}