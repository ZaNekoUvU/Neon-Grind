using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform fireOffset;
    [SerializeField] private float normalFireSpeed = 1f;
    [SerializeField] private float boostedFireSpeed = 0.5f;

    private float currentFireSpeed;

    public PlayerMovement player;
    public FireRateBoost fireRateState;

    private bool buff = false;

    private float lastFireTime = 0f;

    void Update()
    {
        buff = player.rateActive;

        //increase fire rate if buff is active
        if (buff)
        {
            currentFireSpeed = boostedFireSpeed;
        }
        else
        {
            currentFireSpeed = normalFireSpeed;

        }

        //cooldown on bullet fire
        float timeSinceLastFire = Time.time - lastFireTime;

        if (timeSinceLastFire >= currentFireSpeed)
        {
            FireBullet();
            lastFireTime = Time.time;
        }
    }

    //create, fire and destroy bullet
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