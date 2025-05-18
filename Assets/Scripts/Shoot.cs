using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform fireOffset;
    [SerializeField] private float normalFireSpeed = 0.5f;
    [SerializeField] private float boostedFireSpeed = 0.1f;

    public PlayerMovement player;
    public PlayerMovement anim;

    private bool fireContinuously = false;
    private float lastFireTime = 0f;

    void Update()
    {

        if (fireContinuously)
        {
            float currentFireSpeed = player.Active ? boostedFireSpeed : normalFireSpeed;
            float timeSinceLastFire = Time.time - lastFireTime;

            if (timeSinceLastFire >= currentFireSpeed)
            {
                FireBullet();
                lastFireTime = Time.time;
            }
        }
    }

    private void OnAttack(InputValue inputValue)
    {
        fireContinuously = inputValue.isPressed;
    }

    private void FireBullet()
    {
        float bulletSpeed = player.MovementSpeed * 2f;

        GameObject bullet = Instantiate(bulletPrefab, fireOffset.position, fireOffset.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.AddForce(fireOffset.forward * bulletSpeed, ForceMode.Impulse);

        Destroy(bullet, 2f);
    }
}