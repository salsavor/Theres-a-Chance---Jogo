using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private string nomeScene;
    [SerializeField] private int videoIndex;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        // quando o video terminar, passa para a proxima scene
        videoPlayer.loopPointReached += VideoTerminou;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void VideoTerminou(VideoPlayer vp)
    {
        LoadingData.sceneDestino = nomeScene;
        LoadingData.videoIndex = videoIndex;
        SceneManager.LoadScene("LoadingScreen");
    }

    // clicar passa a cutscene
    void Update()
    {
        if (Input.anyKeyDown)
        {
            LoadingData.sceneDestino = nomeScene;
            LoadingData.videoIndex = videoIndex;
            SceneManager.LoadScene("LoadingScreen");
        }
    }
}