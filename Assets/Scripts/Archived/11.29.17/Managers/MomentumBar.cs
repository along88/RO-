using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MomentumBar : MonoBehaviour
{
    private Player[] players; 
    private Slider momentumBar;
    [SerializeField]
    private bool isHyped; 
    private DamageType damage;
    private AudioManager[] playerAudioManager;
    private AudioSource playersTheme;
    [SerializeField]
    private AudioClip themeOne;
    [SerializeField]
    private AudioClip themeTwo;
    [SerializeField]
    private float startingValue;
    [SerializeField]
    private float hypeTimer;
    [SerializeField]
    private Text hypeText;
    [SerializeField]
    private bool isTimer;
    [SerializeField]
    private bool isPlayerOne;
    [SerializeField]
    private Image[] playersName;

    public bool IsHyped { get {return isHyped; } }
    public bool IsPlayerOne
    {
        get { return isPlayerOne; }
        set { isPlayerOne = value; }
    }


    private void Awake()
    {
        players = new Player[2];
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<Player>().ID == 1)
            {
                players[0] = player.GetComponent<Player>();
                playersName[0].sprite = players[0].GetComponent<Image>().sprite;
            }
                
            else
            {
                players[1] = player.GetComponent<Player>();
                playersName[1].sprite = players[1].GetComponent<Image>().sprite;
            }

        }
        momentumBar = GameObject.FindGameObjectWithTag("Slider").GetComponent<Slider>();
        damage = GameObject.FindGameObjectWithTag("Player").GetComponent<DamageType>(); // this needs to be dynamically assigned between both players damage type
        startingValue = 50.0f;
        momentumBar.value = startingValue;
        hypeText = gameObject.transform.GetChild(2).GetComponent<Text>();
        playersTheme = GetComponent<AudioSource>();
        playerAudioManager = new AudioManager[2];
        foreach (var theme in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (theme.GetComponent<Player>().ID == 1)
                playerAudioManager[0] = theme.GetComponent<AudioManager>();
            else
                playerAudioManager[1] = theme.GetComponent<AudioManager>();
        }
    }
    private void Update()
    {
        ActivePlayer();
        UpdateBar();
        IsMaxed();
        ResetMomentumBar();
    }

    private void ActivePlayer()
    {
        if (players[0].IsHit)
            isPlayerOne = false;
        else if (players[1].IsHit)
            isPlayerOne = true;
    }
    private void IsMaxed()
    {
            if (momentumBar.value == momentumBar.maxValue && !isHyped)
            {
                //playersTheme.clip = themeOne;
                //playersTheme.Play();
                hypeText.text = "Player One is HYPED!";
                players[0].IsTaunting = true;
                players[0].IsHyped = true;
            Debug.Log(players[0].name + "Is Hyped");
                isHyped = true;
              
            }
            else if (momentumBar.value == momentumBar.minValue && !isHyped)
            {
               // playersTheme.clip = themeTwo;
                //playersTheme.Play();
                hypeText.text = "Player Two is HYPED!";
                players[1].IsTaunting = true;
                players[1].IsHyped = true;
                isHyped = true;
            }
    }

    private void PlayerDamage()
    {
        if (isPlayerOne)
        {
            if ((momentumBar.value + players[0].DamageType.CurrentDamage(damage.MinDamage, damage.MaxDamage)) > momentumBar.maxValue)
                momentumBar.value = momentumBar.maxValue;
            else
                momentumBar.value += players[0].DamageType.CurrentDamage(players[0].DamageType.MinDamage, players[0].DamageType.MaxDamage);
        }
        else 
        {
            if ((momentumBar.value + players[1].DamageType.CurrentDamage(damage.MinDamage, damage.MaxDamage)) < momentumBar.minValue)
                momentumBar.value = momentumBar.minValue;
            else
                momentumBar.value -= players[1].DamageType.CurrentDamage(players[1].DamageType.MinDamage, players[1].DamageType.MaxDamage);
        }


    }
    private void PlayerNegate()
    {
        if (isPlayerOne)
            momentumBar.value = momentumBar.value;
        else
            momentumBar.value = momentumBar.value;
    }

    public void UpdateBar()
    {
        if (players[0].IsDefending && players[0].IsHit || players[1].IsDefending && players[1].IsHit)
        {
            //float halfDamage = 2.0f;
            if (!isTimer)
                PlayerNegate();
        }
        else if (players[0].IsHit || players[1].IsHit)
        {
            if (!isTimer)
                PlayerDamage();
        }
        
    }
    public void ResetMomentumBar()
    {
        if (isHyped)
        {

            if (!players[0].IsHyped && !players[1].IsHyped || momentumBar.value == startingValue)
            {
                isHyped = false;
                isTimer = false;
                hypeText.text = "";
                momentumBar.value = startingValue;
                players[0].IsHyped = false;
                players[1].IsHyped = false;
                //playersTheme.Stop();
                
            }
            else if (players[0].IsHyped && !players[0].IsTaunting)
            {
                isTimer = true;
                momentumBar.value = Mathf.MoveTowards(momentumBar.value, startingValue, Time.deltaTime * hypeTimer);
                playerAudioManager[0].FadeHypeMusic((momentumBar.value - startingValue) / 50);
                
            }
            else if (players[1].IsHyped && !players[1].IsTaunting)
            {
                isTimer = true;
                momentumBar.value = Mathf.MoveTowards(momentumBar.value, startingValue, Time.deltaTime * hypeTimer);
                playerAudioManager[1].FadeHypeMusic((momentumBar.value + startingValue) / 50);
            }
        }
        else
        {
            if (players[0].IsHyped || players[1].IsHyped)
            {
                players[0].IsHyped = false;
                players[1].IsHyped = false;
               
            }
        }
    }
    

}
