using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour, INeonGrindListener
{
    #region Section fields
    public GameObject[] Sections = new GameObject[5];
    public GameObject[] secondSections = new GameObject[5];
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
    [SerializeField] private float buffCooldown = 15f;
    #endregion

    private bool bossDefeated = false;
    private bool allowObstacleSpawn = true;
    public int bossCycle = 1;

    private List<GameObject> spawnedSections = new List<GameObject>();
    public float destructionDistance = 100f;
    private const float sectionLength = 43.99641f;

    private Coroutine genRoutine;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        StopAllCoroutines();
        zPos = 0f;
        isCreating = false;
        prevSegment = -1;
        prevObs = -1;
        spawnTime = 2f;

        bossDefeated = false;
        allowObstacleSpawn = true;
        bossCycle = 1;

        buffCooldowns[3] = 0f;
        buffCooldowns[4] = 0f;
        buffCooldowns[5] = 0f;

        foreach (var section in spawnedSections)
        {
            if (section != null) Destroy(section);
        }
        spawnedSections.Clear();

        GameObject initialSection = Instantiate(Sections[0], new Vector3(-6.999076f, -7.195025f, 0f), Quaternion.identity);
        spawnedSections.Add(initialSection);

        zPos = sectionLength;

        StartCoroutine(WaitForEventManager());
        genRoutine = StartCoroutine(Gen());
    }

    public void ResetGenerator(Transform newPlayer)
    {
        playerLocation = newPlayer;
        player = newPlayer.gameObject;
        Initialize();
    }

    private IEnumerator WaitForEventManager()
    {
        while (EventManager.Instance == null)
            yield return null;

        EventManager.Instance.AddListener(NeonGrindEvents.BOSS_SPAWNED, this);
        EventManager.Instance.AddListener(NeonGrindEvents.BOSS_DEFEATED, this);
    }

    private void OnDestroy()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.RemoveListener(NeonGrindEvents.BOSS_SPAWNED, this);
            EventManager.Instance.RemoveListener(NeonGrindEvents.BOSS_DEFEATED, this);
        }
    }

    private void Update()
    {
        if (playerLocation == null)
            return;

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

    private IEnumerator Gen()
    {
        GameObject[] currentSectionArray = bossDefeated ? secondSections : Sections;
        sectionNum = Random.Range(0, currentSectionArray.Length);

        while (sectionNum == prevSegment)
            sectionNum = Random.Range(0, currentSectionArray.Length);

        GameObject newSection = Instantiate(currentSectionArray[sectionNum], new Vector3(-6.999076f, -7.195025f, zPos), Quaternion.identity);
        spawnedSections.Add(newSection);

        zPos += sectionLength;

        yield return new WaitForSeconds(0.5f);
        isCreating = false;

        prevSegment = sectionNum;
    }

    private void DestroyOldSections()
    {
        if (playerLocation == null)
            return;

        List<GameObject> toRemove = new List<GameObject>();

        foreach (var section in spawnedSections)
        {
            if (section == null) continue;

            float sectionEndZ = section.transform.position.z + sectionLength;

            if (playerLocation.position.z - sectionEndZ > destructionDistance)
            {
                Destroy(section);
                toRemove.Add(section);
            }
        }

        foreach (var s in toRemove)
        {
            spawnedSections.Remove(s);
        }
    }

    private void SpawnObstacle()
    {
        if (!allowObstacleSpawn || playerLocation == null)
            return;

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

            if (!Physics.CheckSphere(position, 1f, obstacleLayer))
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
        else if (eventType == NeonGrindEvents.BOSS_DEFEATED)
        {
            SwitchObstacleArray();
        }
    }
}