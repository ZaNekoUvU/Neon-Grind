using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
   [SerializeField]
    public TMP_Text scoreDisplay;
    public int score;
    public bool isAddingScore = false;
    public int DistScore { get { return score; } }

    void Update()
    {
        if (isAddingScore == false)
        {
            isAddingScore = true;
            StartCoroutine(AddingScore());

        }
    }

    IEnumerator AddingScore()
    {
        score++;
        scoreDisplay.text = "" + score;
        yield return new WaitForSeconds(0.1f);
        isAddingScore = false;
    }
}