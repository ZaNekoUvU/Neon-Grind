using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public void MenuButtonPress()
    {
        Debug.Log("MenuButton");
        SceneManager.LoadScene("MainMenu");
    }
}
