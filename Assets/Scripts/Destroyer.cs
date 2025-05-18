using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Obstacle") || other.CompareTag("Boss"))
        {
            Destroy(gameObject);
        }
    }
}
