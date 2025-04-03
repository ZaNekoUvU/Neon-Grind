using Unity.VisualScripting;
using UnityEngine;

public class Boost : MonoBehaviour
{
    public GameObject boost;

    public bool jumpBoostActive = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(boost);
        }
    }
}