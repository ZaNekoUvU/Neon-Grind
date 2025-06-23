using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject playerPrefab;

    [SerializeField] AudioClip buttonPress;
    private AudioSource buttonPressSound;

    private void Awake()
    {
        buttonPressSound = gameObject.AddComponent<AudioSource>();
        buttonPressSound.playOnAwake = false;
        buttonPressSound.clip = buttonPress;
    }
    public void PausePress()
    {
        buttonPressSound.Play();
        SceneManager.LoadSceneAsync("Pause", LoadSceneMode.Additive);
         Time.timeScale = 0f;
    }

    public void PlayPress()
    {
        buttonPressSound.Play();
        Debug.Log("Unpause");
        SceneManager.UnloadSceneAsync("Pause");

        Time.timeScale = 1f;
    }

    public void MenuPress()
    {
        buttonPressSound.Play();
        SceneManager.LoadScene("Main Menu");
    }

    public void ResetPress()
    {
        buttonPressSound.Play();
        var generator = FindFirstObjectByType<Generator>();
        if (generator != null)
            Destroy(generator.gameObject);

        SceneManager.LoadScene("Game");

        
    }
}
