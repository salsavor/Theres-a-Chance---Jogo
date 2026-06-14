using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] private Transform alvo;
    [SerializeField] private float alturaPivot = 1.6f;
    [SerializeField] private float distancia = 4f;

    [SerializeField] private float sensibilidade = 3f;
    [SerializeField] private float pitchMin = -30f;
    [SerializeField] private float pitchMax = 70f;

    // --- anti-clipping ---
    [SerializeField] private float distanciaMinima = 0.5f;  // distância mínima ao player
    [SerializeField] private float offsetColisao = 0.2f;    // margem antes da parede
    [SerializeField] private LayerMask camaraColide;        // layers que bloqueiam a câmara

    private float yaw;
    private float pitch;

    void Start()
    {
        yaw = alvo.eulerAngles.y;
        pitch = 15f;
    }

    void LateUpdate()
    {
        if (alvo == null) return;

        yaw += Input.GetAxis("Mouse X") * sensibilidade;
        pitch -= Input.GetAxis("Mouse Y") * sensibilidade;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        Quaternion rotacao = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 pivot = alvo.position + Vector3.up * alturaPivot;
        Vector3 posicaoDesejada = pivot + rotacao * (Vector3.back * distancia);

        // raycast do pivot até à posição desejada da câmara
        Vector3 direcao = posicaoDesejada - pivot;
        float distanciaFinal = distancia;

        if (Physics.Raycast(pivot, direcao.normalized, out RaycastHit hit, distancia, camaraColide))
        {
            // para antes da parede com uma pequena margem
            distanciaFinal = Mathf.Clamp(hit.distance - offsetColisao, distanciaMinima, distancia);
        }

        transform.position = pivot + rotacao * (Vector3.back * distanciaFinal);
        transform.LookAt(pivot);
    }
}