using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Fighters
{
    MARIE,
    DUKEZ
}

public class MainGameManager : MonoBehaviour
{
    private static MainGameManager instance;

    public static MainGameManager Instance
    {
        get
        {
            if (instance == null)
                instance = new MainGameManager();
            return instance;
        }
        
    }

    public MainGameManager()
    {
       // Object.DontDestroyOnLoad(this);
    }
    private static int activePlayers;
    private static int round;
    private static int[] playerVictories = new int[2];
    private static Fighters[] fighters = new Fighters[2];


    public int ActivePlayers
    {
        get { return activePlayers; }
        set { activePlayers = value; }
    }
    public int Rounds
    {
        get { return round; }
        set { round = value; }
    }
    public Fighters[] Fighters
    {
        get { return fighters; }
        set { fighters = value; }
    }
    public int[] PlayerVictories
    {
        get { return playerVictories; }
        set { playerVictories = value; }
    }

    void Start()
    {
        round++;
    }

    public void ClearRounds()
    {
        round = 0;
        playerVictories = new int[2];

    }



}

