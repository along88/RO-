using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rounds : MonoBehaviour {
    public int round;
    public int[] playerVictories = new int[2];
   
	// Use this for initialization
	void Start () {
        round++;
        
        Object.DontDestroyOnLoad(this);
    }
	
	public void ClearRounds()
    {
        round = 0;
        playerVictories = new int[2];

    }
}
