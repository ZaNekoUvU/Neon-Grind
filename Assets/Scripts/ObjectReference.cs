using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectReference : MonoBehaviour
{
    public static TextMeshProUGUI text;
    public static GameObject background;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = GameObject.Find("Final score").GetComponent<TextMeshProUGUI>();
        background = GameObject.FindGameObjectWithTag("Back");


    }
}
