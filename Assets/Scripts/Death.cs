using TMPro;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Death : MonoBehaviour
{
    public int finalScore;
    public int temp;
    public TextMeshProUGUI finalScoreText;
    public Image background;
    public GameObject screen;
    //[SerializeField] private PassedScore passedScore;
    public PassedScore passedScore;

    private void OnCollisionEnter(Collision collide)
    {
        PlayerMovement movementScript = collide.gameObject.GetComponent<PlayerMovement>();

        GameObject gm = GameObject.Find("LevelControls"); //finds the level control game object
        Generator generatorScript = gm.GetComponent<Generator>();
        Score scoreScript = gm.GetComponent<Score>();

        if (collide.gameObject.CompareTag("Player"))
        {
            Debug.Log("player collided");
            movementScript.enabled = false;

            if (generatorScript != null)
            {
                generatorScript.enabled = false;  
            }

            if (scoreScript != null)
            {
                scoreScript.enabled = false;
            }

            temp = passedScore.PrimaryScore;

            Debug.Log(temp.ToString());
            GameOver(temp);
        }
    }

    private void Awake()
    {
        screen = ObjectReference.background;
        screen.SetActive(false);
        finalScoreText = ObjectReference.text;
        passedScore = GameObject.Find("Player").GetComponent<PassedScore>();

        if (background != null)
        {
            background = screen.GetComponent<Image>();
        }
    }
    public void GameOver(int finalScore)
    {
        screen.SetActive(true);
        finalScoreText.text = " FINAL SCORE: "+finalScore.ToString();
        Debug.Log("" + finalScore.ToString());

    }

}
