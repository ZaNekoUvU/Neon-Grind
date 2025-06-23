using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour, INeonGrindListener
{
    #region Section fields
    public GameObject[] Sections = new GameObject[5];
    public float zPos;
    public bool isCreating = false;
    public int sectionNum;
    public int prevSegment = -1;
    #endregion

    #region Obstacle fields 
    [SerializeField] private GameObject[] obstacleArray;
    [SerializeField] private GameObject[] secondObstacleArray;
    [SerializeField] private Transform playerLocation;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private GameObject player;

    public float spawnDistance = 30f;
    public float spawnTime = 2f;

    public const float rightSpawnLimit = 7.13f;
    public const float leftSpawnLimit = -4.65f;
    public const float middle = 1.23f;

    private int prevObs;
    #endregion

    #region Buff management
    private Dictionary<int, float> buffCooldowns = new Dictionary<int, float>();
    [SerializeField]
    float buffCooldown = 15f;
    #endregion

    private bool bossDefeated = false;
    private bool allowObstacleSpawn = true;
    public int bossCycle = 1;

    // List to track spawned sections for destruction
    private List<GameObject> spawnedSections = new List<GameObject>();

    // Distance behind the player where sections get destroyed
    public float destructionDistance = 100f;

    private void Start()
    {
        StartCoroutine(WaitForEventManager());

        // Spawn the initial section at a fixed start position and track it
        GameObject initialSection = Instantiate(Sections[0], new Vector3(-6.999076f, -7.195025f, -1f), Quaternion.identity);
        spawnedSections.Add(initialSection);

        // Initialize buff obstacle cooldowns
        buffCooldowns[3] = 0f;
        buffCooldowns[4] = 0f;
        buffCooldowns[5] = 0f;
    }

    IEnumerator WaitForEventManager()
    {
        while (EventManager.Instance == null)
            yield return null;

        EventManager.Instance.AddListener(NeonGrindEvents.BOSS_SPAWNED, this);
        EventManager.Instance.AddListener(NeonGrindEvents.BOSS_DEFEATED, this);
    }

    void Update()
    {
        // Generate new sections ahead of player when needed
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

        DestroyOldSections();
    }

    // Coroutine to generate new map sections
    IEnumerator Gen()
    {
        sectionNum = Random.Range(0, Sections.Length);

        // Ensure new section isn't the same as the previous one
        while (sectionNum == prevSegment)
            sectionNum = Random.Range(0, Sections.Length);

        // Instantiate and track the new section
        GameObject newSection = Instantiate(Sections[sectionNum], new Vector3(-6.999076f, -7.195025f, zPos), Quaternion.identity);
        spawnedSections.Add(newSection);

        zPos += 43.99641f;

        yield return new WaitForSeconds(0.5f);
        isCreating = false;

        prevSegment = sectionNum;
    }

    // Destroy sections behind player beyond destructionDistance
    void DestroyOldSections()
    {
        List<GameObject> sectionsToRemove = new List<GameObject>();

        foreach (var section in spawnedSections)
        {
            if (section == null) continue;

            // If section is far behind the player, mark for destruction
            if (playerLocation.position.z - section.transform.position.z > destructionDistance)
            {
                Destroy(section);
                sectionsToRemove.Add(section);
            }
        }

        // Remove destroyed sections from tracking list
        foreach (var s in sectionsToRemove)
        {
            spawnedSections.Remove(s);
        }
    }

    // Spawns obstacles in random lanes ahead of player
    void SpawnObstacle()
    {
        if (!allowObstacleSpawn) return;

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
                randomObs = Random.Range(0, bossDefeated ? secondObstacleArray.Length : obstacleArray.Length);
                attempts++;

                if (randomObs == 1 && prevObs == 1)
                    continue;

                if (attempts > maxAttempts) break;

            } while ((randomObs == 3 || randomObs == 4 || randomObs == 5) && Time.time < buffCooldowns[randomObs]);

            if ((randomObs == 3 || randomObs == 4 || randomObs == 5) && Time.time < buffCooldowns[randomObs])
                continue;

            GameObject obstacleToSpawn = (bossDefeated ? secondObstacleArray : obstacleArray)[randomObs];

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

    public void SwitchObstacleArray()
    {
        allowObstacleSpawn = true;

        if (bossCycle == 1)
        {
            bossDefeated = true;
            bossCycle = 2;
        }
        else
        {
            bossDefeated = false;
            bossCycle = 1;
        }
    }

    public void OnEvent(NeonGrindEvents eventType, Component sender, object param = null)
    {
        if (eventType == NeonGrindEvents.BOSS_SPAWNED)
        {
            allowObstacleSpawn = false;
        }

        if (eventType == NeonGrindEvents.BOSS_DEFEATED)
        {
            SwitchObstacleArray();
        }
    }
}