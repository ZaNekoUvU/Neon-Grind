using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed = 2;
    public float horizontalSpeed = 3;
    public float rightLimit = 5.5f;
    public float leftLimit = -5.5f;

    void Update()
    {
        //forward movement
        transform.Translate(Vector3.forward * Time.deltaTime * playerSpeed, Space.World);

        //left directional movement
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            Debug.Log("Input left");
            if (this.gameObject.transform.position.x > leftLimit)
            {
                transform.Translate(Vector3.left * Time.deltaTime * horizontalSpeed);
                Debug.Log("Move left");
            }
        }

        //right directional movement
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            Debug.Log("Input right");
            if (this.gameObject.transform.position.x < rightLimit)
            {

                transform.Translate(Vector3.right * Time.deltaTime * horizontalSpeed);
                Debug.Log("Move right");
            }
        }
    }
}
