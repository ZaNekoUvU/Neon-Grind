using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject playerPrefab;

    public void PausePress()
    {
         SceneManager.LoadSceneAsync("Pause", LoadSceneMode.Additive);
         Time.timeScale = 0f;
    }

    public void PlayPress()
    {
         Debug.Log("Unpause");
         SceneManager.UnloadSceneAsync("Pause");

         Time.timeScale = 1f;
    }

    public void MenuPress()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void ResetPress()
    {
        var generator = FindFirstObjectByType<Generator>();
        if (generator != null)
            Destroy(generator.gameObject);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        
    }
}
