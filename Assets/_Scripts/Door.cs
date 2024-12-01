using UnityEngine;

// Door handles the interaction with the player to determine game state changes
public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private int currentState;
    
    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has a Player component
        if (other.gameObject.TryGetComponent(out Player player))
        {
            // Check if the player's state is greater than or equal to the door's current state
            if (player.State >= currentState)
            {
                // Open the door
                GetComponent<Animator>().SetBool("Open", true);
            }
            else
            {
                // Stop the game and trigger a win condition
                player.StopGame();
                GameManager.Instance.Win(currentState);
            }
        }
    }
}
