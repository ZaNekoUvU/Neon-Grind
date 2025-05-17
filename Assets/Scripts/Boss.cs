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
    public GameObject laneAttack;

    private float distanceAhead = 13f; // How far ahead of the player the boss should float
    public float attackInterval = 2f; // Time between each attack

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

        if (finalScore.DistScore > 200 && !isSpawned)
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
        StartCoroutine(AttackRoutine()); // Start the attack loop
    }

    IEnumerator AttackRoutine()
    {
        while (isSpawned)
        {
            yield return new WaitForSeconds(attackInterval); // Wait between attacks

            // Choose a random lane index
            int laneIndex = Random.Range(0, lanePositions.Length);

            // Calculate spawn position in the chosen lane
            Vector3 attackPos = new Vector3(lanePositions[laneIndex], 1f, transform.position.z);

            // Instantiate the lane attack prefab in that lane
            Instantiate(laneAttack, attackPos, Quaternion.identity);
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
}
