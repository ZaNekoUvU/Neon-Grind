using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PassedScore : MonoBehaviour
{
    private int primaryScore = 0;
    public int passScore = 50;
    private TMP_Text scoreDisplay2; // No need for [SerializeField] since we’ll assign it at runtime
    public bool isAddingScore = false;

    public void Awake()
    {
        primaryScore = 0;
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

    private void Update()
    {
    }

    public void OnTriggerEnter(Collider pass)
    {
        Debug.Log("triggered");
        if (pass.gameObject.CompareTag("Player"))
        {
            ScoreUpdate();

            Debug.Log("Is Player Trigger");
            //StartCoroutine(AddingScore());

        }
    }

    void ScoreUpdate()
    {
        primaryScore = primaryScore + passScore;

        scoreDisplay2.text = primaryScore.ToString();
    }


}