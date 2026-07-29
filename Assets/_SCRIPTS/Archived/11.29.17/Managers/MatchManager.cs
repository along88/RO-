using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


class MatchManager: Manager
{
    [SerializeField]
    private Sprite[] PlayerIcon;
    
    private Vector3 cameraPosition;
    private Camera matchSetcamera;
    private Camera mainCamera;
    private Image uiTime;
    private GameObject MatchSetMenuObject;
    [SerializeField]
    private float matchTimer;
    [SerializeField]
    private Text matchTimerText;
    [SerializeField]
    private float match;
    [SerializeField]
    private Text ringOutText;
    private AudioClip[] playersTheme;
    GameObject pauseMenu;
    [SerializeField]
    Image ringOut;
    [SerializeField]
    private GameObject playerBounds;
    [SerializeField]
    private GameObject[] prefabs;

    public GameObject[] FighterModel;
    public float Match { get { return match; } set { match = value; } }


    private void SelectFighter(Fighters[] fighters)
    {
        var priority = MainGameManager.Instance.PlayerColorPriority;
        switch (fighters[0])
        {
            case global::Fighters.MARIE:
            FighterModel[0] = Instantiate(prefabs[0], new Vector3(-39, 43, 10), Quaternion.LookRotation(Vector3.forward));
            //FighterModel[0].GetComponentInChildren<SpriteRenderer>().sprite = PlayerIcon[0];
            break;
            case global::Fighters.DUKEZ:
                FighterModel[0] = Instantiate(prefabs[1], new Vector3(-39, 43, 10), Quaternion.LookRotation(Vector3.forward));
                FighterModel[0].GetComponentInChildren<MaintainIconFacing>().facing = new Quaternion(0, 0, 0, 0f);
            break;
        default:
            break;
    }
        switch (fighters[1])
        {
            case Fighters.MARIE:
                if (fighters[0] == fighters[1] && !priority[1])
                {
                    //fighters are the same, make p2 different color
                    FighterModel[1] = Instantiate(prefabs[0], new Vector3(9, 43, 10), Quaternion.LookRotation(Vector3.back));
                }
                else
                    FighterModel[1] = Instantiate(prefabs[0], new Vector3(9, 43, 10), Quaternion.LookRotation(Vector3.back));
                if (MainGameManager.Instance.ActivePlayers == 2)
                    FighterModel[1].GetComponent<Player>().ID = 2;
                else
                    FighterModel[1].GetComponent<Player>().ID = 0;
                var IconRotation = FighterModel[1].GetComponentInChildren<MaintainIconFacing>().facing;
                FighterModel[1].GetComponentInChildren<MaintainIconFacing>().facing = new Quaternion(0, 0, 0, 180);
               break;
            case Fighters.DUKEZ:
                if (fighters[0] == fighters[1] && !priority[1])
                {
                    FighterModel[1] = Instantiate(prefabs[1], new Vector3(9, 43, 10), Quaternion.LookRotation(Vector3.back));
                //    FighterModel[1].GetComponent<Image>().material.color = Color.red;
                }
                else
                    FighterModel[1] = Instantiate(prefabs[1], new Vector3(9, 43, 10), Quaternion.LookRotation(Vector3.back));
                break;
            default:
                break;
        }
        FighterModel[0].GetComponentInChildren<SpriteRenderer>().sprite = PlayerIcon[0];
        FighterModel[0].GetComponentInChildren<SpriteRenderer>().color = Color.red;
        FighterModel[1].GetComponentInChildren<SpriteRenderer>().sprite = PlayerIcon[1];
        FighterModel[1].GetComponentInChildren<SpriteRenderer>().color = Color.blue;
        
        FighterModel[0].GetComponent<Player>().ID = 1;
        FighterModel[1].GetComponent<Player>().ID = 2;
        //if (MainGameManager.Instance.ActivePlayers == 2)
        //    FighterModel[1].GetComponent<Player>().ID = 2;
        //else
        //    FighterModel[1].GetComponent<Player>().ID = 0;
    }
        
