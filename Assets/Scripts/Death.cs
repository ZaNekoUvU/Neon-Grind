using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Death : MonoBehaviour
{
    #region UI references 
    public TextMeshProUGUI finalScoreText;
    public Image background;
    public GameObject screen;
    #endregion

    #region Score tracking
    public Score obstacleScore;
    public Score score;
    private int finalscore;
    #endregion

    private void Awake()
    {
        screen = ObjectReference.background;
        finalScoreText = ObjectReference.text;

        score = GameObject.Find("Player").GetComponent<Score>();
        obstacleScore = score;
    }

    private void OnCollisionEnter(Collision collide)
    {
        if (collide.gameObject.CompareTag("Player"))
        {
            HandleDeathCollision(collide.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HandleDeathCollision(other.gameObject);
        }
    }

    private void HandleDeathCollision(GameObject player)
    {
        PlayerMovement movementScript = player.GetComponent<PlayerMovement>();
        GameObject gm = GameObject.Find("LevelControls");
        Generator generatorScript = gm.GetComponent<Generator>();
        Score scoreScript = gm.GetComponent<Score>();

        SceneManager.LoadScene("Death");

    }

    // Displays the death screen with the final score
    /*public void GameOver(int finalScore)
    {
        screen.SetActive(true);
        finalScoreText.text = "FINAL SCORE: " + finalScore.ToString();
    }*/
}