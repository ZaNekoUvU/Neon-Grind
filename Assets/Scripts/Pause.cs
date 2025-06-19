using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    //public GameObject hideCanvas;
    //public float delay = 1f; //delays

    public void PausePress()
    {
            SceneManager.LoadSceneAsync("Pause", LoadSceneMode.Additive);

            Time.timeScale = 0f;
    }

    public void PlayPress()
    {
        //if (SceneManager.GetActiveScene().name == "Pause")
        //{

            Debug.Log("Unpause");
            SceneManager.UnloadSceneAsync("Pause");

            Time.timeScale = 1f;
        //}
    }

    public void MenuPress()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void ResetPress()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }
}
