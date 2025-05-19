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
        Time.timeScale = 0f;       
    }

    public void PlayPress()
    {
        Time.timeScale = 1f;
    }

    public void ResetPress()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    //public void HideCanvas()
    //{
    //    StartCoroutine(HideAfterDelay());
    //}

    //private IEnumerator HideAfterDelay()
    //{
    //    yield return new WaitForSeconds(delay);
    //    hideCanvas.SetActive(false);
    //}
}
