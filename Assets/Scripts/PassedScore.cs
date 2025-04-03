using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PassedScore : MonoBehaviour
{
    public int primaryScore = 0;
    public int passScore = 50;
    private TMP_Text scoreDisplay2; // No need for [SerializeField] since we’ll assign it at runtime
    public bool isAddingScore = false;
    public int PrimaryScore {  get { return primaryScore; } }

    public void Awake()
    {
        GameObject textObject = GameObject.Find("2nd Score Count");
        if (textObject != null)
        {
            scoreDisplay2 = textObject.GetComponent<TMP_Text>();
        }
        else
        {
            Debug.LogError("Could not find '2nd Score Count' GameObject in the scene!");
        }
    }

    public void OnTriggerEnter(Collider pass)
    {
        if (pass.gameObject.CompareTag("Obstacle"))
        {

            //Debug.Log(primaryScore);

            ScoreUpdate();
        }
    }

    void ScoreUpdate()
    {
        primaryScore += passScore;

        //Debug.Log("number 2" + primaryScore);
        scoreDisplay2.text = primaryScore.ToString();
        
    }
}        //secondaryScore += primaryScore;
         //primaryScore = 0;
         //primaryScore = secondaryScore;
         //Debug.Log("number 2" + secondaryScore);