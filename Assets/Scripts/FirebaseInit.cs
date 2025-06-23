using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Threading.Tasks;
using UnityEditor.VersionControl;

public class FirebaseInit : MonoBehaviour
{
    public static DatabaseReference DBreference;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                DBreference = FirebaseDatabase.DefaultInstance.RootReference;
                Debug.Log("Firebase Initialized");
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
            }
        });
    }
}

