using UnityEngine;

public class Death : MonoBehaviour
{


    private void OnCollisionEnter(Collision collide)
    {
        Debug.Log("Collided");
        PlayerMovement movementScript = collide.gameObject.GetComponent<PlayerMovement>();

        GameObject gm = GameObject.Find("LevelControls"); //finds the level control game object
        Generator generatorScript = gm.GetComponent<Generator>();

        if (collide.gameObject.CompareTag("Player"))
        {
            Debug.Log("player collided");
            movementScript.enabled = false;

            if (generatorScript != null)
            {
                generatorScript.enabled = false;
            }
        }
    }
}
