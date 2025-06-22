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
        SceneManager.LoadScene("SampleScene");
    }
}
