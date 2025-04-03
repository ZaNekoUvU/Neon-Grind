using TMPro;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Death : MonoBehaviour
{
    public int finalScore;
    public int obstacleScore;
    public int combinedScore;
    public int distScore;
    public TextMeshProUGUI finalScoreText;
    public Image background;
    public GameObject screen;
    //[SerializeField] private PassedScore passedScore;
    public PassedScore passedScore;
    public Score score;

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

            obstacleScore = passedScore.PrimaryScore;
            distScore = score.DistScore;
            combinedScore = obstacleScore + distScore;

            GameOver(combinedScore);
        }
    }

    private void Awake()
    {
        screen = ObjectReference.background;
        screen.SetActive(false);
        finalScoreText = ObjectReference.text;
        passedScore = GameObject.Find("Player").GetComponent<PassedScore>();
        score = GameObject.Find("LevelControls").GetComponent<Score>();

        if (background != null)
        {
            background = screen.GetComponent<Image>();
        }
    }
    public void GameOver(int finalScore)
    {
        screen.SetActive(true);
        finalScoreText.text = " FINAL SCORE: " + finalScore.ToString();
        Debug.Log("" + finalScore.ToString());

    }

}