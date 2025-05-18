using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    private float lifeTime = 15f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
