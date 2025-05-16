using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    /*public Transform player;
    public GameObject laneAttack;

    public float distanceAhead = 25f; // How far ahead of the player the boss should float
    public float attackInterval = 2f; // Time between each attack
    public float survivalTime = 30f;

    public float[] lanePositions = { -4.65f, 1.23f, 7.13f };

    private bool isSpawned = false;
    private float timer;

    public Death finalScore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = survivalTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSpawned) return;

        if (finalScore == 1000)
        {
            Activate();
        }

        Vector3 targetPos = new Vector3(0, 0, player.position.z + distanceAhead);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 5f);

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Die();
        }

    }

    public void Activate()
    {
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

    private void Die()
    {
        Debug.Log("Boss defeated!");
        isSpawned = false;
        Destroy(gameObject); // Remove the boss from the scene
    }*/

    /*public float[] xVelocity;
    private int arrayPos = 0;

    public Transform laser;
    public Transform mine;

    private int timer = 30;

    IEnumerator bossMovement()
    {
        yield return new WaitForSeconds(2);
        Instantiate(mine, new Vector3(transform.position.x, mine.position.y, 5f), mine.rotation);
        GetComponent<Rigidbody>().linearVelocity = new Vector3(xVelocity[arrayPos], 0, 0);
        arrayPos++;
        if (arrayPos > 13)
            arrayPos = 0;
        yield return new WaitForSeconds(1);
        GetComponent<Rigidbody>().linearVelocity = new Vector3(0, 0, 0);
        Instantiate(laser, new Vector3(transform.position.x, laser.position.y, transform.position.z), laser.rotation);
        yield return new WaitForSeconds(.1f);
        Instantiate(laser, new Vector3(transform.position.x, laser.position.y, transform.position.z), laser.rotation);
        StartCoroutine(bossMovement());
    }*/

}
