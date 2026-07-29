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
    [Header("Audio Sources")]
    [SerializeField]
    private AudioSource musicSource;

    [Header("Music")]
    [SerializeField]
    private AudioClip stageTheme;

    [SerializeField]
    public AudioClip playerOneTheme;

    [SerializeField]
    public AudioClip playerTwoTheme;

    private float BGMLastTime;
    private bool matchThemeStarted;

    private bool playerOneWasHyped;
    private bool playerTwoWasHyped;
    //private bool matchThemeStarted;

    private void Awake()
    {
        if (musicSource == null)
            musicSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (stageTheme == null)
            stageTheme = musicSource.clip;

        GetHypeMusic();
    }

    private void Update()
    {
        if (MainGameManager.Instance == null)
            return;

        if (MainGameManager.Instance.IsMatchSetOver)
        {
            MatchSet();
            return;
        }

        PlayHypeMusic();
        StageTheme();
    }

    private void MatchSet()
    {
        if (matchThemeStarted)
            return;

        AudioClip victoryTheme;

        if (MainGameManager.Instance.WinningPlayerId == 1)
        {
            victoryTheme = playerOneTheme;
        }
        else if (MainGameManager.Instance.WinningPlayerId == 2)
        {
            victoryTheme = playerTwoTheme;
        }
        else
        {
            Debug.LogError(
                $"Invalid winning player ID: " +
                $"{MainGameManager.Instance.WinningPlayerId}",
                this
            );

            return;
        }

        Debug.Log($"Playing set victory theme: {victoryTheme.name}");

        SetPlayerTheme(victoryTheme, true);
        matchThemeStarted = true;
    }

    private void SetPlayerTheme(
    AudioClip newClip,
    bool restart = false
)
    {
        if (newClip == null)
        {
            Debug.LogWarning("Player theme is null.", this);
            return;
        }

        if (musicSource == null)
        {
            Debug.LogError("Music source is null.", this);
            return;
        }

        bool clipChanged = musicSource.clip != newClip;

        if (clipChanged || restart)
        {
            musicSource.Stop();
            musicSource.clip = newClip;
            musicSource.time = 0f;
        }

        musicSource.volume = 1f;
        musicSource.loop = true;

        if (!musicSource.isPlaying)
            musicSource.Play();
    }

    private void PlayHypeMusic()
    {
        bool playerOneIsHyped = players[0].IsHyped;
        bool playerTwoIsHyped = players[1].IsHyped;

        if (playerOneIsHyped && !playerOneWasHyped)
        {
            SetPlayerTheme(playerOneTheme, true);
        }
        else if (playerTwoIsHyped && !playerTwoWasHyped)
        {
            SetPlayerTheme(playerTwoTheme, true);
        }

        playerOneWasHyped = playerOneIsHyped;
        playerTwoWasHyped = playerTwoIsHyped;
    }

    private void StageTheme()
    {
        if (
            players[0].IsHyped ||
            players[1].IsHyped ||
            isPaused ||
            MainGameManager.Instance.IsMatchOver
        )
        {
            if (musicSource.clip == stageTheme)
                BGMLastTime = musicSource.time;

            return;
        }

        if (musicSource.clip != stageTheme)
        {
            musicSource.Stop();
            musicSource.clip = stageTheme;
            musicSource.volume = 0.5f;
            musicSource.loop = true;
            musicSource.time = BGMLastTime;
            musicSource.Play();
        }
        else if (!musicSource.isPlaying)
        {
            musicSource.volume = 0.5f;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    private void GetHypeMusic()
    {
        players = new Player[2];

        foreach (GameObject playerObject in GameObject.FindGameObjectsWithTag("Player"))
        {
            Player player = playerObject.GetComponent<Player>();
            AudioManager playerAudio = playerObject.GetComponent<AudioManager>();

            if (player == null || playerAudio == null)
                continue;

            if (player.ID == 1)
            {
                players[0] = player;
                playerOneTheme = playerAudio.hypeMusic;
            }
            else
            {
                players[1] = player;
                playerTwoTheme = playerAudio.hypeMusic;
            }
        }
    }
}