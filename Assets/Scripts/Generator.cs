using System.Collections;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public GameObject[] Sections = new GameObject[3];

    public int zPos = 50;
    public bool isCreating = false;
    public int segmentNum;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        if (isCreating == false)
        {
            isCreating = true;
            StartCoroutine(Gen());
        }
        
    }

    IEnumerator Gen()
    {
        segmentNum = Random.Range(0, 3);
        Instantiate(Sections[segmentNum], new Vector3(-6.999076f, -7.195025f, zPos), Quaternion.identity);
        zPos += 50;
        yield return new WaitForSeconds(10);
        isCreating = false;
    }
}
