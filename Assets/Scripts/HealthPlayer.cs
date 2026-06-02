using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthPlayer : MonoBehaviour
{
    [SerializeField] private GameObject life1, life2, life3; // sprites de vida, editáveis no inspector
    public static int life; // valor das vidas, static para ser acedido externamente

    void Start()
    {
        life = 3; // inicia o jogo com 3 vidas
        life1.gameObject.SetActive(true); // ativa a sprite da 1ª vida
        life2.gameObject.SetActive(true); // ativa a sprite da 2ª vida
        life3.gameObject.SetActive(true); // ativa a sprite da 3ª vida
    }

    void Update()
    {
        health(); // verifica o estado das vidas a cada frame
    }

    private void health()
    {
        switch (life) // verifica o valor atual das vidas
        {
            case 3: // se tem 3 vidas
                life1.gameObject.SetActive(true);
                life2.gameObject.SetActive(true);
                life3.gameObject.SetActive(true);
                break;
            case 2: // se tem 2 vidas
                life1.gameObject.SetActive(true);
                life2.gameObject.SetActive(true);
                life3.gameObject.SetActive(false); // desativa a 3ª vida
                break;
            case 1: // se tem 1 vida
                life1.gameObject.SetActive(true);
                life2.gameObject.SetActive(false); // desativa a 2ª vida
                life3.gameObject.SetActive(false);
                break;
            case 0: // se não tem vidas
                life1.gameObject.SetActive(false);
                life2.gameObject.SetActive(false);
                life3.gameObject.SetActive(false);
                SceneManager.LoadScene("GameOver"); // carrega a cena de Game Over
                break;
        }
    }
}