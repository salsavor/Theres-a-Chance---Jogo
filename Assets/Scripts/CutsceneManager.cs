using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private string nomeScene;
    [SerializeField] private int videoIndex;
    [SerializeField] private bool usarLoadingScreen = true; // desativar para n ter loadingScreen na cutscene final

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += VideoTerminou;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void MudarScene()
    {
        if (usarLoadingScreen)
        {
            LoadingData.sceneDestino = nomeScene;
            LoadingData.videoIndex = videoIndex;
            SceneManager.LoadScene("LoadingScreen");
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(nomeScene);
        }
    }

    private void VideoTerminou(VideoPlayer vp) => MudarScene();

    void Update()
    {
        if (Input.anyKeyDown)
            MudarScene();
    }
}