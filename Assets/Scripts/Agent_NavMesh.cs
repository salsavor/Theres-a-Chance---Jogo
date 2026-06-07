using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 10f;
    public float floatHeight = 1.5f;
    public float rotationSpeed = 5f;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // desativa a rotação automática do NavMeshAgent
        agent.updateRotation = false;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRadius)
        {
            agent.SetDestination(player.position);

            // roda suavemente para o player
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0f;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }
        }
        else
        {
            agent.ResetPath();
        }

        // altura relativa ao chão
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10f))
        {
            float targetY = hit.point.y + floatHeight;
            Vector3 pos = transform.position;
            pos.y = Mathf.Lerp(pos.y, targetY, Time.deltaTime * 5f);
            transform.position = pos;
        }
    }
}