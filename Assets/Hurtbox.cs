using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour {

    [SerializeField]
    private GameEvent damage;

    private void OnTriggerEnter(Collider other)
    {
        
            damage.Raise();
        
    }
}
