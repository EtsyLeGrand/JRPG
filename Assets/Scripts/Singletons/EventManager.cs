using System;
using System.Collections.Generic;
using UnityEngine;

// Source: http://bernardopacheco.net/using-an-event-manager-to-decouple-your-game-in-unity

public class EventManager : Singleton<EventManager>
{
    private Dictionary<string, Action<Dictionary<string, object>>> eventDictionary;

    void Start()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, Action<Dictionary<string, object>>>();
        }
    }

    public static void StartListening(string eventName, Action<Dictionary<string, object>> listener)
    {
        if (Instance.eventDictionary.TryGetValue(eventName, 
            out Action<Dictionary<string, object>> thisEvent))
        {
            thisEvent += listener;
            Instance.eventDictionary[eventName] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            Instance.eventDictionary.Add(eventName, thisEvent);
        }

        Debug.Log("Now listening to " + eventName);
    }

    public static void StopListening(string eventName, Action<Dictionary<string, object>> listener)
    {
        //if (eventManager == null) return;
        if (Instance.eventDictionary.TryGetValue(eventName, 
            out Action<Dictionary<string, object>> thisEvent))
        {
            thisEvent -= listener;
            Instance.eventDictionary[eventName] = thisEvent;
        }
    }

    public static void TriggerEvent(string eventName, Dictionary<string, object> message)
    {
        if (Instance.eventDictionary.TryGetValue(eventName, 
            out Action<Dictionary<string, object>> thisEvent))
        {
            thisEvent.Invoke(message);
        }
    }
}