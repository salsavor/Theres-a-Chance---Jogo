using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject painelPausa;
    [SerializeField] private GameObject painelControlos;
    [SerializeField] private CameraOrbit scriptCamara;
    [SerializeField] private Player scriptPlayer;

    [SerializeField] private GameObject primeiroBotaoPausa;     // botão Continuar
    [SerializeField] private GameObject primeiroBotaoControlos; // botão Voltar

    [Header("Áudio")]
    [SerializeField] private AudioMixerSnapshot somNormalSnapshot;
    [SerializeField] private AudioMixerSnapshot somPausaSnapshot;
    [SerializeField] private float tempoTransicaoSom = 0.15f;
    [SerializeField] private AudioSource playerAudioSource;

    public static bool pausado = false;
    private PlayerControls controls;

    void Awake()
    {
        pausado = false;
        controls = new PlayerControls();

        controls.Player.Pause.performed += ctx =>
        {
            if (painelControlos.activeSelf)
            {
                AbrirPausa();
                return;
            }

            if (pausado) Retomar();
            else Pausar();
        };
    }

    void Start()
    {
        painelPausa.SetActive(false);
        painelControlos.SetActive(false);

        if (somNormalSnapshot != null)
            somNormalSnapshot.TransitionTo(0f);
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    private void Pausar()
    {
        painelPausa.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pausado = true;

        // seleciona o primeiro botão para o gamepad navegar
        EventSystem.current.SetSelectedGameObject(primeiroBotaoPausa);

        if (scriptCamara != null) scriptCamara.enabled = false;
        if (scriptPlayer != null) scriptPlayer.enabled = false;
        if (playerAudioSource != null) playerAudioSource.Pause();
        if (somPausaSnapshot != null) somPausaSnapshot.TransitionTo(tempoTransicaoSom);
    }

    public void Retomar()
    {
        painelPausa.SetActive(false);
        painelControlos.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pausado = false;

        EventSystem.current.SetSelectedGameObject(null);

        if (scriptCamara != null) scriptCamara.enabled = true;
        if (scriptPlayer != null) scriptPlayer.enabled = true;
        if (playerAudioSource != null) playerAudioSource.UnPause();
        if (somNormalSnapshot != null) somNormalSnapshot.TransitionTo(tempoTransicaoSom);
    }

    public void AbrirControlos()
    {
        painelPausa.SetActive(false);
        painelControlos.SetActive(true);
        EventSystem.current.SetSelectedGameObject(primeiroBotaoControlos);
    }

    private void AbrirPausa()
    {
        painelControlos.SetActive(false);
        painelPausa.SetActive(true);
        EventSystem.current.SetSelectedGameObject(primeiroBotaoPausa);
    }

    public void SairDoJogo()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}