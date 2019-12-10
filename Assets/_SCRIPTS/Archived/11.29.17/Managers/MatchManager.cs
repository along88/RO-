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
        

    //private float BGMLastTime;
    //private float hypeMusicLastTime;
    private Vector3 cameraPosition;
        
        
    private Camera matchSetcamera;
    private Camera mainCamera;
    private Image uiTime;
        
        
        
        
    private GameObject MatchSetMenuObject;
        
    [SerializeField]
    private float matchTimer;
    [SerializeField]
    private Text matchTimerText;
    //[SerializeField]
    //private Rounds rounds;
    [SerializeField]
    private float match;
    [SerializeField]
    private Player[] players;
    [SerializeField]
    private Text ringOutText;
    private AudioManager[] playersTheme;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioSource menuSFX;
    Manager pauseMenu;

    [SerializeField]
    Image ringOut;
    [SerializeField]
    private GameObject playerBounds;
        


    [SerializeField]
    private GameObject[] prefabs;

    public GameObject[] FighterModel;
    public float Match { get { return match; } set { match = value; } }
    // public Rounds Rounds { get { return rounds; } set { rounds = value; } }

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
                    
                // FighterModel[1].GetComponent<Image>().material.color = Color.red;
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
        FighterModel[1].GetComponentInChildren<SpriteRenderer>().color = Color.green;
        FighterModel[0].GetComponent<Player>().ID = 1;
        if (MainGameManager.Instance.ActivePlayers == 2)
            FighterModel[1].GetComponent<Player>().ID = 2;
        else
            FighterModel[1].GetComponent<Player>().ID = 0;
    }
        
    private void Awake()
    {
        SelectFighter(MainGameManager.Instance.Fighters);
        AssignOpponent();
        InitializeComponents();
        pauseMenu = new Manager();

}
    private void Start()
    {
        playerBounds = GameObject.FindGameObjectWithTag("StageBounds");
            
        GetCameras();

    }
    private void Update()
    {
        RoundTimer();
        pauseMenu.PauseMenu();
        RingOutVictory();
            
        AudioSourceManager audio = new AudioSourceManager();
        
    }
        
    private void InitializeComponents()
    {
        //stageTheme = GetComponent<AudioSource>().clip;
        ringOut.enabled = false;
        audioSource = GetComponent<AudioSource>();
        menuSFX = GetComponent<AudioSource>();
        uiTime = GameObject.Find("Time").GetComponent<Image>();
        uiTime.enabled = false;
        matchTimerText = GetComponentInChildren<Text>();
            
            
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
        if (!isMatchOver)
            matchTimer -= Time.deltaTime;
        if (matchTimer <= 0)
        {
            //matchTimer = 0;
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

        if (!isMatchOver)
        {
            var slider = gameObject.GetComponentInChildren<Slider>();
            if (slider.value > 50.0f)
            {
                //uiText.text = "Player 1 wins!";
                uiTime.enabled = true;
                isPlayerOneVictory = true;
                isMatchOver = true;


                // playersTheme[0].StopHypeMusic();

            }
            else if (slider.value < 50.0f)
            {
                //uiText.text = "Player 2 wins!";
                uiTime.enabled = true;
                isPlayerOneVictory = false;
                isMatchOver = true;
                // playersTheme[1].StopHypeMusic();
            }
            else
            {
                uiTime.enabled = true;
                isMatchOver = true;

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
        if (!isMatchOver)
        {
            if (players[0].IsHypeHit)
            {
                ringOut.enabled = true;

                isPlayerOneVictory = false;
                isMatchOver = true;
            }
            else if (players[1].IsHypeHit)
            {

                ringOut.enabled = true;

                isPlayerOneVictory = true;
                isMatchOver = true;

            }
            if (players[0].transform.position.y < playerBounds.transform.position.y)
            {

                ringOut.enabled = true;

                isPlayerOneVictory = false;
                isMatchOver = true;

            }
            else if (players[1].transform.position.y < playerBounds.transform.position.y)
            {
                ringOut.enabled = true;
                isPlayerOneVictory = true;
                isMatchOver = true;



            }
        }
    }
        

    private IEnumerator MatchSetDelay()
    {
        Debug.Log("Match");
        if (!audioSource.isPlaying)
            audioSource.Play();
        WaitForSeconds delay = new WaitForSeconds(2.0f);
        yield return delay;
        ringOut.enabled = false;
        uiTime.enabled = false;
        matchSetcamera.enabled = true;
        matchSetcamera.transform.position = cameraPosition;
        matchSetcamera.fieldOfView = 20.0f;
        if (isPlayerOneVictory)
        {

            StartCoroutine("VictoryTaunt", 0);
        }
        else if (!isPlayerOneVictory)
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

    //if (rounds.playerVictories[player] >= 1)
    //    StartCoroutine("MatchSetNavigation");
    if (MainGameManager.Instance.PlayerVictories[player] >= 1)
    {
        Manager matchMenu = new Manager();
        matchMenu.MatchSetNavigation();
    }

    else
    {
        MainGameManager.Instance.Rounds++;
        MainGameManager.Instance.PlayerVictories[player]++;
        // isMatchOver = false;
        if (MainGameManager.Instance.ActivePlayers == 2)
            SceneManager.LoadScene("Multiplayer");
        else
            SceneManager.LoadScene("SinglePlayer");
    }

    }
        
    
}

    



