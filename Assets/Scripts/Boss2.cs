using System.Collections;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Audio;

public class Boss2 : MonoBehaviour, INeonGrindListener
{
    #region References & Prefabs
    public Score finalScore;
    public PlayerMovement playerSpeed;
    public Transform player;
    public GameObject bossPrefab;
    public GameObject homingMissilePrefab;
    public Generator generator;
    public int defeatReward = 100;
    #endregion

    #region Boss Settings
    public float missileAttackInterval;
    public float forceMoveInterval = 10f;
    private float windUpDuration = 5f;

    private float distanceAhead = 13f;
    public float attackInterval = 2f;

    public float laneChangeInterval = 2f;
    private float laneTimer = 0f;
    private int currentLane;

    public float[] lanePositions = { -4.65f, 1.23f, 7.13f };
    public int bossSpawn = 20;
    #endregion

    #region States
    private float timeSinceLastSwitch = 0f;
    private float bossSpeed;
    private bool isSpawned = false;
    private GameObject activeBoss;
    private float scoreAtPrevBossDefeat = -1f;
    private bool waitingForRespawn = false;
    #endregion

    #region Audio
    [SerializeField] AudioClip bossSpawnIn;
    [SerializeField] AudioClip missileFire;
    [SerializeField] AudioClip switchAttack;
    private AudioSource missileSound;
    private AudioSource bossSound;
    private AudioSource switchSound;
    #endregion

    private void Awake()
    {
        bossSound = gameObject.AddComponent<AudioSource>();
        bossSound.playOnAwake = false;
        bossSound.clip = bossSpawnIn;

        missileSound = gameObject.AddComponent<AudioSource>();
        missileSound.playOnAwake = false;
        missileSound.clip = missileFire;

        switchSound = gameObject.AddComponent<AudioSource>();
        switchSound.playOnAwake = false;
        switchSound.clip = switchAttack;

        missileSound.volume = 0.3f;
        switchSound.volume = 0.3f;
        bossSound.volume = 0.3f;
    }
    void Start()
    {
        activeBoss = Instantiate(bossPrefab);
        activeBoss.SetActive(false);
        StartCoroutine(WaitForEventManager());
        generator = FindFirstObjectByType<Generator>();
    }

    // Waits until EventManager is ready, then registers this boss as an event listener
    IEnumerator WaitForEventManager()
    {
        while (EventManager.Instance == null)
            yield return null;

        EventManager.Instance.AddListener(NeonGrindEvents.BOSS_DEFEATED, this);
    }

    private void FixedUpdate()
    {
        // Only activates if it's boss cycle 2 and the boss is ready to respawn
        if (!isSpawned && waitingForRespawn && generator.bossCycle == 2)
        {
            float scoreSinceDefeat = finalScore.DistScore - scoreAtPrevBossDefeat;
            if (scoreSinceDefeat >= bossSpawn)
            {
                Activate();
                waitingForRespawn = false;
            }
        }
    }

    private void Update()
    {
        bossSpeed = playerSpeed.MovementSpeed;

        if (activeBoss != null)
        {
            laneTimer -= Time.deltaTime;

            // Periodically change lanes
            if (laneTimer <= 0f)
            {
                ChangeLane();
                laneTimer = laneChangeInterval;
            }

            // Boss smoothly follows ahead of the player
            Vector3 targetPos = new Vector3(
                lanePositions[currentLane],
                activeBoss.transform.position.y,
                player.position.z + distanceAhead
            );
            activeBoss.transform.position = Vector3.Lerp(
                activeBoss.transform.position,
                targetPos,
                Time.deltaTime * 5f
            );
        }
    }

    // Spawns the boss and starts its attack behavior
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

        isSpawned = true;
        bossSound.Play();
        StartCoroutine(AttackRoutine());
        EventManager.Instance?.PostNotification(NeonGrindEvents.BOSS_SPAWNED, this);
    }

    // Main attack loop, handles timed attacks: missile + force move
    IEnumerator AttackRoutine()
    {
        float timeSinceLastHoming = 0f;
        timeSinceLastSwitch = 0f;

        while (isSpawned && activeBoss != null && activeBoss.activeSelf)
        {
            yield return new WaitForSeconds(1f);

            if (!isSpawned || activeBoss == null || !activeBoss.activeSelf)
                yield break;

            timeSinceLastHoming += 1f;
            timeSinceLastSwitch += 1f;

            // Fire homing missile
            if (timeSinceLastHoming >= missileAttackInterval)
            {
                LaunchMissile();
                timeSinceLastHoming = 0f;
            }

            // Forcefully push player to a new lane after wind-up
            if (timeSinceLastSwitch >= forceMoveInterval)
            {
                StartCoroutine(ForceMovePlayerAttack());
                timeSinceLastSwitch = 0f;
            }
        }
    }

    // Adds delay before forcing the player to a new lane
    IEnumerator ForceMovePlayerAttack()
    {
        yield return new WaitForSeconds(windUpDuration);
        switchSound.Play();
        int newLaneIndex = Random.Range(0, lanePositions.Length);
        playerSpeed.ForceLaneMovement(newLaneIndex);
    }

    // Picks a new lane that isn't the current one
    void ChangeLane()
    {
        int newLaneIndex = Random.Range(0, lanePositions.Length);

        while (newLaneIndex == currentLane && lanePositions.Length > 1)
        {
            newLaneIndex = Random.Range(0, lanePositions.Length);
        }

        currentLane = newLaneIndex;
    }

    // Fires a homing projectile at the player
    void LaunchMissile()
    {
        missileSound.Play();
        Vector3 spawnPos = new Vector3(activeBoss.transform.position.x, 1f, activeBoss.transform.position.z);
        GameObject missile = Instantiate(homingMissilePrefab, spawnPos, Quaternion.identity);
        missile.GetComponent<HomingAttack>().target = player;
    }

    // Handles BOSS_DEFEATED event: marks boss as defeated and sets up respawn tracking
    public void OnEvent(NeonGrindEvents eventType, Component sender, object param = null)
    {
        if (eventType == NeonGrindEvents.BOSS_DEFEATED && generator.bossCycle == 2)
        {
            isSpawned = false;
            waitingForRespawn = true;
            finalScore.BossReward(defeatReward);
            scoreAtPrevBossDefeat = finalScore.DistScore;
        }
    }
}