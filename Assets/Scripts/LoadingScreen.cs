using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoClip[] videos; // 3 slots: índice 0, 1, 2


    void Awake()
    {
        // destrói duplicados
        if (videos == null || videos.Length == 0)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        Debug.Log("videoIndex recebido: " + LoadingData.videoIndex);
        Debug.Log("videos.Length: " + videos.Length);

        if (videos == null || videos.Length == 0)
        {
            Debug.LogError("Array vazio!");
            SceneManager.LoadScene(LoadingData.sceneDestino);
            return;
        }

        int index = Mathf.Clamp(LoadingData.videoIndex, 0, videos.Length - 1);
        Debug.Log("index final: " + index);

        videoPlayer.clip = videos[index];
        videoPlayer.Play();
        videoPlayer.loopPointReached += VideoTerminou;
    }

    private void VideoTerminou(VideoPlayer vp)
    {
        SceneManager.LoadScene(LoadingData.sceneDestino);
    }
}