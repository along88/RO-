using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioSourceManager : Manager
{
    private float hypeMusicLastTime;
    private float BGMLastTime;
    [SerializeField]
    private AudioClip stageTheme;
    [SerializeField]
    public AudioClip playerOneTheme;
    [SerializeField]
    public AudioClip playerTwoTheme;
    private AudioClip[] playersHypeTheme;
    [SerializeField]



    private void Awake()
    {
        stageTheme = GetComponent<AudioSource>().clip;
    }
    private void Update()
    {
        StageTheme();
        PlayHypeMusic();
        GetHypeMusic();
        MatchSet();
    }

    private void PlayHypeMusic()
    {
        if (isPaused && audioSource.clip == playerOneTheme || isPaused && audioSource.clip == playerTwoTheme)
            hypeMusicLastTime = audioSource.time;
        if (players[0].IsHyped)
            SetPlayerTheme(playerOneTheme);
        else if (players[1].IsHyped)
            SetPlayerTheme(playerTwoTheme);

    }
    //Stage Theme
    private void StageTheme()
    {
        if (!players[0].IsHyped && !players[1].IsHyped && !isPaused && !isMatchOver)
        {
            if (BGMLastTime > 0.0f)
            {
                if (audioSource.clip != stageTheme)
                {
                    audioSource.volume = 0.5f;
                    audioSource.clip = stageTheme;
                    audioSource.time = BGMLastTime;
                    audioSource.Play();
                }
                else
                {
                    audioSource.volume = 0.5f;
                    if (!audioSource.isPlaying)
                        audioSource.Play();

                }
            }
            else if (!audioSource.isPlaying)
            {
                audioSource.volume = 0.5f;
                audioSource.Play();
            }
        }
        if (audioSource.clip == stageTheme && isMatchOver ||
            audioSource.clip == stageTheme && players[0].IsHyped ||
            audioSource.clip == stageTheme && players[1].IsHyped ||
            audioSource.clip == stageTheme && isPaused)
            BGMLastTime = audioSource.time;
    }
    private void SetPlayerTheme(AudioClip AudioClip)
    {
        audioSource.clip = AudioClip;
        audioSource.volume = 1.0f;
        if (hypeMusicLastTime > 0.0f)
        {
            audioSource.time = hypeMusicLastTime;
            if (!audioSource.isPlaying)
                audioSource.Play();
            hypeMusicLastTime = 0.0f;
        }
        else if (!audioSource.isPlaying)
            audioSource.Play();
    }
    private void GetHypeMusic()
    {
        players = new Player[2];

        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<Player>().ID == 1)
            {
                players[0] = player.GetComponent<Player>();
                playerOneTheme = player.GetComponent<AudioManager>().hypeMusic;
                //playersTheme[0] = player.GetComponent<AudioManager>();
            }
            else
            {
                players[1] = player.GetComponent<Player>();
                playerTwoTheme = player.GetComponent<AudioManager>().hypeMusic;
                // playersTheme[1] = player.GetComponent<AudioManager>();
            }

            //rounds = 0;

        }
    }
    private void MatchSet()
    {
        if (isMatchOver)
        {
            if (isPlayerOneVictory)
                audioSource.clip = playerOneTheme;
            else
                audioSource.clip = playerTwoTheme;
            StartCoroutine("MatchSetDelay");


            //Wait X seconds

        }
    }
}