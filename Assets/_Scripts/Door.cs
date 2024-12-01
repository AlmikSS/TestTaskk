using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private int _currentStete;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            if (player.State >= _currentStete)
            {
                GetComponent<Animator>().SetBool("Open", true);
            }
            else
            {
                player.Stop();
                GameManager.Instance.Win(_currentStete);
            }
        }
    }
}
