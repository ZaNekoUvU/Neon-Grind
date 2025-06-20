using UnityEngine;

public class FireRateBoost : MonoBehaviour
{
    public GameObject fireRateBoost;

    public bool fireRateActive = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EventManager.Instance?.PostNotification(NeonGrindEvents.PICKUP_OBTAINED, this, "FireRateBoost");
            Destroy(fireRateBoost);
        }
    }
}
