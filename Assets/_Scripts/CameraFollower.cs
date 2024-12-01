using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _moveSpeed;

    private void LateUpdate()
    {
        var target = new Vector3
        {
            x = _target.position.x + _offset.x,
            y = _target.position.y + _offset.y,
            z = _target.position.z + _offset.z,
        };
        
        var pos = Vector3.Lerp(transform.position, target, _moveSpeed * Time.deltaTime); 
        transform.position = pos;
    }
}