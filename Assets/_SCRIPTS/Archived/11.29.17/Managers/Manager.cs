using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager:MonoBehaviour
{
    protected Player[] players;
    private GameObject MatchSetMenuObject;
    private GameObject nav;
    private bool matchDefaultPosition;

    private GameObject pauseMenuObject;
    //BUTTONS
    private Button[] matchSetButtons;
    private Button[] pauseButtons;
    private Vector3 resumeButton;
    private Vector3 quitButton;

    protected bool isPlayerOneVictory;
    protected bool isPaused;
    protected bool isMatchOver;


    [SerializeField]
    private AudioSource menuSFX;
    [SerializeField]
    private AudioClip navChime;
    [SerializeField]
    private AudioClip navConfirm;
    protected AudioSource audioSource;

    private void Awake()
    {
        InitializeButtons();

    }

    private void InitializeButtons()
    {
        //Pause Menu
        pauseMenuObject = GameObject.FindGameObjectWithTag("ShowOnPause");
        pauseButtons = new Button[2];
        foreach (Button button in pauseMenuObject.GetComponentsInChildren<Button>())
            if (button.name.ToLower() == string.Format("resume"))
                pauseButtons[0] = button;
            else
                pauseButtons[1] = button;
        pauseMenuObject.SetActive(false);


        //Match Set Menu - make this it's own class that inherits from Menu
        MatchSetMenuObject = GameObject.FindGameObjectWithTag("MatchMenu");
        matchSetButtons = new Button[2];
        foreach (Button button in MatchSetMenuObject.GetComponentsInChildren<Button>())
            if (button.name.ToLower() == string.Format("rematch"))
                matchSetButtons[0] = button;
            else
                matchSetButtons[1] = button;
        MatchSetMenuObject.SetActive(false);

        //Navigation Object
        nav = GameObject.FindGameObjectWithTag("Nav");
        nav.transform.position = (pauseButtons[0].transform.position - new Vector3(130, 0, 0));
        nav.SetActive(false);
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
            while (pauseMenuObject.activeSelf)
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
                nav.active = false;
                SceneManager.LoadScene("Main Menu");

                Time.timeScale = 1.0f;

            }
            else
            {
                if (isMatchOver)
                {
                    //rounds.ClearRounds();
                    MainGameManager.Instance.ClearRounds();
                    nav.active = false;
                    if (MainGameManager.Instance.ActivePlayers == 2)
                        SceneManager.LoadScene("Multiplayer");
                    else
                        SceneManager.LoadScene("SinglePlayer");
                }

                Time.timeScale = 1.0f;
                isPaused = false;
                pauseMenuObject.SetActive(false);
                nav.active = false;

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
                pauseMenuObject.SetActive(true);
                nav.active = true;
                StartCoroutine("PauseNavigation");
            }


        }
    }
    private float Navigation()
    {
        return Input.GetAxis("NavV2");
    }
    public void MatchSetNavigation()
    {

        MatchSetMenuObject.SetActive(true);
        nav.active = true;
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
    private bool ConfirmButton()
    {
        bool buttonPressed = new bool();
        if (Input.GetButtonDown("Attack1"))
            buttonPressed = Input.GetButtonDown("Attack1");
        if (Input.GetButtonDown("Attack2"))
            buttonPressed = Input.GetButtonDown("Attack2");
        return buttonPressed;
    }




}
