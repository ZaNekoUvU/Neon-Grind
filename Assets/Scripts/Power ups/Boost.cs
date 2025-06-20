using Unity.VisualScripting;
using UnityEngine;

public class JumpBoost : MonoBehaviour
{
    public GameObject jumpBoost;

    public bool jumpBoostActive = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EventManager.Instance?.PostNotification(NeonGrindEvents.PICKUP_OBTAINED, this, "JumpBoost");
            Destroy(jumpBoost);
        }
    }
}