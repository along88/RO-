using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    private static EventManager instance;

    public static EventManager Instance
    {
        get
        {
            if (instance == null)
                instance = new EventManager();
            return instance;
        }
    }

    public delegate void OnDamage();

    public event OnDamage DamageHandler;

    public void OnDamageTaken()
    {
        if(DamageHandler != null)
            DamageHandler();
        
    }
	
}
