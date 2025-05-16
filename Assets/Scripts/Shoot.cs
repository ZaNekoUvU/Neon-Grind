using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;
    
    public PlayerMovement player;

    private float bulletSpeed;

    [SerializeField]
    private Transform fireOffset;

    [SerializeField]
    private float fireSpeed;

    private bool fireContinuosuly;
    private float lastFireTime;

    private void Start()
    {
        
        
    }
    void Update()
    {
        if (bulletSpeed == 0f && player.MovementSpeed > 0f)
        {
            bulletSpeed = (player.MovementSpeed) * 2f;
        }

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
        bulletSpeed = player.MovementSpeed * 2f;

        GameObject bullet = Instantiate(bulletPrefab, fireOffset.position, fireOffset.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        rb.useGravity = false;        
        rb.linearDamping = 0f;                 
        rb.angularDamping = 0f;          

        rb.AddForce(fireOffset.forward * bulletSpeed, ForceMode.Impulse); // Use Impulse for instant force

        Destroy(bullet, 2f);
    }
}
