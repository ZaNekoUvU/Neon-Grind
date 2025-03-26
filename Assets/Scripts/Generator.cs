using System.Collections;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public GameObject[] Sections = new GameObject[3];

    public float zPos = 43.99641f;//stores the position of new spawn location
    public bool isCreating = false;
    public int sectionNum;
    public int prevSegment = -1;//stores the index of previous spawned section

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
        sectionNum = Random.Range(0, 3);
        
        //ensures the same section doesn't spawn twice in a row
        if (sectionNum == prevSegment) 
        { 
            while (sectionNum == prevSegment)
                sectionNum = Random.Range(0, 3); 
        }

        Instantiate(Sections[sectionNum], new Vector3(-6.999076f, -7.195025f, zPos), Quaternion.identity);
        zPos += 43.99641f;
        yield return new WaitForSeconds(4);
        isCreating = false;

        prevSegment = sectionNum;
    }
}
