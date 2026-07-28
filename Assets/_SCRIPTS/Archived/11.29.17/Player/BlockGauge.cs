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
    [SerializeField] private float gaugeRechargeRate = 25f;
    private Coroutine gaugeRechargeCoroutine;

    public bool IsRecharging
    {
        get { return isRecharging; }
    }

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        player = GetComponent<Player>();
       
        canBlock = true;
        
    }
    private void Start()
    {
        if(player.ID == 1)
        gaugeSlider = GameObject.FindGameObjectWithTag("gaugeSlider1").GetComponent<Slider>();
        else
            gaugeSlider = GameObject.FindGameObjectWithTag("gaugeSlider2").GetComponent<Slider>();
        gaugeSlider.value = gaugeSlider.maxValue;
        jammerText = gaugeSlider.GetComponentsInChildren<Image>()[2];
        jammerText.enabled = false;
    }
    void Update()
    {
        
        BlockGaugeSlider();
        DashGaugeDrain();
        //BlockRecharge();
        if (gaugeSlider.value <= gaugeSlider.minValue && !isRecharging)
            StartGaugeRecharge();
        
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
    private void StartGaugeRecharge()
    {
        // Prevent multiple copies of the coroutine from running.
        if (gaugeRechargeCoroutine != null)
        {
            return;
        }

        gaugeRechargeCoroutine = StartCoroutine(GaugeRecharge());
    }
    private void BlockGaugeSlider()
    {
        if (player.IsDefending)
        {
           
            gaugeSlider.value--;
            if(gaugeSlider.value <= gaugeSlider.minValue)
            {
                gaugeSlider.value = gaugeSlider.minValue;
                player.CanBlock = false;
               
            }
        }
        



    }

    private void BlockRecharge()
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
    private IEnumerator GaugeRecharge()
    {
        isRecharging = true;

        player.CanBlock = false;
        player.CanDash = false;

        jammerText.enabled = true;

        // Cancel currently active actions.
        player.IsDefending = false;
        player.IsDashing = false;

        while (gaugeSlider.value < gaugeSlider.maxValue)
        {
                gaugeSlider.value = Mathf.MoveTowards(
                gaugeSlider.value,
                gaugeSlider.maxValue,
                gaugeRechargeRate * Time.deltaTime
            );
            player.CanBlock = false;
            player.CanDash = false;

            // Pause here and continue on the following frame.
            yield return null;
        }

        gaugeSlider.value = gaugeSlider.maxValue;

        isRecharging = false;

        player.CanBlock = true;
        player.CanDash = true;

        jammerText.enabled = false;
        gaugeRechargeCoroutine = null;
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
