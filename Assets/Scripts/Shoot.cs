using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private float bulletSpeed;

    [SerializeField]
    private Transform fireOffset;

    [SerializeField]
    private float fireSpeed;

    private bool fireContinuosuly;
    private float lastFireTime;

    // Update is called once per frame
    void Update()
    {
        if (fireContinuosuly)
        {
            float timeSinceLastFire = Time.time - lastFireTime;

            if (timeSinceLastFire >= fireSpeed)
            {
                fireBullet();

                lastFireTime= Time.time;
            }
            
        }
    }

    private void OnAttack(InputValue inputValue)
    {
        fireContinuosuly = inputValue.isPressed;
    }

    private void fireBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, fireOffset.position, transform.rotation); 
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * bulletSpeed, ForceMode.VelocityChange);
    }
}
