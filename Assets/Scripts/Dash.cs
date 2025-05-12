using UnityEngine;

public class Dash : MonoBehaviour
{
    public GameObject dash;

    public bool dashActive = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(dash);
        }
    }
}
