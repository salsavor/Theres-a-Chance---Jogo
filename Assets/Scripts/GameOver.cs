using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void RestartLevel()
    {
        HealthPlayer.life = 3; // reset das vidas antes de reiniciar
        SceneManager.LoadScene("nivel1"); // carrega o nível 1
    }

    public void MainMenu()
    {
        HealthPlayer.life = 3; // reset das vidas antes de ir ao menu
        SceneManager.LoadScene("GameMenu"); // carrega o menu principal
    }
}