using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour, INeonGrindListener
{
    public Score finalScore;
    public PlayerMovement playerSpeed;
    private float bossSpeed;

    public Transform player;
    public GameObject bossPrefab;
    public GameObject laneAttackPrefab;
    public float laneAttackInterval;
    public GameObject waveAttackPrefab; 
    public float waveAttackInterval;
    public GameObject homingMissilePrefab;
    public float missileAttackInterval;

    private float distanceAhead = 13f; 
    public float attackInterval = 2f; 

    public float laneChangeInterval = 2f;
    private float laneTimer = 0;
    private int currentLane;

    public float[] lanePositions = { -4.65f, 1.23f, 7.13f };

    private bool isSpawned = false;
    private GameObject activeBoss;

    public GameObject bossBar;

    public int bossSpawn;

    private Generator generator;

    private bool hasSpawnedInitially = false;
    private bool secondBossDefeated = false;
    private float scoreAtPrevBossDefeat = -1f;
    private int firstInstance = 1;

    private bool firstBossHasSpawnedOnce = false;
    private bool waitingForBossRespawn = false;

    private bool waitingForRespawn = false;
    void Start()
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
        if (!isSpawned && generator.bossCycle == 1)
        {
            if (!waitingForRespawn && finalScore.DistScore >= bossSpawn)
            {
                Activate();
                waitingForRespawn = false;
            }
            else if (waitingForRespawn)
            {
                float scoreSinceDefeat = finalScore.DistScore - scoreAtPrevBossDefeat;
                if (scoreSinceDefeat >= bossSpawn)
                {
                    Activate();
                    waitingForRespawn = false;
                }
            }
        }
    }
    void Update()
    {
        bossSpeed = playerSpeed.MovementSpeed;

        if (activeBoss != null)
        {
            float target = player.position.z + distanceAhead;

            laneTimer -= Time.deltaTime;
            if (laneTimer <= 0f)
            {
                ChangeLane();
                laneTimer = laneChangeInterval;
            }

            Vector3 targetPos = new Vector3(lanePositions[currentLane], activeBoss.transform.position.y, player.position.z + distanceAhead);
            activeBoss.transform.position = Vector3.Lerp(activeBoss.transform.position, targetPos, Time.deltaTime * 5f);
        }
    } 
     
    public void Activate()
    {
        Vector3 spawnPos = new Vector3(1.23f, 4.45f, player.position.z + distanceAhead);
        activeBoss = Instantiate(bossPrefab, spawnPos, Quaternion.identity);
        isSpawned = true;
        StartCoroutine(AttackRoutine()); 
        bossBar.SetActive (true);
        EventManager.Instance?.PostNotification(NeonGrindEvents.BOSS_SPAWNED, this);
    }

    IEnumerator AttackRoutine()
    {
        float timeSinceLastHoming = 0f;
        float timeSinceLastJumpAttack = 0f;
        float timeSinceLastLane = 0f;

        while (isSpawned)
        {
            yield return new WaitForSeconds(attackInterval);

            //Regular lane attack
            int laneIndex = currentLane;
            timeSinceLastLane += attackInterval;
            if (timeSinceLastLane >= laneAttackInterval)
            {
                Vector3 spawnPos = new Vector3(lanePositions[laneIndex], 1f, activeBoss.transform.position.z);
                Instantiate(laneAttackPrefab, spawnPos, Quaternion.identity);
                timeSinceLastLane = 0;
            }

            //Jump attack every few seconds
            timeSinceLastJumpAttack += attackInterval;
            if (timeSinceLastJumpAttack >= waveAttackInterval)
            {
                Vector3 waveSpawnPos = new Vector3(lanePositions[1], 1f, activeBoss.transform.position.z);
                Instantiate(waveAttackPrefab, waveSpawnPos, waveAttackPrefab.transform.rotation);
                timeSinceLastJumpAttack = 0f;
            }
            
            //Homing projectile
            timeSinceLastHoming += attackInterval;
            if (timeSinceLastHoming >= missileAttackInterval)
            {
                LaunchMissile();
                timeSinceLastHoming = 0f;
            }
        }
    }

    void ChangeLane()
    {
        int newLaneIndex = Random.Range(0, lanePositions.Length);

        while (newLaneIndex == currentLane && lanePositions.Length > 1)
        {
            newLaneIndex = Random.Range(0, lanePositions.Length);
        }

        currentLane = newLaneIndex;
    }

    void LaunchMissile()
    {
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
            scoreAtPrevBossDefeat = finalScore.DistScore;
        }
    }
}
