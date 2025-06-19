using UnityEngine;

public class FireRateBoost : MonoBehaviour
{
    public GameObject fireRateBoost;

    public bool fireRateActive = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(fireRateBoost);
        }
    }
}
