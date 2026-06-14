using UnityEngine;

public class DefaultDamage : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<VidaPlayer>().PerderVida();
        }
    }
}
