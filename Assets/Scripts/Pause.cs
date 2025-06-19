using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    //public GameObject hideCanvas;
    //public float delay = 1f; //delays
    private bool isPaused = false;

    public void PausePress()
    {        
        if (!isPaused)
        {            
            SceneManager.LoadSceneAsync("Pause", LoadSceneMode.Additive);
            Time.timeScale = 0f;
            isPaused = true;
        }        
    }

    public void PlayPress()
    {
        SceneManager.UnloadSceneAsync("Pause");
        Time.timeScale = 1f;

        isPaused = false;
    }

    public void ResetPress()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }
}
