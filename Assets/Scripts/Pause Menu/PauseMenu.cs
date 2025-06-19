using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void OnResumeButton()
    {
        Pause pauseScript = FindAnyObjectByType<Pause>();
        if (pauseScript != null)
        {
            pauseScript.PlayPress();
        }
    }

    public void OnResetButton()
    {
        Pause pauseScript = FindAnyObjectByType<Pause>();
        if (pauseScript != null)
        {
            pauseScript.ResetPress();
        }
    }
}
