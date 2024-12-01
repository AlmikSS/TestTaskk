using UnityEngine;

public class FlagZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetComponent<Animator>().SetBool("Open", true);
        }
    }
}