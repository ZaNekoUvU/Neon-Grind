using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    private float lifeTime = 20f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
