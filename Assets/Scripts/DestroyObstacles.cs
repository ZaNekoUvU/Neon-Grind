using UnityEngine;

public class DestroyObstacles : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle") || other.CompareTag("PickUp"))
        {
            Destroy(other.gameObject);
        }
    }
    
}
