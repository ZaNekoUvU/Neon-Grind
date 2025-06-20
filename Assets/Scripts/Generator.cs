using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Events;

public class Generator : MonoBehaviour
{
    #region Section Fields
    public GameObject[] Sections = new GameObject[5];
    public float zPos;
    public bool isCreating = false;
    public int sectionNum;
    public int prevSegment = -1;
    #endregion

    #region Obstacle Fields
    [SerializeField] private GameObject[] obstacleArray;
    [SerializeField] private GameObject[] secondObstacleArray;
    [SerializeField] private Transform playerLocation;
    [SerializeField] private LayerMask obstacleLayer;

    private bool usingSecondPool = false;

    public float spawnDistance = 30f;
    public float spawnTime = 2f;

    public const float rightSpawnLimit = 7.13f;
    public const float leftSpawnLimit = -4.65f;
    public const float middle = 1.23f;

    private int prevObs;
    #endregion

    #region Buff Management
    private Dictionary<int, float> buffCooldowns = new Dictionary<int, float>();
    [SerializeField] float buffCooldown = 15f;
    #endregion

    private void OnEnable()
    {
        GameEvents.OnFirstBossDefeated += SwitchToSecondPool;
    }

    private void OnDisable()
    {
        GameEvents.OnFirstBossDefeated -= SwitchToSecondPool;
    }

    private void SwitchToSecondPool()
    {
        usingSecondPool = true;
        Debug.Log("Switched to second obstacle pool");
    }

    private void Start()
    {
        Instantiate(Sections[0], new Vector3(-6.999076f, -7.195025f, -1f), Quaternion.identity);
        buffCooldowns[3] = 0f;
        buffCooldowns[4] = 0f;
        buffCooldowns[5] = 0f;
    }

    void Update()
    {
        if (!isCreating && playerLocation.position.z + 130f > zPos)
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

    IEnumerator Gen()
    {
        sectionNum = Random.Range(0, Sections.Length);
        while (sectionNum == prevSegment)
            sectionNum = Random.Range(0, Sections.Length);

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

        GameObject[] poolToUse = usingSecondPool ? secondObstacleArray : obstacleArray;

        foreach (int index in chosenIndices)
        {
            int randomObs;
            int attempts = 0;
            const int maxAttempts = 10;

            do
            {
                randomObs = Random.Range(0, poolToUse.Length);
                attempts++;
                if (randomObs == 1 && prevObs == 1) continue;
                if (attempts > maxAttempts) break;
            }
            while ((randomObs == 3 || randomObs == 4 || randomObs == 5) && Time.time < buffCooldowns[randomObs]);

            if ((randomObs == 3 || randomObs == 4 || randomObs == 5) && Time.time < buffCooldowns[randomObs])
                continue;

            GameObject obstacleToSpawn = poolToUse[randomObs];
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