using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    private float droneSpeed = 10f; // Default fallback

    private void Start()
    {
        // Capture the player's base speed ONCE (ignores boosts)
        if (PlayerMovement.Instance != null)
        {
            droneSpeed = PlayerMovement.Instance.MovementSpeed;
        }
    }

    private void Update()
    {
        // Move forward at constant, unboosted speed
        transform.Translate(Vector3.forward * Time.deltaTime * droneSpeed, Space.World);
    }
}