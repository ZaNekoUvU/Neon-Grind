using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    #region Section fields
    public GameObject[] Sections = new GameObject[5];
    public float zPos;
    public bool isCreating = false;
    public int sectionNum;
    public int prevSegment = -1;
    #endregion

    #region Obstacle fields 
    [SerializeField]
    private GameObject[] obstacleArray;
    [SerializeField]
    private Transform playerLocation;
    [SerializeField]
    private LayerMask obstacleLayer;
    
    public float spawnDistance = 30f;
    public float spawnTime = 2f;

    public const float rightSpawnLimit = 7.13f;
    public const float leftSpawnLimit = -4.65f;
    public const float middle = 1.23f;

    private int prevObs;
    #endregion

    #region Buff management
    //Dictionary to store cooldown timers 
    private Dictionary<int, float> buffCooldowns = new Dictionary<int, float>();
    [SerializeField]
    float buffCooldown = 15f;
    #endregion

    private void Start()
    {
        //Spawn the initial section at a fixed start position
        Instantiate(Sections[0], new Vector3(-6.999076f, -7.195025f, -1f), Quaternion.identity);

        //Initialize buff obstacle cooldowns to 0
        buffCooldowns[3] = 0f;
        buffCooldowns[4] = 0f;
        buffCooldowns[5] = 0f;
    }

    void Update()
    {
        if (!isCreating && playerLocation.position.z + 50f > zPos)
        {
            isCreating = true;
            StartCoroutine(Gen());
        }

        spawnTime -= Time.deltaTime;

        if (spawnTime <= 0f)
        {
            SpawnObstacle();
            spawnTime = Random.Range(0.5f, 2f);
        }
    }

    //Coroutine to generate new map sections
    IEnumerator Gen()
    {
        sectionNum = Random.Range(0, Sections.Length);

        //Ensure new section isn't the same as the previous one
        while (sectionNum == prevSegment)
            sectionNum = Random.Range(0, Sections.Length);

        Instantiate(Sections[sectionNum], new Vector3(-6.999076f, -7.195025f, zPos), Quaternion.identity);

        zPos += 43.99641f;

        yield return new WaitForSeconds(0.5f);
        isCreating = false;

        prevSegment = sectionNum;
    }

    //Spawns obstacles in one or two random lanes
    void SpawnObstacle()
    {
        //Determine base position ahead of the player
        Vector3 spawnPosition = playerLocation.position + playerLocation.forward * spawnDistance;

        float[] lanes = { leftSpawnLimit, middle, rightSpawnLimit };

        //Decide randomly whether to spawn in 1 or 2 lanes
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

        //Try to spawn obstacles in selected lanes
        foreach (int index in chosenIndices)
        {
            int randomObs;
            int attempts = 0;
            const int maxAttempts = 10;

            do
            {
                randomObs = Random.Range(0, obstacleArray.Length);
                attempts++;

                if (randomObs == 1 && prevObs == 1)
                    continue;

                if (attempts > maxAttempts) break;

            } while ((randomObs == 3 || randomObs == 4 || randomObs == 5) && Time.time < buffCooldowns[randomObs]);

            if ((randomObs == 3 || randomObs == 4 || randomObs == 5) && Time.time < buffCooldowns[randomObs])
                continue;

            GameObject obstacleToSpawn = obstacleArray[randomObs];
            Vector3 position = spawnPosition;
            position.x = lanes[index];
            position.y = 1f;

            float checkRadius = 1f;

            if (!Physics.CheckSphere(position, checkRadius, obstacleLayer))
            {
                Instantiate(obstacleToSpawn, position, Quaternion.identity);

                prevObs = randomObs;

                if (randomObs == 3 || randomObs == 4 || randomObs == 5)
                {
                    buffCooldowns[randomObs] = Time.time + buffCooldown;
                }
            }
        }
    }
}
