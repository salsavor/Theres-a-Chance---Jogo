using UnityEngine;
using UnityEngine.EventSystems;

public class BotaoHoverZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float escalaHover = 1.1f;
    [SerializeField] private float velocidade = 8f;
    [SerializeField] private AudioSource audioSource; // AudioSource na camara
    [SerializeField] private AudioClip somBtn;
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
    public void OnPointerEnter(PointerEventData eventData)
    {
        hover = true;

        if (audioSource != null && somBtn != null)
        {
            audioSource.PlayOneShot(somBtn);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hover = false;
    }
}