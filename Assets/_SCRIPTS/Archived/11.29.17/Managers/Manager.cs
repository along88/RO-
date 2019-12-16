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
    [SerializeField]
    protected Player[] players;
   
    protected GameObject nav;
    

   
    //BUTTONS
   
    protected Button[] pauseButtons;
    protected Vector3 resumeButton;
    protected Vector3 quitButton;

    protected bool isPlayerOneVictory;
    protected bool isPaused;
    protected bool isMatchOver;


    [SerializeField]
    protected AudioSource menuSFX;
    [SerializeField]
    protected AudioClip navChime;
    [SerializeField]
    protected AudioClip navConfirm;
    protected AudioSource audioSource;

    private void Start()
    {
        players= new Player[2];
    }







    protected bool ConfirmButton()
    {
        bool buttonPressed = new bool();
        if (Input.GetButtonDown("Attack1"))
            buttonPressed = Input.GetButtonDown("Attack1");
        if (Input.GetButtonDown("Attack2"))
            buttonPressed = Input.GetButtonDown("Attack2");
        return buttonPressed;
    }




}
