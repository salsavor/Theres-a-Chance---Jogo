using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 2f; // tempo entre ataques em segundos
    private float lastAttackTime; // regista o tempo do último ataque

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player")) // se o Player entrou na zona de ataque
        {
            if (Time.time >= lastAttackTime + attackCooldown) // se o cooldown já passou
            {
                HealthPlayer.life -= 1; // remove 1 vida ao Player
                lastAttackTime = Time.time; // regista o tempo do ataque
                Player.myAnimation.SetBool("damage", true); // ativa a animação de dano
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player")) // se o Player saiu da zona de ataque
        {
            Player.myAnimation.SetBool("damage", false); // desativa a animação de dano
        }
    }
}