using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] private Transform alvo;            // o player
    [SerializeField] private float alturaPivot = 1.6f;  // ponto do player a centrar
    [SerializeField] private float distancia = 4f;      // distância ao player

    [SerializeField] private float sensibilidade = 3f;  // sensibilidade do rato
    [SerializeField] private float pitchMin = -30f;     // limite a olhar de baixo
    [SerializeField] private float pitchMax = 70f;      // limite a olhar de cima

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

        // o rato controla yaw (horizontal) e pitch (vertical)
        yaw += Input.GetAxis("Mouse X") * sensibilidade;
        pitch -= Input.GetAxis("Mouse Y") * sensibilidade;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        // posição em órbita à volta do player
        Quaternion rotacao = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 pivot = alvo.position + Vector3.up * alturaPivot;
        transform.position = pivot + rotacao * (Vector3.back * distancia);

        // aponta sempre para o player → mantém-no no centro do ecrã
        transform.LookAt(pivot);
    }
}
