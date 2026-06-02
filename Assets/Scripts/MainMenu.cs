using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("nivel1"); // carrega o nível 1
    }

    public void QuitGame()
    {
        Application.Quit(); // fecha a aplicação
        Debug.Log("Estamos Fora da aplicação"); // confirma na consola que a aplicação fechou
    }
}