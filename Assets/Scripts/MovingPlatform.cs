using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Vector3 lastPosition;
    private CharacterController playerCC;
    private Transform player;
    [SerializeField] private LayerMask plataformaLayer;

    void Start()
    {
        lastPosition = transform.position;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerCC = playerObj.GetComponent<CharacterController>();
        }
    }

    void LateUpdate()
    {
        if (player == null || playerCC == null) return;

        Ray ray = new Ray(player.position + Vector3.up * 1f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 4f, plataformaLayer))
        {
            if (hit.transform == transform)
            {
                Vector3 delta = transform.position - lastPosition;
                playerCC.Move(delta);
            }
        }

        lastPosition = transform.position;
    }
}