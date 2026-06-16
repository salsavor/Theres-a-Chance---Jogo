using UnityEngine;

public class PocaoVelo : MonoBehaviour
{
    [SerializeField] private float duracao = 10f;
    [SerializeField] private float speedBonus = 3f;
    [SerializeField] private AudioSource audioSource; // AudioSource num objeto persistente
    [SerializeField] private AudioClip somRecolha;    // som ao apanhar

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

            if (audioSource != null && somRecolha != null)
                audioSource.PlayOneShot(somRecolha);

            Invoke("ReverterEfeito", duracao);

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