using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using System.Threading.Tasks;

public class FirebaseInit : MonoBehaviour
{
    public static DatabaseReference DBreference;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(Task =>
        {
            var status = Task.Result;
            if (status == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                Debug.Log("Firebase Initialized");
            }
            else
            {
                Debug.LogError("Could not resolbe all Firebase dependencies: " + status);
            }
        });
    }
}
