using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreboardManager : MonoBehaviour
{
    public TMP_Text[] scoreTextArray = new TMP_Text[10]; // Assign these in the Inspector
    private List<ScoreEntry> topScores = new List<ScoreEntry>();

    void Start()
    {
        LoadTopScores();
    }

    public void LoadTopScores()
    {
        FirebaseInit.DBreference.Child("scores")
            .OrderByChild("score")
            .LimitToLast(10)
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Failed to load scores.");
                    return;
                }

                DataSnapshot snapshot = task.Result;
                topScores.Clear();

                foreach (DataSnapshot scoreSnapshot in snapshot.Children)
                {
                    string playerId = scoreSnapshot.Child("playerId").Value.ToString();
                    int score = int.Parse(scoreSnapshot.Child("score").Value.ToString());
                    topScores.Add(new ScoreEntry(playerId, score));
                }

                topScores.Sort((a, b) => b.score.CompareTo(a.score)); // Highest first

                DisplayScores();
            });
    }

    private void DisplayScores()
    {
        // First clear all fields
        for (int i = 0; i < scoreTextArray.Length; i++)
        {
            scoreTextArray[i].text = "";
        }

        // Then fill with top scores
        for (int i = 0; i < Mathf.Min(topScores.Count, scoreTextArray.Length); i++)
        {
            ScoreEntry entry = topScores[i];
            scoreTextArray[i].text = $"{entry.playerId}: {entry.score}";
        }
    }

    public void MenuLoaderPress()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
