﻿using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MainMenu : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip navChime;
    [SerializeField]
    private AudioClip navConfirm;
    [SerializeField]
    private Button multiplayerButton;
    [SerializeField]
    private Button quitButton;
    [SerializeField]
    private GameObject nav;
    [SerializeField]
    private Vector2 defaultPosition;

    private void Awake()
    {
        nav.transform.position = (multiplayerButton.transform.position - new Vector3(175, 0, 0));
        audioSource = GetComponent<AudioSource>();
        
    }
    private void Update()
    {
        MenuControls();
    }

    private Vector2 SetDefaultPosition(Vector2 _defaultPosition)
    {

        return defaultPosition;
    }
    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void MenuControls()
    {
        var multiplayer = (multiplayerButton.transform.position - new Vector3(175, 0, 0));
        var quit = (quitButton.transform.position - new Vector3(175, 0, 0));


        if (Navigation() > 0.0f || Navigation2() > 0.0f)
        {
            audioSource.clip = navChime;
            if(nav.transform.position != multiplayer)
            {
                audioSource.Play();
                nav.transform.position = multiplayer;
            }

        }
        else if (Navigation() < 0.0f || Navigation2() < 0.0f)
        {
            if (nav.transform.position != quit)
            {
                audioSource.Play();
                nav.transform.position = quit;
            }
        }
        else if (ConfirmButton())
        {
            audioSource.clip = navConfirm;
            if (audioSource.clip == navConfirm && !audioSource.isPlaying)
                audioSource.Play();

            if (nav.transform.position == quit)
            {

                QuitGame();
            }
            else if (nav.transform.position == multiplayer)
            {
                //LoadLevel("RingMap");
                MainGameManager.Instance.ActivePlayers = 2;
                SceneManager.LoadScene("CharacterSelectMenu");
            }
            
        }
    }

    private float Navigation2()
    {
        return Input.GetAxis("NavV2");
    }

    private float Navigation()
    {
        
            return Input.GetAxis("NavV1");
        
          


    }
    private bool ConfirmButton()
    {
        bool buttonPressed = false;
        if (Input.GetButtonDown("Attack1"))
            buttonPressed = Input.GetButtonDown("Attack1");
        if (Input.GetButtonDown("Attack2"))
            buttonPressed = Input.GetButtonDown("Attack2");
        return buttonPressed;
    }
}
