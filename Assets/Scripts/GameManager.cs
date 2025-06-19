using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static event Action OnBossSpawn;
    public static event Action OnSecondBossSpawn;

    public Score score;
    public int bossSpawnThreshold = 200;
    public int secondBossOffset = 100;
    private bool bossSpawned = false;
    private bool secondBossSpawned = false;
    private int boss1KillScore;

    public void FirstBossDefeated()
    {
        boss1KillScore = score.DistScore;
        Debug.Log("First Boss Defeated. Second will spawn at: " + (boss1KillScore + secondBossOffset));
    }

    void Update()
    {
        if (!bossSpawned && score.DistScore >= bossSpawnThreshold)
        {
            bossSpawned = true;
            Debug.Log("Triggering Boss 1");
            OnBossSpawn?.Invoke();
        }

        if (boss1KillScore > 0 && !secondBossSpawned && score.DistScore >= boss1KillScore + secondBossOffset)
        {
            secondBossSpawned = true;
            Debug.Log("Triggering Boss 2");
            OnSecondBossSpawn?.Invoke();
        }
    }
}