using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        // Diz ao Unity para chamar a função "VideoFilmou" quando o vídeo terminar
        videoPlayer.loopPointReached += VideoFilmou;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void VideoFilmou(VideoPlayer vp)
    {
        // Carrega a cena do tutorial
        SceneManager.LoadScene("IlhaTutorial");
    }

    // Opcional: Se o jogador carregar no Espaço ou Enter, passa à frente (Skip)
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("IlhaTutorial");
        }
    }
}