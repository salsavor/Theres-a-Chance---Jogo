using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] private Transform alvo;
    [SerializeField] private float alturaPivot = 1.6f;
    [SerializeField] private float distancia = 4f;
    [SerializeField] private float sensibilidade = 3f;
    [SerializeField] private float sensibilidadeGamepad = 150f; // gamepad precisa de valor maior
    [SerializeField] private float pitchMin = -30f;
    [SerializeField] private float pitchMax = 70f;
    [SerializeField] private float distanciaMinima = 0.5f;
    [SerializeField] private float offsetColisao = 0.2f;
    [SerializeField] private LayerMask camaraColide;

    private float yaw;
    private float pitch;

    private PlayerControls controls;
    private Vector2 lookInput;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    void Start()
    {
        yaw = alvo.eulerAngles.y;
        pitch = 15f;
    }

    void LateUpdate()
    {
        if (alvo == null) return;

        // deteta se é gamepad ou rato
        bool isGamepad = Gamepad.current != null && lookInput != Vector2.zero 
                         && Mouse.current.delta.ReadValue() == Vector2.zero;

        float sens = isGamepad ? sensibilidadeGamepad * Time.deltaTime : sensibilidade;

        yaw += lookInput.x * sens;
        pitch -= lookInput.y * sens;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        Quaternion rotacao = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 pivot = alvo.position + Vector3.up * alturaPivot;
        Vector3 posicaoDesejada = pivot + rotacao * (Vector3.back * distancia);

        Vector3 direcao = posicaoDesejada - pivot;
        float distanciaFinal = distancia;

        if (Physics.Raycast(pivot, direcao.normalized, out RaycastHit hit, distancia, camaraColide))
            distanciaFinal = Mathf.Clamp(hit.distance - offsetColisao, distanciaMinima, distancia);

        transform.position = pivot + rotacao * (Vector3.back * distanciaFinal);
        transform.LookAt(pivot);
    }
}