using UnityEngine;

public class BotaoHoverZoom : MonoBehaviour
{
    [SerializeField] private float escalaHover = 1.1f;
    [SerializeField] private float velocidade = 8f;
    private Vector3 escalaOriginal;
    private bool hover = false;

    void Start()
    {
        escalaOriginal = transform.localScale;
    }

    void Update()
    {
        Vector3 alvo = hover ? escalaOriginal * escalaHover : escalaOriginal;
        transform.localScale = Vector3.Lerp(transform.localScale, alvo, Time.unscaledDeltaTime * velocidade);
    }

    // adiciona o EventTrigger no Inspector ou usa estas funções
    public void OnHoverEnter() => hover = true;
    public void OnHoverExit() => hover = false;
}