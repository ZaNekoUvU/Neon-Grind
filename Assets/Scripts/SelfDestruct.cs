using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    private float lifeTime = 50f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
