using UnityEngine;

public class ScoreIncreaser : MonoBehaviour
{
    private Score scoreManager;

    private void Start()
    {
        scoreManager = Object.FindAnyObjectByType<Score>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            scoreManager.AddObstacleScore();
            EventManager.Instance?.PostNotification(NeonGrindEvents.OBSTACLE_PASSED, this);
        }
    }
}
