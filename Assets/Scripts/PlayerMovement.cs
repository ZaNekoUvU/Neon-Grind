using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed;


    float horizontalSpeed;
    float rightLimit = 9f;
    float leftLimit = -6.2f;
    public float jumpForce = 400f;
    public LayerMask groundMask;

    void Update()
    {
        horizontalSpeed = playerSpeed * 1.5f;

        //forward movement
        transform.Translate(Vector3.forward * Time.deltaTime * playerSpeed, Space.World);

        //left directional movement
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            //prevents players from moving off the map
            if (this.gameObject.transform.position.x > leftLimit)
            {
                transform.Translate(Vector3.left * Time.deltaTime * horizontalSpeed);
            }
        }

        //right directional movement
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            //prevents players from moving off the map
            if (this.gameObject.transform.position.x < rightLimit)
            {
                transform.Translate(Vector3.right * Time.deltaTime * horizontalSpeed);
            }
        }

    }

    void Jump()
    {
        float height = GetComponent<Collider>().bounds.size.y;
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, (height / 2) + 0.1f, groundMask);

        
    }
}
