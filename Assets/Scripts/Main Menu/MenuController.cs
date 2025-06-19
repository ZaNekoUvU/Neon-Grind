using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public GameObject start;
    public GameObject credits;
    public GameObject exit;

    public void StartButtonPress()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void CreditButtonPress()
    {
        SceneManager.LoadScene("CreditScene");
    }
    public void ExitButtonPress()
    {
        Application.Quit();
    }
}
