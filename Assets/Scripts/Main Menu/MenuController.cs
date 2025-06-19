using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{


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
