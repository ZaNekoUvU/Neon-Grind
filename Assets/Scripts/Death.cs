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

    public Score scoreStorage;

    private void OnCollisionEnter(Collision collide)
    {
<<<<<<< HEAD
        if (collide.gameObject.CompareTag("Player"))
        {
            HandleDeathCollision(collide.gameObject);
=======
        PlayerMovement movementScript = collide.gameObject.GetComponent<PlayerMovement>();

        GameObject gm = GameObject.Find("LevelControls");
        Generator generatorScript = gm.GetComponent<Generator>();
        Score scoreScript = gm.GetComponent<Score>();

        if (collide.gameObject.CompareTag("Player"))
        {
            string playerId = "Player_" + Random.Range(1000, 9999);
            score.SaveScore(playerId);

            SceneManager.LoadScene("Death");
>>>>>>> Angus2
        }
    }

    private void OnTriggerEnter(Collider other)
    {
<<<<<<< HEAD
        if (other.CompareTag("Player"))
        {
            HandleDeathCollision(other.gameObject);
=======
        PlayerMovement movementScript = other.gameObject.GetComponent<PlayerMovement>();

        GameObject gm = GameObject.Find("LevelControls");
        Generator generatorScript = gm.GetComponent<Generator>();
        Score scoreScript = gm.GetComponent<Score>();

        if (other.gameObject.CompareTag("Player"))
        {
            string playerId = "Player_" + Random.Range(1000, 9999);
            score.SaveScore(playerId);

            SceneManager.LoadScene("Death");
>>>>>>> Angus2
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