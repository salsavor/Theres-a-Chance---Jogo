using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoClip[] videos; // 3 slots: índice 0, 1, 2

    void Start()
    {
        videoPlayer.clip = videos[LoadingData.videoIndex];
        videoPlayer.Play();
        videoPlayer.loopPointReached += VideoTerminou;
    }

    private void VideoTerminou(VideoPlayer vp)
    {
        SceneManager.LoadScene(LoadingData.sceneDestino);
    }
}