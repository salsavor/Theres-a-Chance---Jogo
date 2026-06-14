using UnityEngine;

public class CheckpointMarker : MonoBehaviour
{
    
     void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            VidaPlayer.checkpointDesafio = transform; // atualiza o checkpoint para o marcador atual
            Debug.Log("Checkpoint atualizado para: " + transform.position);
        }
    }
}

