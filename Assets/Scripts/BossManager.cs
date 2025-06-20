using UnityEngine;
using static Events;

public class BossManager : MonoBehaviour
{
    public GameObject firstBossPrefab;
    public GameObject secondBossPrefab;
    public Transform spawnPoint;

    public int firstBossThreshold = 1000;
    public int secondBossThreshold = 2000;

    private bool firstBossSpawned = false;
    private bool secondBossSpawned = false;

    private void OnEnable()
    {
        GameEvents.OnScoreChanged += HandleScore;
    }

    private void OnDisable()
    {
        GameEvents.OnScoreChanged -= HandleScore;
    }

    void HandleScore(int score)
    {
        if (!firstBossSpawned && score >= firstBossThreshold)
        {
            SpawnFirstBoss();
        }
        else if (!secondBossSpawned && score >= secondBossThreshold)
        {
            SpawnSecondBoss();
        }
    }

    void SpawnFirstBoss()
    {
        Instantiate(firstBossPrefab, spawnPoint.position, Quaternion.identity);
        firstBossSpawned = true;
        Debug.Log("First Boss Spawned!");
    }

    void SpawnSecondBoss()
    {
        Instantiate(secondBossPrefab, spawnPoint.position, Quaternion.identity);
        secondBossSpawned = true;
        GameEvents.OnSecondBossTriggered?.Invoke();
        Debug.Log("Second Boss Spawned!");
    }
}
