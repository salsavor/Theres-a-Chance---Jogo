using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player")) // se o Player entrou na zona de saída do nível
        {
            string currentScene = SceneManager.GetActiveScene().name; // obtém o nome da cena atual

            if (currentScene == "nivel1") // se está no nível 1
                SceneManager.LoadScene("nivel2"); // carrega o nível 2
            else if (currentScene == "nivel2") // se está no nível 2
            {
                HealthPlayer.life = 3; // reset das vidas
                SceneManager.LoadScene("GameMenu"); // carrega o menu principal
            }
        }
    }
}