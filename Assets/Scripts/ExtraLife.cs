using UnityEngine;

public class ExtraLife : MonoBehaviour
{
    public GameObject life;

    public bool extraLifeActive = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(life);
        }
    }
}
