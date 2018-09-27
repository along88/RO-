using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event", menuName = "Events/Event", order = 1)]
public class GameEvent : ScriptableObject
{
    private List<EventListener> listeners = new List<EventListener>();

    public void Raise()
    {
        for (int index = listeners.Count - 1; index >= 0; index--)
        {
            listeners[index].OnNotify();
        }
    }

    public void RegisterEvent(EventListener listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    public void RemoveEvent(EventListener listener)
    {
        if (listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
        
    }
}
