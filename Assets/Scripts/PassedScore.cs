using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PassedScore : MonoBehaviour
{
    public int secondaryScore;
    public int passScore = 50;
    [SerializeField] public TMP_Text scoreDisplay2;


    private void Update()
    {
        scoreDisplay2.text = "" + secondaryScore;
    }

    public void OnTriggerEnter(Collider pass)
    {        
        if (pass.gameObject.CompareTag("Player"))
        {
            Debug.Log("add score");
            secondaryScore += passScore;
        }
    }
}
