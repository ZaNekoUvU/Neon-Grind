using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public void MenuButtonPress()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartButtonPress()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("Game");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game")
        {
            GameObject player = GameObject.FindWithTag("Player");
            Generator generator = FindFirstObjectByType<Generator>();
            if (player != null && generator != null)
            {
                generator.ResetGenerator(player.transform);
            }

            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}