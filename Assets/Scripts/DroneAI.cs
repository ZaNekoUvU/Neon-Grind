using UnityEngine;
using System.Collections;

public class DroneMovement : MonoBehaviour
{
    private float droneSpeed;
    private float targetSpeed;
    private float initialSpeed = 3f; // Slower starting speed
    private float speedTransitionDuration = 5f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform fireOffset;
    private float lastFireTime = 0f;

    private void Start()
    {
        // Get player's base speed (ignores boosts)
        if (PlayerMovement.Instance != null)
        {
            targetSpeed = PlayerMovement.Instance.MovementSpeed;
        }
        else
        {
            targetSpeed = droneSpeed;
        }

        // Start at slower speed and begin speed-up coroutine
        droneSpeed = initialSpeed;
        StartCoroutine(SpeedUpCoroutine());
    }

    private void FireBullet()
    {
        Debug.Log("Firing bullet");

        float bulletSpeed = droneSpeed * 2f;

        GameObject bullet = Instantiate(bulletPrefab, fireOffset.position, fireOffset.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.AddForce(-fireOffset.forward * bulletSpeed, ForceMode.Impulse);

        Destroy(bullet, 4f);
    }

    private IEnumerator SpeedUpCoroutine()
    {
        float elapsedTime = 0f;
        float startSpeed = initialSpeed;

        while (elapsedTime < speedTransitionDuration)
        {
            droneSpeed = Mathf.Lerp(startSpeed, targetSpeed, elapsedTime / speedTransitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        droneSpeed = targetSpeed;
    }

    private void Update()
    {
        // Move forward at the current speed
        transform.Translate(Vector3.forward * Time.deltaTime * droneSpeed, Space.World);

        float timeSinceLastFire = Time.time - lastFireTime;

        if (timeSinceLastFire >= 2f)
        {
            FireBullet();
            lastFireTime = Time.time;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet") || other.CompareTag("Boss"))
        {
            Destroy(gameObject);
        }
    }
}