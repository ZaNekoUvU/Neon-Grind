using System.Collections;
using TMPro;
using UnityEngine;
using static Events;

public class Score : MonoBehaviour
{
    [SerializeField] public TMP_Text scoreDisplay;
    private int score;
    public const int passScore = 10;
    public bool isAddingScore = false;
    public int DistScore => score;

    void Update()
    {
        if (!isAddingScore)
        {
            isAddingScore = true;
            StartCoroutine(AddingScore());
        }
    }

    IEnumerator AddingScore()
    {
        score++;
        scoreDisplay.text = score.ToString();
        GameEvents.OnScoreChanged?.Invoke(score); 
        yield return new WaitForSeconds(0.1f);
        isAddingScore = false;
    }

    public void OnTriggerEnter(Collider pass)
    {
        if (pass.CompareTag("Obstacle"))
        {
            score += passScore;
            scoreDisplay.text = score.ToString();
            GameEvents.OnScoreChanged?.Invoke(score); 
        }
    }
}