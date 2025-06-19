using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static event Action OnBossSpawn;

    public Score score;
    public int bossSpawnThreshold = 200;
    private bool bossSpawned = false;

    void Update()
    {
        if (!bossSpawned && score.DistScore >= bossSpawnThreshold)
        {
            bossSpawned = true;
            OnBossSpawn?.Invoke();
        }
    }
}