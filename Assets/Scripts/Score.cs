using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class Score : MonoBehaviour
{
    [SerializeField]
    public TMP_Text scoreDisplay;
    private int score;
    public const int passScore = 10;
    public bool isAddingScore = false;
    public static int LastRunScore;
    public string playerId;

    public int DistScore { get { return score; } set { } }

    void Update()
    {
        if (isAddingScore == false)
        {
            isAddingScore = true;
            StartCoroutine(AddingScore());
        }
        scoreDisplay.text = score.ToString();
    }

    IEnumerator AddingScore()
    {
        score++;
        //scoreDisplay.text = "" + score;
        yield return new WaitForSeconds(0.1f);
        isAddingScore = false;
    }

    public void AddObstacleScore()
    {
        score += passScore;
    }

    public void SaveScore(string playerId)
    {
        string key = FirebaseInit.DBreference.Child("scores").Push().Key;

        ScoreEntry newScore = new ScoreEntry(playerId, score);
        string json = JsonUtility.ToJson(newScore);

        FirebaseInit.DBreference.Child("scores").Child(key).SetRawJsonValueAsync(json);
    }
    public void BossReward(int score)
    {
        DistScore += score;
    }
}

[System.Serializable]
public class ScoreEntry
{
    public string playerId;
    public int score;

    public ScoreEntry(string playerId, int score)
    {
        this.playerId = playerId;
        this.score = score;
    }
}