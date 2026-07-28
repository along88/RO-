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
    public Button[] matchSetButtons;
    private Vector3 cameraPosition;
    //private AudioSourceManager asm;


    private Camera matchSetcamera;
    private Camera mainCamera;
    private Image uiTime;
    private GameObject MatchSetMenuObject;
    //private GameObject pauseMenuObject;

    [SerializeField]
    private float matchTimer;
    [SerializeField]
    private Text matchTimerText;
    //[SerializeField]
    //private Rounds rounds;
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

    //private AudioSourceManager asm;


    [SerializeField]
    private GameObject[] prefabs;

    public GameObject[] FighterModel;
    public float Match { get { return match; } set { match = value; } }
    private bool matchDefaultPosition;

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
        // InitializeButtons();
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
            foundPlayers.Add(player.GetComponent<Player>());

        players = foundPlayers.ToArray();
        MainGameManager.Instance.ActivePlayers = players.Count();
        
        //pauseMenu = new Manager();

}
    private void Start()
    {
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
                //asm.playerOneTheme = player.GetComponent<AudioManager>().hypeMusic;
                playersTheme[0] = player.GetComponent<AudioManager>().hypeMusic;
            }
            else
            {
                players[1] = player.GetComponent<Player>();
                //asm.playerTwoTheme = player.GetComponent<AudioManager>().hypeMusic;
                playersTheme[1] = player.GetComponent<AudioManager>().hypeMusic;
            }

            //rounds = 0;

        }
    }
    private void Update()
    {
        RoundTimer();
        PauseMenu();
        RingOutVictory();
        if (players[0].IsHyped)
        {
            StartCoroutine("HypeTaunt",0); 
        }
        //AudioSourceManager audio = new AudioSourceManager();

    }
        
    private void InitializeComponents()
    {
        //stageTheme = GetComponent<AudioSource>().clip;
        //asm = GetComponent<AudioManager>();
        pauseMenu = GameObject.FindGameObjectWithTag("ShowOnPause");
        MatchSetMenuObject = GameObject.FindGameObjectWithTag("MatchMenu");
        nav = GameObject.FindGameObjectWithTag("Nav");
        nav.SetActive(false);
        MatchSetMenuObject.SetActive(false);
        pauseMenu.SetActive(false);
        ringOut.enabled = false;
        audioSource = GetComponent<AudioSource>();
        menuSFX = GetComponent<AudioSource>();
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
        else
        {
            StartCoroutine("MatchSetDelay", 0);
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
            //audioSource.clip = playersTheme[0];
            StartCoroutine("VictoryTaunt", 0);
        }
        else if (!isPlayerOneVictory)
        {
            //audioSource.clip = playersTheme[1];
            StartCoroutine("VictoryTaunt", 1);
        }
    }
    IEnumerator VictoryTaunt(int player)
    {
        
        
        if (MainGameManager.Instance.PlayerVictories[player] < 1)
            audioSource.clip = playersTheme[player];
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
        
            MatchSetNavigation();
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
    IEnumerator HypeTaunt(int player)
    {
        audioSource.clip = playersTheme[player];
        audioSource.Play();
        while (players[player].IsHyped)
        {

            
            
            WaitForSeconds delay = new WaitForSeconds(2.0f);
            yield return delay;
        }



        //players[player].IsTaunting = true;

        //if (rounds.playerVictories[player] >= 1)
        //    StartCoroutine("MatchSetNavigation");
        

    }
    protected  void InitializeButtons()
    {
        //Match Set Menu - make this it's own class that inherits from Menu
        
        matchSetButtons = new Button[2];
        foreach (Button button in MatchSetMenuObject.GetComponentsInChildren<Button>())
            if (button.name.ToLower() == string.Format("rematch"))
                matchSetButtons[0] = button;
            else
                matchSetButtons[1] = button;
        MatchSetMenuObject.SetActive(false);

        //Pause Menu
        //pauseMenuObject = GameObject.FindGameObjectWithTag("ShowOnPause");
        pauseButtons = new Button[2];
        foreach (Button button in pauseMenu.GetComponentsInChildren<Button>())
            if (button.name.ToLower() == string.Format("resume"))
                pauseButtons[0] = button;
            else
                pauseButtons[1] = button;
        pauseMenu.SetActive(false);




        //Navigation Object
        
        nav.transform.position = (pauseButtons[0].transform.position - new Vector3(130, 0, 0));
        nav.SetActive(false);
    }
    public void MatchSetNavigation()
    {

        MatchSetMenuObject.SetActive(true);
        nav.SetActive(true);
        if (!matchDefaultPosition)
        {
            nav.transform.position = (matchSetButtons[0].transform.position - new Vector3(100, 15.0f, 0));
            matchDefaultPosition = true;
        }

        var text = MatchSetMenuObject.GetComponentInChildren<Text>();
        if (isPlayerOneVictory)
        {
            players[1].gameObject.SetActive(false);
            text.text = string.Format(players[0].name + " Wins!");
        }
        else
        {
            players[0].gameObject.SetActive(false);
            text.text = string.Format(players[1].name + " Wins!");
        }


        Time.timeScale = 0.01f;
        StartCoroutine("PauseNavigation");

    }
    IEnumerator PauseNavigation()
    {
        if (isMatchOver)
        {
            while (MatchSetMenuObject.activeSelf)
            {
                float pauseEndTime = Time.realtimeSinceStartup + 1f;
                while (Time.realtimeSinceStartup < pauseEndTime)
                {

                    yield return 0;
                    PauseControls();

                }

            }
        }
        else
        {
            while (pauseMenu.activeSelf)
            {
                float pauseEndTime = Time.realtimeSinceStartup + 1f;
                while (Time.realtimeSinceStartup < pauseEndTime)
                {

                    yield return 0;
                    PauseControls();

                }

            }
        }
    }
    private void PauseControls()
    {


        if (isMatchOver)
        {

            Debug.Log(matchSetButtons[0].name);
            resumeButton = (matchSetButtons[0].transform.position - new Vector3(130, 10.0f, 0));
            quitButton = (matchSetButtons[1].transform.position - new Vector3(150, 1, 0));

            //nav.transform.parent = MatchSetMenuObject.transform;
        }
        else
        {
            resumeButton = (pauseButtons[0].transform.position - new Vector3(110, 0, 0));
            quitButton = (pauseButtons[1].transform.position - new Vector3(110, 0, 0));
        }

        if (Navigation() > 0.0f)
        {
            menuSFX.clip = navChime;
            if (nav.transform.position != resumeButton)
            {
                nav.transform.position = resumeButton;
                Debug.Log("Nav Moved!");
                menuSFX.Play();
            }

        }
        else if (Navigation() < 0.0f)
        {
            menuSFX.clip = navChime;
            if (nav.transform.position != quitButton)
            {
                nav.transform.position = quitButton;
                menuSFX.Play();
            }

        }
        else if (ConfirmButton())
        {

            menuSFX.clip = navConfirm;
            if (isPaused)
                menuSFX.Play();


            if (nav.transform.position == quitButton)
            {
                MainGameManager.Instance.ClearRounds();
                nav.SetActive(false);
                SceneManager.LoadScene("Main Menu");

                Time.timeScale = 1.0f;

            }
            else
            {
                if (isMatchOver)
                {
                    //rounds.ClearRounds();
                    MainGameManager.Instance.ClearRounds();
                    nav.SetActive(false);
                    if (MainGameManager.Instance.ActivePlayers == 2)
                        SceneManager.LoadScene("Multiplayer");
                    else
                        SceneManager.LoadScene("SinglePlayer");
                }

                Time.timeScale = 1.0f;
                isPaused = false;
                pauseMenu.SetActive(false);
                nav.SetActive(false);

            }
        }

    }
    private bool PauseButton()
    {
        return Input.GetButtonDown("Submit");
    }
    public void PauseMenu()
    {
        if (PauseButton() && !isMatchOver)
        {
            if (!isPaused)
            {

                Time.timeScale = 0.0f;
                isPaused = true;
                audioSource.Pause();
                pauseMenu.SetActive(true);
                nav.SetActive(true);
                StartCoroutine("PauseNavigation");
            }


        }
    }
    private float Navigation()
    {
        return Input.GetAxis("NavV2");
    }


}

    



