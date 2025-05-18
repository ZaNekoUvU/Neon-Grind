using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Score finalScore;
    public PlayerMovement playerSpeed;
    private float bossSpeed;

    public Transform player;
    public GameObject bossPrefab;
    public GameObject laneAttackPrefab;
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

    void Start()
    {
        bossBar.SetActive(false);
    }

    void Update()
    {
        bossSpeed = playerSpeed.MovementSpeed;

        if (finalScore.DistScore > 1000 && !isSpawned)
        {
            Activate();
        }

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
    }

    IEnumerator AttackRoutine()
    {
        float timeSinceLastHoming = 0f;
        float timeSinceLastJumpAttack = 0f;

        while (isSpawned)
        {
            yield return new WaitForSeconds(attackInterval);

            //Regular lane attack
            int laneIndex = currentLane;
            Vector3 spawnPos = new Vector3(lanePositions[laneIndex], 1f, activeBoss.transform.position.z);
            Instantiate(laneAttackPrefab, spawnPos, Quaternion.identity);

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
}
