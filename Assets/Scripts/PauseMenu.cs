using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject painelPausa;
    [SerializeField] private GameObject painelControlos;
    [SerializeField] private CameraOrbit scriptCamara;
    [SerializeField] private Player scriptPlayer;

    [Header("Configurações de Áudio (Pausa)")]
    [SerializeField] private AudioMixerSnapshot somNormalSnapshot; // Estado com som normal
    [SerializeField] private AudioMixerSnapshot somPausaSnapshot;  // Estado com som abafado/baixo
    [SerializeField] private float tempoTransicaoSom = 0.15f; // Velocidade da transição
    [SerializeField] private AudioSource playerAudioSource; // AudioSource do Player para pausar sons de passos, etc.

    private bool pausado = false;

    void Start()
    {
        painelPausa.SetActive(false);
        painelControlos.SetActive(false);

        if (somNormalSnapshot != null)
            somNormalSnapshot.TransitionTo(0f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // se estiver nos controlos, volta para a pausa
            if (painelControlos.activeSelf)
            {
                AbrirPausa();
                return;
            }

            if (pausado)
                Retomar();
            else
                Pausar();
        }
    }

    private void Pausar()
    {
        painelPausa.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pausado = true;

        if (scriptCamara != null) scriptCamara.enabled = false;
        if (scriptPlayer != null) scriptPlayer.enabled = false;
        if (playerAudioSource != null) playerAudioSource.Pause();
        if (somPausaSnapshot != null)
            somPausaSnapshot.TransitionTo(tempoTransicaoSom);
    }

    public void Retomar()
    {
        painelPausa.SetActive(false);
        painelControlos.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pausado = false;

        if (scriptCamara != null) scriptCamara.enabled = true;
        if (scriptPlayer != null) scriptPlayer.enabled = true;
        if (playerAudioSource != null) playerAudioSource.UnPause();
        if (somNormalSnapshot != null)
            somNormalSnapshot.TransitionTo(tempoTransicaoSom);
    }

    public void AbrirControlos()
    {
        painelPausa.SetActive(false);
        painelControlos.SetActive(true);
    }

    private void AbrirPausa()
    {
        painelControlos.SetActive(false);
        painelPausa.SetActive(true);
    }

    public void SairDoJogo()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}