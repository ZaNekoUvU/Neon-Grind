using System.Collections;
using UnityEngine;
using Unity.VisualScripting;

public class Boss : MonoBehaviour, INeonGrindListener
{
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
    private int currentLane;
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
        bossSound = gameObject.AddComponent<AudioSource>();
        bossSound.playOnAwake = false;
        bossSound.clip = bossSpawnIn;

        missileSound = gameObject.AddComponent<AudioSource>();
        missileSound.playOnAwake = false;
        missileSound.clip = missileFire;

        waveSound = gameObject.AddComponent<AudioSource>();
        waveSound.playOnAwake = false;
        waveSound.clip = waveAttack;

        missileSound.volume = 0.3f;
        waveSound.volume = 0.3f;
        bossSound.volume = 0.3f;
    }
    private void Start()
    {
        activeBoss = Instantiate(bossPrefab);
        activeBoss.SetActive(false);
        StartCoroutine(WaitForEventManager());
        generator = FindFirstObjectByType<Generator>();
    }

    // Waits for EventManager to be initialized before subscribing
    IEnumerator WaitForEventManager()
    {
        while (EventManager.Instance == null)
            yield return null;

        EventManager.Instance.AddListener(NeonGrindEvents.BOSS_DEFEATED, this);
    }

    private void FixedUpdate()
    {
        // Check if boss should spawn or respawn based on current cycle and score
        
    }

    private void Update()
    {
        if (!isSpawned && generator.bossCycle == 1)
        {
            if (!waitingForRespawn && finalScore.DistScore >= bossSpawn)
            {
                Activate();
            }
            else if (waitingForRespawn && finalScore.DistScore - scoreAtPrevBossDefeat >= bossSpawn)
            {
                Activate();
            }
        }
        if (activeBoss == null) return;

        bossSpeed = playerSpeed.MovementSpeed;
        laneTimer -= Time.deltaTime;

        // Change lanes periodically
        if (laneTimer <= 0f)
        {
            ChangeLane();
            laneTimer = laneChangeInterval;
        }

        // Smoothly follow the player at a fixed distance
        Vector3 targetPos = new Vector3(lanePositions[currentLane], activeBoss.transform.position.y, player.position.z + distanceAhead);
        activeBoss.transform.position = Vector3.Lerp(activeBoss.transform.position, targetPos, Time.deltaTime * 5f);
    }

    // Spawns the boss and starts the attack coroutine
    public void Activate()
    {
        if (activeBoss == null)
        {
            Vector3 spawnPos = new Vector3(1.23f, 4.45f, player.position.z + distanceAhead);
            activeBoss = Instantiate(bossPrefab, spawnPos, Quaternion.identity);
        }
        else
        {
            // Reset position before enabling
            activeBoss.transform.position = new Vector3(1.23f, 4.45f, player.position.z + distanceAhead);
            activeBoss.SetActive(true);
        }

        // Reset boss logic if needed
        var bossHealth = activeBoss.GetComponent<BossHealth>();
        if (bossHealth != null)
        {
            bossHealth.currentHealth = bossHealth.maxHealth;
            bossHealth.enabled = true; // Reactivate boss health script
        }
        bossSound.Play();
        isSpawned = true;
        StartCoroutine(AttackRoutine());
        EventManager.Instance?.PostNotification(NeonGrindEvents.BOSS_SPAWNED, this);
    }

    // Main boss attack loop — handles all timed attacks
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

            // Lane attack
            timeSinceLastLane += attackInterval;
            if (timeSinceLastLane >= laneAttackInterval)
            {
                waveSound.Play();
                Vector3 spawnPos = new Vector3(lanePositions[currentLane], 1f, activeBoss.transform.position.z);
                Instantiate(laneAttackPrefab, spawnPos, Quaternion.identity);
                timeSinceLastLane = 0f;
            }

            // Wave attack
            timeSinceLastJumpAttack += attackInterval;
            if (timeSinceLastJumpAttack >= waveAttackInterval)
            {
                waveSound.Play();
                Vector3 waveSpawnPos = new Vector3(lanePositions[1], 1f, activeBoss.transform.position.z);
                Instantiate(waveAttackPrefab, waveSpawnPos, Quaternion.identity);
                timeSinceLastJumpAttack = 0f;
            }

            // Homing missile
            timeSinceLastHoming += attackInterval;
            if (timeSinceLastHoming >= missileAttackInterval)
            {
                LaunchMissile();
                timeSinceLastHoming = 0f;
            }
        }
    }

    // Picks a new random lane different from the current one
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

    // Instantiates a homing missile targeting the player
    private void LaunchMissile()
    {
        missileSound.Play();
        Vector3 spawnPos = new Vector3(activeBoss.transform.position.x, 1f, activeBoss.transform.position.z);
        GameObject missile = Instantiate(homingMissilePrefab, spawnPos, Quaternion.identity);
        missile.GetComponent<HomingAttack>().target = player;
    }

    // Reacts to boss defeat and enables respawn logic
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
}


