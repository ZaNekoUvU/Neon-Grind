using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss2 : MonoBehaviour, INeonGrindListener
{
    public Score finalScore;
    public PlayerMovement playerSpeed;
    private float bossSpeed;

    public Transform player;
    public GameObject bossPrefab;

    public GameObject homingMissilePrefab;
    public float missileAttackInterval;

    public float forceMoveInterval = 10f;
    private float timeSinceLastSwitch = 0f;
    private float windUpDuration = 5f;

    private float distanceAhead = 13f;
    public float attackInterval = 2f;

    public float laneChangeInterval = 2f;
    private float laneTimer = 0;
    private int currentLane;

    public float[] lanePositions = { -4.65f, 1.23f, 7.13f };

    private bool isSpawned = false;
    private GameObject activeBoss;

    public GameObject bossBar;

    private bool firstBossDefeated = false;
    private float scoreAtPrevBossDefeat = -1f;
    public int bossSpawn = 20;

    private Generator generator;

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
        if (firstBossDefeated && !isSpawned && generator.bossCycle == 2)
        {
            float scoreSinceDefeat = finalScore.DistScore - scoreAtPrevBossDefeat;
            if (scoreSinceDefeat >= bossSpawn)
            {
                Activate();
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
        bossBar.SetActive(true);
    }

    IEnumerator AttackRoutine()
    {
        float timeSinceLastHoming = 0f;
        timeSinceLastSwitch = 0f;

        while (isSpawned)
        {
            yield return new WaitForSeconds(1f);

            timeSinceLastHoming += 1f;
            timeSinceLastSwitch += 1f;

            if (timeSinceLastHoming >= missileAttackInterval)
            {
                LaunchMissile();
                timeSinceLastHoming = 0f;
            }

            if (timeSinceLastSwitch >= forceMoveInterval)
            {
                StartCoroutine(ForceMovePlayerAttack());
                timeSinceLastSwitch = 0f;
            }
        }
    }

    IEnumerator ForceMovePlayerAttack()
    {
        yield return new WaitForSeconds(windUpDuration);

        int newLaneIndex = Random.Range(0, lanePositions.Length);
        playerSpeed.ForceLaneMovement(newLaneIndex);
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
        if (eventType == NeonGrindEvents.BOSS_DEFEATED)
        {
            firstBossDefeated = true;
            scoreAtPrevBossDefeat = finalScore.DistScore;

            if (generator.bossCycle == 2)
            {
                isSpawned = false;
            }
        }
    }
}
