using UnityEngine;
using UnityEngine.InputSystem;

public class MenuNavegacao : MonoBehaviour
{
    [SerializeField] private Botao3D[] botoes; // arrasta: Jogar, Creditos, Sair
    private int indiceAtual = 0;
    private PlayerControls controls;

    void Awake()
    {
        controls = new PlayerControls();

        controls.UI.Navigate.performed += ctx =>
        {
            Vector2 dir = ctx.ReadValue<Vector2>();
            if (dir.x > 0.5f || dir.y < -0.5f) Navegar(1);
            else if (dir.x < -0.5f || dir.y > 0.5f) Navegar(-1);
        };

        controls.UI.Submit.performed += ctx => ClicarAtual();
    }

    void Start()
    {
        AtivarHover(indiceAtual);
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    private void Navegar(int direcao)
    {
        DesativarHover(indiceAtual);
        indiceAtual = (indiceAtual + direcao + botoes.Length) % botoes.Length;
        AtivarHover(indiceAtual);
    }

    private void ClicarAtual()
    {
        botoes[indiceAtual].Clicar();
    }

    private void AtivarHover(int index)
    {
        botoes[index].SetHover(true);
    }

    private void DesativarHover(int index)
    {
        botoes[index].SetHover(false);
    }
}