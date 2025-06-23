using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartButtonPress()
    {
        SceneManager.LoadScene("Game");
    }
    public void CreditButtonPress()
    {
        SceneManager.LoadScene("CreditScene");
    }
    public void LeaderBoardButtonPress()
    {
        SceneManager.LoadScene("LeaderBoardScene");
    }
    public void ExitButtonPress()
    {
        Application.Quit();
    }
}
