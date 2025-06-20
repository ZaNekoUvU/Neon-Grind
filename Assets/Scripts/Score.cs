using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField]
    public TMP_Text scoreDisplay;
    private int score;
    public const int passScore = 50;
    public bool isAddingScore = false;
    public int DistScore { get { return score; } }

    public void Awake()
    {

    }

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

    //public void OnTriggerEnter(Collider pass)
    //{
    //    if (pass.gameObject.CompareTag("Obstacle")) //increases score when passing obstacles
    //    {
    //        score += passScore;

    //        scoreDisplay.text = "" + score;
    //    }
    //}

    public void AddObstacleScore()
    {
        score += passScore;
        //scoreDisplay.text = score.ToString();
    }
}