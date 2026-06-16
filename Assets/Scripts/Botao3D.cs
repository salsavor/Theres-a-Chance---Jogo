using UnityEngine;

public class Botao3D : MonoBehaviour
{
    [SerializeField] private Material materialNormal;
    [SerializeField] private Material materialHover;
    [SerializeField] private string sceneParaCarregar; // "Cutscene" no Play, deixa vazio no Quit

    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = materialNormal;
    }

    void OnMouseEnter()
    {
        meshRenderer.material = materialHover;
    }

    void OnMouseExit()
    {
        meshRenderer.material = materialNormal;
    }

    void OnMouseDown()
    {
        if (sceneParaCarregar != "")
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneParaCarregar);
        else
            Application.Quit();
    }
}