using System.Collections;
using UnityEngine;

public class Generator : MonoBehaviour
{
    #region Section fields
    public GameObject[] Sections = new GameObject[5];
    public float zPos;//stores the position of new spawn location
    public bool isCreating = false;
    public int sectionNum;
    public int prevSegment = -1;//stores the index of previous spawned section
    #endregion

    #region Obstacle Serialized fields 
    //GameObjects and transform for obstacle and player
    [SerializeField]
    private GameObject[] obstacleArray;
    [SerializeField]
    private Transform playerLocation;

    //variables to help spawning obstacles
    public float spawnDistance = 30f;
    public float spawnTime = 2f;

    public const float rightSpawnLimit = 7.13f;
    public const float leftSpawnLimit = -4.65f;
    public const float middle = 1.23f;
    #endregion

    private void Start()
    {
        Instantiate(Sections[0], new Vector3(-6.999076f, -7.195025f, -1f), Quaternion.identity);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        if (isCreating == false)
        {
            isCreating = true;
            StartCoroutine(Gen());
        }

        //timer for spawning based on frames.
        spawnTime -= Time.deltaTime;  

        if (spawnTime <= 0)
        {
            SpawnObstacle();

            //if (obstacleScore == 100)
            //{
            //    obstacleScore = 0;
            //    spawnTime = spawnTime - 0.1f;
            //}
            //else
            //{
            //    spawnTime = 2f;
            //}
            spawnTime = 2f;
        }
    }

    IEnumerator Gen()
    {
        sectionNum = Random.Range(0, Sections.Length);
        
        //ensures the same section doesn't spawn twice in a row
        if (sectionNum == prevSegment) 
        { 
            while (sectionNum == prevSegment)
                sectionNum = Random.Range(0, Sections.Length); 
        }

        Instantiate(Sections[sectionNum], new Vector3(-6.999076f, -7.195025f, zPos), Quaternion.identity);

        zPos += 43.99641f;
        yield return new WaitForSeconds(4);
        isCreating = false;

        prevSegment = sectionNum;
    }

    void SpawnObstacle()
    {
        int randomObs= Random.Range(0, obstacleArray.Length);
        GameObject obstacleToSpawn = obstacleArray[randomObs];

        Vector3 spawnPosition = playerLocation.position + playerLocation.forward * spawnDistance;

        int randomX = Random.Range(1,4);
        if (randomX == 1)
        { spawnPosition.x = leftSpawnLimit;  }

        if (randomX == 2)
        { spawnPosition.x = middle; }

        if (randomX == 3)
        { spawnPosition.x = rightSpawnLimit; }

        spawnPosition.y = 1f;

        Instantiate(obstacleToSpawn, spawnPosition, Quaternion.identity);
    }
}
