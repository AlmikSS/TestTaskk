using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float moveSpeed;

    private void LateUpdate()
    {
        Vector3 targetPosition = new Vector3
        {
            x = target.position.x + offset.x,
            y = target.position.y + offset.y,
            z = target.position.z + offset.z,
        };
        
        Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime); 
        transform.position = newPosition;
    }
}