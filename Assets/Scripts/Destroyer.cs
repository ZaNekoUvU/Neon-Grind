using UnityEngine;

public class Destroyer : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject, 2f);
        }
    }
}
