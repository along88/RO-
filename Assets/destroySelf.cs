using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroySelf : MonoBehaviour {
    float timer;
	// Use this for initialization
	void Awake () {
        timer = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
       // Debug.Log(Mathf.Abs(timer - Time.time).ToString());
		if(Mathf.Abs(timer - Time.time) >= .5f)
        {
            Destroy(this.gameObject);
        }
        
	}
}
