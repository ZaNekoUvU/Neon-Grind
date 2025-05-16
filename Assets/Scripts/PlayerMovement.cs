using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed;
    [SerializeField]
    GameObject charAnimation;

    #region lanes
    private int desiredLane = 1;
    float rightLimit = 7.13f;
    float leftLimit = -4.65f;
    float middle = 1.23f;
    #endregion

    public float gravity = 5f;
    public float jumpForce = 30f;

    public LayerMask groundMask;
    private bool isGrounded;
    private Rigidbody rb;

    #region jump pickup fields
    public float jumpBoostTimer = 0.1f;
    public float jumpBoostStrength = 60f;
    private bool boostRestart = false;
    public JumpBoost boostState;
    #endregion

    #region dash pickup fields
    public Dash DashState;
    private bool dashRestart = false;
    public float dashTimer = 0.1f;
    public float dashMultiplier;
    #endregion

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
        
        if (dashRestart)
        {
            dashTimer = 10f;
            dashRestart = false;
        }

        if (DashState.dashActive)
        {
            dashTimer -= Time.deltaTime;
        }

        //forward movement
        if (dashTimer > 0)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * (playerSpeed * dashMultiplier), Space.World);
        }
        else
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
                charAnimation.GetComponent<Animator>().Play("Jump");
                rb.AddForce(Vector3.up * jumpBoostStrength, ForceMode.Impulse);
                isGrounded = false;
            }

            else
            {
                charAnimation.GetComponent<Animator>().Play("Jump");
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if touching the ground
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            charAnimation.GetComponent<Animator>().Play("Running");
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("JumpBoost"))
        {
            JumpBoost pickup = collision.gameObject.GetComponent<JumpBoost>();
            if (pickup != null)
            {
                Debug.Log("Player Took Jump Boost");
                boostRestart = true;
                boostState = pickup;
                boostState.jumpBoostActive = true;
            }
        }

        if (collision.gameObject.CompareTag("Dash"))
        {
            Dash pickup = collision.gameObject.GetComponent<Dash>();
            if (pickup != null)
            {
                Debug.Log("Player Took Dash");
                dashRestart = true;
                DashState = pickup;
                DashState.dashActive = true;
            }
        }
    }
}