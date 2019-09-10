using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroySelf : MonoBehaviour {
    public float seconds;
    float timer;
	// Use this for initialization
	void Awake () {
        seconds = .5f;
        timer = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
       // Debug.Log(Mathf.Abs(timer - Time.time).ToString());
		if(Mathf.Abs(timer - Time.time) >= seconds)
        {
            Destroy(this.gameObject);
        }
        
	}
}
