using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour
{
    [SerializeField]
    private GameEvent gameEvent;

    [SerializeField]
    private UnityEvent response;

    void OnEnable()
    {
        gameEvent.RegisterEvent(this);
    }

    void OnDisable()
    {
        gameEvent.RemoveEvent(this);
    }

    public void OnNotify()
    {
        response.Invoke();
    }
}
