using TMPro;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Death : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;
    public Image background;
    public GameObject screen;
    public Score obstacleScore;
    public Score score;
    private int finalscore;

    public Score scoreStorage;

    private void OnCollisionEnter(Collision collide)
    {
        PlayerMovement movementScript = collide.gameObject.GetComponent<PlayerMovement>();

        GameObject gm = GameObject.Find("LevelControls");
        Generator generatorScript = gm.GetComponent<Generator>();
        Score scoreScript = gm.GetComponent<Score>();

        if (collide.gameObject.CompareTag("Player"))
        {
            string playerId = "Player_" + Random.Range(1000, 9999);
            score.SaveScore(playerId);

            SceneManager.LoadScene("Death");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement movementScript = other.gameObject.GetComponent<PlayerMovement>();

        GameObject gm = GameObject.Find("LevelControls");
        Generator generatorScript = gm.GetComponent<Generator>();
        Score scoreScript = gm.GetComponent<Score>();

        if (other.gameObject.CompareTag("Player"))
        {
            string playerId = "Player_" + Random.Range(1000, 9999);
            score.SaveScore(playerId);

            SceneManager.LoadScene("Death");
        }
    }

    private void Awake()
    {
        finalScoreText = ObjectReference.text;
        obstacleScore = GameObject.Find("Player").GetComponent<Score>();
        score = GameObject.Find("Player").GetComponent<Score>();
    }
    public void GameOver(int finalScore)
    {
    }
}