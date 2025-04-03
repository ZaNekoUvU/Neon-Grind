using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PassedScore : MonoBehaviour
{
    private int primaryScore = 0;
    private int secondaryScore = 0;
    public int passScore = 50;
    private TMP_Text scoreDisplay2; // No need for [SerializeField] since we’ll assign it at runtime
    public bool isAddingScore = false;

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

    private void Update()
    {

    }

    IEnumerator AddingScore()
    {
        primaryScore = secondaryScore + passScore;

        scoreDisplay2.text = primaryScore.ToString();
        Debug.Log(primaryScore);

        yield return new WaitForSeconds(0.1f);

    }

    public void OnTriggerEnter(Collider pass)
    {
        Debug.Log("triggered");
        if (pass.gameObject.CompareTag("Player"))
        {

            StartCoroutine(AddingScore());

        }
    }


}