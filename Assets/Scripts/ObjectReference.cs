using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectReference : MonoBehaviour
{
    public static TextMeshProUGUI text;
    public static GameObject background;

    void Start()
    {
        text = GameObject.Find("Final score").GetComponent<TextMeshProUGUI>();
        background = GameObject.FindGameObjectWithTag("Back");


    }
}
