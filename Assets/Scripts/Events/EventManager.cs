using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    private Dictionary<NeonGrindEvents, List<INeonGrindListener>> listeners = new();

    private void Awake()
    {
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

    public void AddListener(NeonGrindEvents eventType, INeonGrindListener listener)
    {
        if (listener == null) return;


        if (!listeners.TryGetValue(eventType, out var listenList))
        {
            listenList = new List<INeonGrindListener>();
            listeners[eventType] = listenList;
        }

        if (!listenList.Contains(listener))
        {
            listenList.Add(listener);
        }
    }

    public void PostNotification(NeonGrindEvents eventType, Component sender, object param = null)
    {
        if (!listeners.TryGetValue(eventType, out var listenList)) return;

        for (int i = listenList.Count - 1; i >= 0; i--)
        {
            listenList[i]?.OnEvent(eventType, sender, param);
        }
    }

    public void RemoveListener(NeonGrindEvents eventType, INeonGrindListener listener)
    {
        if (listeners.TryGetValue(eventType, out var listenList))
        {
            listenList.Remove(listener);

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
