using UnityEngine;
using UnityEngine.EventSystems;

public class BotaoHoverZoom : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float escalaHover = 1.1f;
    [SerializeField] private float velocidade = 8f;
    [SerializeField] private AudioClip somHover;
    [SerializeField] private AudioSource audioSource;
    private Vector3 escalaOriginal;
    private bool hover = false;

    void Start() => escalaOriginal = transform.localScale;

    void Update()
    {
        Vector3 alvo = hover ? escalaOriginal * escalaHover : escalaOriginal;
        transform.localScale = Vector3.Lerp(transform.localScale, alvo, Time.unscaledDeltaTime * velocidade);
    }

    // rato
    public void OnPointerEnter(PointerEventData e) => AtivarHover();
    public void OnPointerExit(PointerEventData e) => DesativarHover();

    // gamepad/teclado
    public void OnSelect(BaseEventData e) => AtivarHover();
    public void OnDeselect(BaseEventData e) => DesativarHover();

    private void AtivarHover()
    {
        hover = true;
        if (somHover != null)
            AudioSource.PlayClipAtPoint(somHover, transform.position);
    }

    private void DesativarHover() => hover = false;
}