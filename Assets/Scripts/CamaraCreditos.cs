using UnityEngine;
using System.Collections;

public class CameraCreditos : MonoBehaviour
{
    [SerializeField] private Transform posicaoMenu;     // posição inicial (caravana)
    [SerializeField] private Transform posicaoCreditos; // posição dos créditos
    [SerializeField] private float velocidade = 2f;
    public static CameraCreditos instance;
    private bool emTransicao = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AbrirCreditos()
    {
        if (!emTransicao)
            StartCoroutine(MoverCamera(posicaoCreditos));
    }

    public void FecharCreditos()
    {
        if (!emTransicao)
            StartCoroutine(MoverCamera(posicaoMenu));
    }

    private IEnumerator MoverCamera(Transform destino)
    {
        emTransicao = true;

        while (Vector3.Distance(transform.position, destino.position) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, destino.position, Time.deltaTime * velocidade);
            transform.rotation = Quaternion.Lerp(transform.rotation, destino.rotation, Time.deltaTime * velocidade);
            yield return null;
        }

        // snap final para garantir posição exata
        transform.position = destino.position;
        transform.rotation = destino.rotation;

        emTransicao = false;
    }
}
