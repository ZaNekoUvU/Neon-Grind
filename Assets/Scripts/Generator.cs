using System.Collections;
using System.Collections.Generic;
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

    [SerializeField]
    private LayerMask obstacleLayer;

    //variables to help spawning obstacles
    public float spawnDistance = 30f;
    public float spawnTime = 2f;

    //lane positions
    public const float rightSpawnLimit = 7.13f;
    public const float leftSpawnLimit = -4.65f;
    public const float middle = 1.23f;
    #endregion

    //dictionary to store cooldowns
    private Dictionary<int, float> buffCooldowns = new Dictionary<int, float>();
    [SerializeField] float buffCooldown = 15f;

    private void Start()
    {
        //spawns the first section
        Instantiate(Sections[0], new Vector3(-6.999076f, -7.195025f, -1f), Quaternion.identity);

        //initizalize buff cooldowns to zero
        buffCooldowns[3] = 0f;
        buffCooldowns[4] = 0f;
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
            spawnTime = Random.Range(0.5f, 2f);
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
        yield return new WaitForSeconds(0.5f);
        isCreating = false;

        prevSegment = sectionNum;
    }

    void SpawnObstacle()
    {
        Vector3 spawnPosition = playerLocation.position + playerLocation.forward * spawnDistance;
        float[] lanes = { leftSpawnLimit, middle, rightSpawnLimit };

        int numLanesToSpawn = Random.Range(1, 3);
        List<int> chosenIndices = new List<int>();

        while (chosenIndices.Count < numLanesToSpawn)
        {
            int randIndex = Random.Range(0, lanes.Length);
            if (!chosenIndices.Contains(randIndex))
            {
                chosenIndices.Add(randIndex);
            }
        }

        foreach (int index in chosenIndices)
        {
            int randomObs;
            int attempts = 0;
            const int maxAttempts = 10;

            do
            {
                randomObs = Random.Range(0, obstacleArray.Length);
                attempts++;

                if (attempts > maxAttempts) break;
            } while ((randomObs == 3 || randomObs == 4) && Time.time < buffCooldowns[randomObs]);

            if ((randomObs == 3 || randomObs == 4) && Time.time < buffCooldowns[randomObs])
                continue;
            GameObject obstacleToSpawn = obstacleArray[randomObs];

            Vector3 position = spawnPosition;
            position.x = lanes[index];
            position.y = 1f;

            float checkRadius = 1f;

            if (!Physics.CheckSphere(position, checkRadius, obstacleLayer))
            {
                Instantiate(obstacleToSpawn, position, Quaternion.identity);

                if (randomObs == 3 || randomObs == 4)
                {
                    buffCooldowns[randomObs] = Time.time + buffCooldown;
                }
            }
        }
    }

}