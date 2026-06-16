using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoClip[] videos;

    private string sceneParaCarregar;

    void Start()
    {
        int videoIndex = LoadingData.videoIndex;
        sceneParaCarregar = LoadingData.sceneDestino;

        videoPlayer.clip = videos[videoIndex];
        videoPlayer.Play();
        videoPlayer.loopPointReached += VideoTerminou;
    }

    private void VideoTerminou(VideoPlayer vp)
    {
        SceneManager.LoadScene(sceneParaCarregar);
    }
}