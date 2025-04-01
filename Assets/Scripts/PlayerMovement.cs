using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed;

    float horizontalSpeed;
    float rightLimit = 7.13f;
    float leftLimit = -4.65f;
    float middle = 1.23f;
    
    public float gravity = 5f;
    public float jumpForce = -10f;

    public LayerMask groundMask;

    private int desiredLane = 1;
    
    private bool isGrounded;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;

    }

    void Update()
    {
       // horizontalSpeed = playerSpeed * 1.5f;

        //forward movement
        transform.Translate(Vector3.forward * Time.deltaTime * playerSpeed, Space.World);


        Vector3 targetPosition = new Vector3(middle, transform.position.y, transform.position.z);


        //left directional movement
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            desiredLane--;
            if (desiredLane < 0)
                desiredLane = 0;
        }

        //right directional movement
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            desiredLane++;
            if (desiredLane > 2)
                desiredLane = 2;
        }

        if (desiredLane == 0)
        {
            targetPosition = new Vector3(leftLimit, transform.position.y, transform.position.z);
        }
        
        if (desiredLane == 1)
        {
            targetPosition = new Vector3(middle, transform.position.y, transform.position.z);
        }

        if (desiredLane == 2)
        {
            targetPosition = new Vector3(rightLimit, transform.position.y, transform.position.z);        
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * 5f);

        //jump movement
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }

    }

    void Jump()
    {
        float height = 5f;
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;

            //transform.Translate(Vector3.up * Time.deltaTime * jumpForce, Space.World);
            //transform.position.translate(transform.position.x, transform.position.y * jumpForce * Time.deltaTime, transform.position.z);
        }

        //if (transform.position.y >= height)
        //{
        //    rb.AddForce(Vector3.down * gravity, ForceMode.Impulse);
        //}

        //if (!isGrounded)
        //{
        //    //rb.useGravity
        //}

    }
    private void OnCollisionEnter(Collision collision)
    {
        // Check if touching the ground
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }

}
