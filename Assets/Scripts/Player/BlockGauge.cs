using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockGauge : MonoBehaviour
{
    private InputManager inputManager;
    private Player player;
    public float blockGauge;
    private Slider gaugeSlider;
    private Image jammerText;
    private bool canBlock;
    private bool isRecharging;
    

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        player = GetComponent<Player>();
        gaugeSlider = GameObject.FindGameObjectWithTag("gaugeSlider"+player.ID).GetComponent<Slider>();
        gaugeSlider.value = gaugeSlider.maxValue;
        canBlock = true;
        jammerText = gaugeSlider.GetComponentsInChildren<Image>()[2];
        jammerText.enabled = false;
    }
    void Update()
    {
        
        BlockGaugeSlider();
        DashGaugeDrain();
        GaugeRecharge();
        RechargeEcho();
    }

    //private void Block()
    //{
    //    if (player.IsGrounded && !player.IsAttacking && !player.IsTaunting && !player.IsExhausted && !player.IsKnockedBack)
    //    {
    //        if (Time.timeScale != 0.0f && inputManager.DefendButton(player.ID))
    //            player.IsDefending = true;
    //        else if (!inputManager.DefendButton(player.ID))
    //            player.IsDefending = false;
    //    }
    //}

    private void BlockGaugeSlider()
    {
        if (player.IsDefending && player.CanBlock)
        {
           
            gaugeSlider.value--;
            if(gaugeSlider.value <= gaugeSlider.minValue)
            {
                gaugeSlider.value = gaugeSlider.minValue;
                player.CanBlock = false;
                isRecharging = true;
            }
        }
        



    }

    private void GaugeRecharge()
    {
        
         if (gaugeSlider.value <= gaugeSlider.minValue)
        {
            player.CanBlock = false;
            player.CanDash = false;
            isRecharging = true;
            jammerText.enabled = true;
            if (player.IsDefending || player.IsDashing)
            {
                player.IsDefending = false;
                player.IsDashing = false;
            }
            gaugeSlider.value = Mathf.MoveTowards(gaugeSlider.minValue, gaugeSlider.maxValue, Time.deltaTime);
            

        }
    }

    private void DashGaugeDrain()
    {

        
        if (player.IsDashing)
        {

            gaugeSlider.value -= 2.5f;
            if (gaugeSlider.value <= gaugeSlider.minValue)
            {
                gaugeSlider.value = gaugeSlider.minValue;
                player.CanDash = false;
                
            }
        }
        
    }
    private void RechargeEcho()
    {
        if (!player.IsDefending && !player.IsDashing)
        {
            gaugeSlider.value++;
            if (gaugeSlider.value >= gaugeSlider.maxValue)
            {
                gaugeSlider.value = gaugeSlider.maxValue;
                player.CanBlock = true;
                player.CanDash = true;
                jammerText.enabled = false;
                
            }
        }
    }

}
