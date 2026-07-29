using UnityEngine;

public enum Fighters
{
    MARIE,
    DUKEZ
}

public class MainGameManager : MonoBehaviour
{
    public static MainGameManager Instance { get; private set; }

    [SerializeField]
    private int activePlayers = 2;

    private int round;
    private int[] playerVictories = new int[2];

    [SerializeField]
    private Fighters[] fighters = new Fighters[2];

    [SerializeField]
    private bool[] playerColorPriority = new bool[2];

    // Current individual round.
    public bool IsMatchOver { get; set; }
    public bool IsPlayerOneVictory { get; set; }

    // Entire best-of-three set.
    public bool IsMatchSetOver { get; set; }
    public int WinningPlayerId { get; set; }

    public int ActivePlayers
    {
        get => activePlayers;
        set => activePlayers = value;
    }

    public int Rounds
    {
        get => round;
        set => round = value;
    }

    public Fighters[] Fighters
    {
        get => fighters;
        set => fighters = value;
    }

    public int[] PlayerVictories
    {
        get => playerVictories;
        set => playerVictories = value;
    }

    public bool[] PlayerColorPriority
    {
        get => playerColorPriority;
        set => playerColorPriority = value;
    }

    //public static MainGameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        activePlayers = 2;
    }

    public void BeginRound()
    {
        round++;

        IsMatchOver = false;
        IsPlayerOneVictory = false;
    }

    public void ClearRounds()
    {
        round = 0;
        playerVictories = new int[2];

        IsMatchOver = false;
        IsPlayerOneVictory = false;

        IsMatchSetOver = false;
        WinningPlayerId = 0;
    }
}