    private void Awake()
    {
        SelectFighter(MainGameManager.Instance.Fighters);
        AssignOpponent();
        InitializeComponents();
        InitializeButtons();

        var foundPlayers = new List<Player>();

        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
            foundPlayers.Add(player.GetComponent<Player>());

        players = foundPlayers.ToArray();
        MainGameManager.Instance.ActivePlayers = players.Count();
    }
    private void Start()
    {
        MainGameManager.Instance.BeginRound();
        GetPlayers();
            
        GetCameras();

    }
    private void GetPlayers()
    {
        
        players = new Player[2];
        playerBounds = GameObject.FindGameObjectWithTag("StageBounds");
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<Player>().ID == 1)
            {
                players[0] = player.GetComponent<Player>();

                playersTheme[0] = player.GetComponent<AudioManager>().hypeMusic;
            }
            else
            {
                players[1] = player.GetComponent<Player>();

                playersTheme[1] = player.GetComponent<AudioManager>().hypeMusic;
            }


        }
    }
    private void Update()
    {
        RoundTimer();

        RingOutVictory();
        foreach (var player in players)
        {
            if (player.IsHyped) {
                StartCoroutine("HypeTaunt", 0);
                break;
            }
        }
    }
        
    private void InitializeComponents()
    {
        pauseMenu = GameObject.FindGameObjectWithTag("ShowOnPause");
        MatchSetMenuObject = GameObject.FindGameObjectWithTag("MatchMenu");
        nav = GameObject.FindGameObjectWithTag("Nav");
        nav.SetActive(false);
        MatchSetMenuObject.SetActive(false);
        pauseMenu.SetActive(false);
        ringOut.enabled = false;
        audioSource = GetComponent<AudioSource>();

        uiTime = GameObject.Find("Time").GetComponent<Image>();
        uiTime.enabled = false;
        matchTimerText = GetComponentInChildren<Text>();
        playersTheme = new AudioClip[2];
    }

    private void AssignOpponent()
    {
        foreach (var _opponent in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (_opponent.GetComponent<Player>() != FighterModel[0].GetComponent<Player>())
                FighterModel[0].GetComponent<Player>().Opponent = _opponent.GetComponent<Player>();
            else
                FighterModel[1].GetComponent<Player>().Opponent = _opponent.GetComponent<Player>();
        }
    }
        
        
    private void RoundTimer()
    {
        if (!MainGameManager.Instance.IsMatchOver)
            matchTimer -= Time.deltaTime;
        if (matchTimer <= 0)
        {

            UpdateTimer();
            DetermineMomentumWinner();
        }
        else if (matchTimer > 0)
            UpdateTimer();

    }
    private void UpdateTimer()
    {

        int seconds = (int)(matchTimer % 60);
        matchTimerText.text = seconds.ToString();


    }
    private void DetermineMomentumWinner()
    {

        if (!MainGameManager.Instance.IsMatchOver)
        {
            var slider = gameObject.GetComponentInChildren<Slider>();
            if (slider.value > 50.0f)
            {

                uiTime.enabled = true;
                MainGameManager.Instance.IsPlayerOneVictory = true;
                MainGameManager.Instance.IsMatchOver = true;
            }
            else if (slider.value < 50.0f)
            {

                uiTime.enabled = true;
                MainGameManager.Instance.IsPlayerOneVictory = false;
                MainGameManager.Instance.IsMatchOver = true;

            }
            else
            {
                uiTime.enabled = true;
                MainGameManager.Instance.IsMatchOver = true;

            }

        }


    }
    private void GetCameras()
    {
        foreach (var camera in GameObject.FindGameObjectsWithTag("camera"))
        {
            matchSetcamera = camera.GetComponent<Camera>();
        }
        foreach (var camera in GameObject.FindGameObjectsWithTag("MainCamera"))
        {
            mainCamera = camera.GetComponent<Camera>();
            cameraPosition = mainCamera.transform.position;
        }
        matchSetcamera.enabled = false;
    }
    private void RingOutVictory()
    {
        if (!MainGameManager.Instance.IsMatchOver)
        {
            if (players[0].IsHypeHit)
            {
                ringOut.enabled = true;

                MainGameManager.Instance.IsPlayerOneVictory = false;
                MainGameManager.Instance.IsMatchOver = true;
            }
            else if (players[1].IsHypeHit)
            {

                ringOut.enabled = true;

                MainGameManager.Instance.IsPlayerOneVictory = true;
                MainGameManager.Instance.IsMatchOver = true;

            }
            if (players[0].transform.position.y < playerBounds.transform.position.y)
            {

                ringOut.enabled = true;

                MainGameManager.Instance.IsPlayerOneVictory = false;
                MainGameManager.Instance.IsMatchOver = true;

            }
            else if (players[1].transform.position.y < playerBounds.transform.position.y)
            {
                ringOut.enabled = true;
                MainGameManager.Instance.IsPlayerOneVictory = true;
                MainGameManager.Instance.IsMatchOver = true;
            }

        }
        else
        {
            StartCoroutine("MatchSetDelay", 0);
        }
        
    }
    private IEnumerator MatchSetDelay()
    {
        Debug.Log("Match");
        WaitForSeconds delay = new WaitForSeconds(2.0f);
        yield return delay;
        ringOut.enabled = false;
        uiTime.enabled = false;
        matchSetcamera.enabled = true;
        matchSetcamera.transform.position = cameraPosition;
        matchSetcamera.fieldOfView = 20.0f;
        if (MainGameManager.Instance.IsPlayerOneVictory)
        {

            StartCoroutine("VictoryTaunt", 0);
        }
        else if (!MainGameManager.Instance.IsPlayerOneVictory)
        {

            StartCoroutine("VictoryTaunt", 1);
        }
    }
    IEnumerator VictoryTaunt(int player)
    {
        
        
        matchSetcamera.transform.LookAt(players[player].transform.position);
        players[player].transform.LookAt(matchSetcamera.transform.position);
        players[player].IsTaunting = true;

        WaitForSeconds delay = new WaitForSeconds(2.0f);
        yield return delay;
        Debug.Log("Player1" + MainGameManager.Instance.PlayerVictories[0] + " Player2:" + MainGameManager.Instance.PlayerVictories[1]);

        if (MainGameManager.Instance.PlayerVictories[player] >= 1)
        {
            // This player already had one win, so this is their second.
            MainGameManager.Instance.IsMatchOver = true;
            MainGameManager.Instance.IsPlayerOneVictory = player == 0;

            Debug.Log(
                $"Full match over. Winner: Player {player + 1}"
            );

            MatchSetNavigation();
        }
        else
        {
            // First round win.
            MainGameManager.Instance.PlayerVictories[player]++;
            MainGameManager.Instance.Rounds++;

            if (MainGameManager.Instance.ActivePlayers == 2)
                SceneManager.LoadScene("Multiplayer");
            else
                SceneManager.LoadScene("SinglePlayer");
        }

    }
    IEnumerator HypeTaunt(int player)
    {
        
        while (players[player].IsHyped)
        {
            WaitForSeconds delay = new WaitForSeconds(2.0f);
            yield return delay;
        }


    }
    protected  void InitializeButtons()
    {
        MatchSetMenuObject.SetActive(false);
        pauseMenu.SetActive(false);
        
    }
    public void MatchSetNavigation()
    {
        MatchSetMenuObject.SetActive(true);
        nav.SetActive(true);

        Text text =
            MatchSetMenuObject.GetComponentInChildren<Text>();

        if (MainGameManager.Instance.IsPlayerOneVictory)
        {
            players[1].gameObject.SetActive(false);
            text.text = players[0].name + " Wins!";
        }
        else
        {
            players[0].gameObject.SetActive(false);
            text.text = players[1].name + " Wins!";
        }

        Time.timeScale = 0.01f;
    }

    public void Rematch()
    {
        Time.timeScale = 1f;
        MainGameManager.Instance.ClearRounds();

        if (MainGameManager.Instance.ActivePlayers == 2)
            SceneManager.LoadScene("Multiplayer");
        else
            SceneManager.LoadScene("SinglePlayer");
    }
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        MainGameManager.Instance.ClearRounds();
        SceneManager.LoadScene("Main Menu");
    }
    public void ResumeMatch()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenu.SetActive(false);
    }

}