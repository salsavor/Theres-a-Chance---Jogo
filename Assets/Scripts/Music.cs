using UnityEngine;

public class Music : MonoBehaviour
{
    public static GameObject music; // referência estática ao GameObject de música

    void Start()
    {
        BackgroundMusic(); // inicia a função de música de fundo
    }

    void BackgroundMusic()
    {
        DontDestroyOnLoad(gameObject); // mantém o GameObject ativo ao mudar de cena

        if (music == null) // se não existe nenhum GameObject de música ativo
            music = gameObject; // define este como o GameObject de música
        else
            Destroy(gameObject); // destrói se já existir um para evitar duplicação
    }
}