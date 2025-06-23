using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreboardManager : MonoBehaviour
{
    public Transform scoreListParent;
    public GameObject scoreEntryPrefab; // Make a prefab with TMP_Text or whatever
    private List<ScoreEntry> topScores = new List<ScoreEntry>();

    void Start()
    {
        LoadTopScores();
    }

    public void LoadTopScores()
    {
        FirebaseInit.DBreference.Child("scores").OrderByChild("score").LimitToLast(10).GetValueAsync().ContinueWithOnMainThread(task =>
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

            topScores.Sort((a, b) => b.score.CompareTo(a.score)); // Highest to lowest
            DisplayScores();
        });
    }

    private void DisplayScores()
    {
        foreach (Transform child in scoreListParent)
            Destroy(child.gameObject);

        foreach (ScoreEntry entry in topScores)
        {
            GameObject newEntry = Instantiate(scoreEntryPrefab, scoreListParent);
            newEntry.GetComponent<TMP_Text>().text = $"{entry.playerId}: {entry.score}";
        }
    }

    public void MenuLoaderPress()
    {
        SceneManager.LoadScene("MainMenu");

    }
}
