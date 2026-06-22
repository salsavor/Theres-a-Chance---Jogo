using UnityEngine;

public class Botao3D : MonoBehaviour
{
    [SerializeField] private Material materialNormal;
    [SerializeField] private Material materialHover;
    [SerializeField] private string sceneParaCarregar;
    [SerializeField] private AudioClip somBtn;
    [SerializeField] private AudioSource audioSource;

    [Header("Creditos")]
    [SerializeField] private CameraCreditos cameraCreditos;
    [SerializeField] private bool abrirCreditos = false;
    [SerializeField] private bool fecharCreditos = false;

    private MeshRenderer meshRenderer;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer == null)
            meshRenderer = GetComponentInChildren<MeshRenderer>();

        if (meshRenderer != null)
            meshRenderer.material = materialNormal;
    }

    // rato
    void OnMouseEnter() => SetHover(true);
    void OnMouseExit() => SetHover(false);
    void OnMouseDown() => Clicar();

    // gamepad
    public void SetHover(bool ativo)
    {
        meshRenderer.material = ativo ? materialHover : materialNormal;
        if (ativo && somBtn != null)
            AudioSource.PlayClipAtPoint(somBtn, transform.position);
    }

    public void Clicar()
    {
        if (fecharCreditos && cameraCreditos != null)
        {
            cameraCreditos.FecharCreditos();
            return;
        }
        if (abrirCreditos && cameraCreditos != null)
        {
            cameraCreditos.AbrirCreditos();
            return;
        }

        if (sceneParaCarregar != "")
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneParaCarregar);
        else
            Application.Quit();
    }
}