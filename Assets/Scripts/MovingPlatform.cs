using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Transform playerOnPlatform = null;
    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
    }

    void LateUpdate()
    {
        if (playerOnPlatform != null)
        {
            // Calcula quanto a plataforma se moveu neste frame
            Vector3 delta = transform.position - lastPosition;
            // Aplica esse mesmo movimento ao jogador
            playerOnPlatform.GetComponent<CharacterController>().Move(delta);
        }

        lastPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerOnPlatform = other.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerOnPlatform = null;
    }
}