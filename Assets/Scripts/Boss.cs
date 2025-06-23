using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class Boss : MonoBehaviour, INeonGrindListener
{
    public static Boss Instance { get; private set; }

    #region References
    public Score finalScore;
    public PlayerMovement playerSpeed;
    public Transform player;
    public Generator generator;
    public int defeatReward = 100;
    #endregion

    #region Boss Prefabs
    public GameObject bossPrefab;
    public GameObject laneAttackPrefab;
    public GameObject waveAttackPrefab;
    public GameObject homingMissilePrefab;
    #endregion

    #region Boss Settings
    public float distanceAhead = 13f;
    public float attackInterval = 2f;
    public float laneAttackInterval = 3f;
    public float waveAttackInterval = 5f;
    public float missileAttackInterval = 7f;
    public float laneChangeInterval = 2f;
    public float[] lanePositions = { -4.65f, 1.23f, 7.13f };
    public int bossSpawn;
    #endregion

    #region States
    private GameObject activeBoss;
    private float laneTimer = 0f;
    private int currentLane = 1;
    private float bossSpeed;
    private bool isSpawned = false;
    private bool waitingForRespawn = false;
    private float scoreAtPrevBossDefeat = -1f;
    #endregion

    #region Audio
    [SerializeField] AudioClip bossSpawnIn;
    [SerializeField] AudioClip missileFire;
    [SerializeField] AudioClip waveAttack;
    private AudioSource missileSound;
    private AudioSource bossSound;
    private AudioSource waveSound;
    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("[Boss] Instance set on: " + gameObject.name);

        bossSound = gameObject.AddComponent<AudioSource>();
        bossSound.clip = bossSpawnIn;

        missileSound = gameObject.AddComponent<AudioSource>();
        missileSound.clip = missileFire;

        waveSound = gameObject.AddComponent<AudioSource>();
        waveSound.clip = waveAttack;

        bossSound.volume = missileSound.volume = waveSound.volume = 0.3f;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetBoss();
    }

    private void Start()
    {
        StartCoroutine(WaitForEventManager());
        generator = FindFirstObjectByType<Generator>();
    }

    IEnumerator WaitForEventManager()
    {
        while (EventManager.Instance == null)
            yield return null;

        EventManager.Instance.AddListener(NeonGrindEvents.BOSS_DEFEATED, this);
    }

    private void FixedUpdate()
    {
        if (!isSpawned && generator != null && generator.bossCycle == 1)
        {
            if (!waitingForRespawn && finalScore.DistScore >= bossSpawn)
            {
                if (activeBoss != null)
                {
                    Destroy(activeBoss);
                    activeBoss = null;
                }
                Activate();
            }
            else if (waitingForRespawn && finalScore.DistScore - scoreAtPrevBossDefeat >= bossSpawn)
            {
                Activate();
            }
        }
    }

    private void Update()
    {
        if (activeBoss == null) return;

        bossSpeed = playerSpeed.MovementSpeed;
        laneTimer -= Time.deltaTime;

        if (laneTimer <= 0f)
        {
            ChangeLane();
            laneTimer = laneChangeInterval;
        }

        Vector3 targetPos = new Vector3(lanePositions[currentLane], activeBoss.transform.position.y, player.position.z + distanceAhead);
        activeBoss.transform.position = Vector3.Lerp(activeBoss.transform.position, targetPos, Time.deltaTime * 5f);
    }

    public void Activate()
    {
        if (activeBoss == null)
        {
            Vector3 spawnPos = new Vector3(1.23f, 4.45f, player.position.z + distanceAhead);
            activeBoss = Instantiate(bossPrefab, spawnPos, Quaternion.identity);
        }
        else
        {
            activeBoss.transform.position = new Vector3(1.23f, 4.45f, player.position.z + distanceAhead);
            activeBoss.SetActive(true);
        }

        var bossHealth = activeBoss.GetComponent<BossHealth>();
        if (bossHealth != null)
        {
            bossHealth.currentHealth = bossHealth.maxHealth;
            bossHealth.enabled = true;
        }

        bossSound.Play();
        isSpawned = true;
        StartCoroutine(AttackRoutine());
        EventManager.Instance?.PostNotification(NeonGrindEvents.BOSS_SPAWNED, this);
    }

    IEnumerator AttackRoutine()
    {
        float timeSinceLastHoming = 0f;
        float timeSinceLastJumpAttack = 0f;
        float timeSinceLastLane = 0f;

        while (isSpawned && activeBoss != null && activeBoss.activeSelf)
        {
            yield return new WaitForSeconds(attackInterval);

            if (!isSpawned || activeBoss == null || !activeBoss.activeSelf)
                yield break;

            timeSinceLastLane += attackInterval;
            if (timeSinceLastLane >= laneAttackInterval)
            {
                waveSound.Play();
                Vector3 spawnPos = new Vector3(lanePositions[currentLane], 1f, activeBoss.transform.position.z);
                Instantiate(laneAttackPrefab, spawnPos, Quaternion.identity);
                timeSinceLastLane = 0f;
            }

            timeSinceLastJumpAttack += attackInterval;
            if (timeSinceLastJumpAttack >= waveAttackInterval)
            {
                waveSound.Play();
                Vector3 waveSpawnPos = new Vector3(lanePositions[1], 1f, activeBoss.transform.position.z);
                Instantiate(waveAttackPrefab, waveSpawnPos, Quaternion.identity);
                timeSinceLastJumpAttack = 0f;
            }

            timeSinceLastHoming += attackInterval;
            if (timeSinceLastHoming >= missileAttackInterval)
            {
                LaunchMissile();
                timeSinceLastHoming = 0f;
            }
        }
    }

    private void ChangeLane()
    {
        int newLaneIndex;
        do
        {
            newLaneIndex = Random.Range(0, lanePositions.Length);
        }
        while (newLaneIndex == currentLane && lanePositions.Length > 1);

        currentLane = newLaneIndex;
    }

    private void LaunchMissile()
    {
        missileSound.Play();
        Vector3 spawnPos = new Vector3(activeBoss.transform.position.x, 1f, activeBoss.transform.position.z);
        GameObject missile = Instantiate(homingMissilePrefab, spawnPos, Quaternion.identity);
        missile.GetComponent<HomingAttack>().target = player;
    }

    public void OnEvent(NeonGrindEvents eventType, Component sender, object param = null)
    {
        if (eventType == NeonGrindEvents.BOSS_DEFEATED && generator.bossCycle == 1)
        {
            isSpawned = false;
            waitingForRespawn = true;
            finalScore.BossReward(defeatReward);
            scoreAtPrevBossDefeat = finalScore.DistScore;
        }
    }

    public void ResetBoss()
    {
        StopAllCoroutines();
        isSpawned = false;
        waitingForRespawn = false;
        scoreAtPrevBossDefeat = -1f;
        laneTimer = 0f;
        currentLane = 1;

        player = GameObject.FindWithTag("Player")?.transform;
        generator = FindFirstObjectByType<Generator>();

        Activate();
    }
}