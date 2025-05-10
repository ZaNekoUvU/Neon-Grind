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
    public float jumpForce = 30f;

    public LayerMask groundMask;

    private int desiredLane = 1;

    private bool isGrounded;
    private Rigidbody rb;

    public float jumpBoostTimer = 0.1f;
    public float boostJumpStrength = 60f;

    private bool boostRestart = false;

    public Boost boostState;

    private bool lifeActive;
    public ExtraLife LifeState;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;

        boostState.jumpBoostActive = true;
    }

    void Update()
    {
        if (boostRestart)
        {
            jumpBoostTimer = 10f;
            boostRestart = false;
        }

        if (boostState.jumpBoostActive)
        {
            jumpBoostTimer -= Time.deltaTime;

        }
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
            if (jumpBoostTimer <= 0)
            {
                boostState.jumpBoostActive = false;
            }

            Jump();
        }

    }

    public void Jump()
    {
        if (isGrounded)
        {
            //Debug.Log(jumpBoostState + " jump state");
            if (jumpBoostTimer > 0)
            {
                rb.AddForce(Vector3.up * boostJumpStrength, ForceMode.Impulse);
                isGrounded = false;

                Debug.Log("jump boost on" + boostState.jumpBoostActive);
            }

            else
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
                //boostState.jumpBoostActive = false;

                Debug.Log("jump boost not on" + boostState.jumpBoostActive);
            }

        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        // Check if touching the ground
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("JumpBoost"))
        {
            Debug.Log("Player Took boost");
            boostRestart = true;
            boostState.jumpBoostActive = true;
        }

        if (collision.gameObject.CompareTag("ExtraLife"))
        {
            Debug.Log("Player Took extra life");
            lifeActive = true;
            LifeState.extraLifeActive = true;
        }
    }

}