using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Função para o botão Play
    public void Play()
    {
        SceneManager.LoadScene("CutsceneInicial");
    }

    public void Quit()
    {
        Debug.Log("estas fora do jogo");
        Application.Quit(); 
    }
}