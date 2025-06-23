using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // Singleton instance
    public static EventManager Instance;

    // Dictionary to hold lists of listeners for each event type
    private Dictionary<NeonGrindEvents, List<INeonGrindListener>> listeners = new();

    private void Awake()
    {
        // Enforce singleton pattern and persist between scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Registers a listener for a specific event type.
    public void AddListener(NeonGrindEvents eventType, INeonGrindListener listener)
    {
        if (listener == null) return;

        // Create list for event if it doesn't exist
        if (!listeners.TryGetValue(eventType, out var listenList))
        {
            listenList = new List<INeonGrindListener>();
            listeners[eventType] = listenList;
        }

        // Add listener if not already registered
        if (!listenList.Contains(listener))
        {
            listenList.Add(listener);
        }
    }

    // Posts an event notification to all registered listeners of the specified event type.
    public void PostNotification(NeonGrindEvents eventType, Component sender, object param = null)
    {
        if (!listeners.TryGetValue(eventType, out var listenList)) return;

        // Iterate backwards to safely handle removal during iteration
        for (int i = listenList.Count - 1; i >= 0; i--)
        {
            listenList[i]?.OnEvent(eventType, sender, param);
        }
    }

    // Removes a specific listener from an event type.
    public void RemoveListener(NeonGrindEvents eventType, INeonGrindListener listener)
    {
        if (listeners.TryGetValue(eventType, out var listenList))
        {
            listenList.Remove(listener);

            // Clean up if there are no more listeners for this event type
            if (listenList.Count == 0)
            {
                listeners.Remove(eventType);
            }
        }
    }
    public void Clear()
    {
        listeners.Clear();
    }
}