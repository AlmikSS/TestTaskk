using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Camera mainCamera;
    
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;

    private bool isGameStarted;
    private Animator animator;

    public bool IsGameStarted => isGameStarted;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void StartGame()
    {
        isGameStarted = true;
        animator.SetBool("IsGameStart", true);
    }

    public void StopGame()
    {
        isGameStarted = false;
        animator.SetBool("IsGameStart", false);
    }

    private void Update()
    {
        if (!isGameStarted) return;

        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector3 newPosition = transform.localPosition;
        newPosition.z += moveSpeed * Time.deltaTime;

        if (Input.GetMouseButton(0))
        {
            float distanceToCamera = mainCamera.WorldToScreenPoint(transform.position).z;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToCamera));

            float clampedX = Mathf.Clamp(worldPosition.x, minX, maxX);

            float raycastHeightOffset = 1.2f;

            bool isBlockedLeft = Physics.Raycast(transform.position + Vector3.up * raycastHeightOffset, Vector3.left, 0.5f);
            bool isBlockedRight = Physics.Raycast(transform.position + Vector3.up * raycastHeightOffset, Vector3.right, 0.5f);

            if ((clampedX < transform.localPosition.x && !isBlockedLeft) || (clampedX > transform.localPosition.x && !isBlockedRight))
            {
                newPosition.x = clampedX;
            }
        }

        transform.localPosition = newPosition;
    }
}