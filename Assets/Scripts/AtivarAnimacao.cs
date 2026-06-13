using UnityEngine;

public class AtivarAnimacao : MonoBehaviour
{
    [SerializeField] private string tagPlayer = "Player";    // tag do player
    [SerializeField] private Animator animatorAlvo;          // Animator do OUTRO objeto
    [SerializeField] private string nomeTrigger = "Ativar";  // parâmetro Trigger no Animator
    [SerializeField] private bool umaVezSo = true;           // ativa só na primeira passagem
    private bool jaAtivou = false;

    private void OnTriggerEnter(Collider other)
    {
        // só reage ao player
        if (other.CompareTag(tagPlayer))
        {
           

            if (animatorAlvo != null)
            {
                animatorAlvo.SetTrigger(nomeTrigger); // dispara a animação no outro objeto
                jaAtivou = true;
            }
        }
    }
}