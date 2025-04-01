using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public void PausePress()
    {        
        Time.timeScale = 0f;       
    }

    public void PlayPress()
    {
        Time.timeScale = 1f;
    }

    public void ResetPress()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
