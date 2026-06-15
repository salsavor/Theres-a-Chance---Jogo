using UnityEngine;

public class DetecaoInimigo : MonoBehaviour
{
    [SerializeField] private string tagPlayer = "Player";

    private ControladorInimigo controlador; // referência ao script do pai (Agro1)

    void Start()
    {
        // procura o ControladorInimigo no objeto pai (Agro1)
        controlador = GetComponentInParent<ControladorInimigo>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // se o player entrou na zona, começa a perseguição
        if (other.CompareTag(tagPlayer))
        {
            if (controlador != null)
                controlador.IniciarPerseguicao();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // se o player saiu da zona, para a perseguição
        if (other.CompareTag(tagPlayer))
        {
            if (controlador != null)
                controlador.PararPerseguicao();
        }
    }
}