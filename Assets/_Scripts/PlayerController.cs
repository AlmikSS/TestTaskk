using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;

    private bool _isGameStarted;
    
    public bool IsGameStarted => _isGameStarted;
    
    public void StartGame()
    {
        _isGameStarted = true;
        GetComponentInChildren<Animator>().SetBool("IsGameStart", true);
    }

    public void StopGame()
    {
        _isGameStarted = false;
        GetComponentInChildren<Animator>().SetBool("IsGameStart", false);
    }
    
    private void Update()
    {
        if (!_isGameStarted) return;

        var newPosition = transform.localPosition;
        newPosition.z += _moveSpeed * Time.deltaTime;

        if (Input.GetMouseButton(0))
        {
            var distanceToCamera = _camera.WorldToScreenPoint(transform.position).z;
            var worldPosition = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToCamera));

            var clampedX = Mathf.Clamp(worldPosition.x, _minX, _maxX);

            var raycastHeightOffset = 1.2f;

            var isBlockedLeft = Physics.Raycast(transform.position + Vector3.up * raycastHeightOffset, Vector3.left, 0.5f);
            var isBlockedRight = Physics.Raycast(transform.position + Vector3.up * raycastHeightOffset, Vector3.right, 0.5f);

            if ((clampedX < transform.localPosition.x && !isBlockedLeft) || (clampedX > transform.localPosition.x && !isBlockedRight))
            {
                newPosition.x = clampedX;
            }
        }

        transform.localPosition = newPosition;
    }
}