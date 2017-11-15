using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    //Required Components
    private Player player;
    private InputManager inputManager;
    //Jump Controls
    private float lastJump;
    [SerializeField]
    private float jumpDelay;
    private WaitForSeconds resetJump = new WaitForSeconds(0.0f);

    // Use this for initialization
    void Awake ()
    {
        player = GetComponent<Player>();
        inputManager = GetComponent<InputManager>();
    }
    // Update is called once per frame
    void Update ()
    {
       // Move();
        //Dash();
        Jump();
    }
    private void Move()
    {
        if (player.IsGrounded && !player.IsDashing && !player.IsAttacking && player.CanMove && Time.timeScale != 0.0f)
        {
            if (inputManager.Movement(player.ID) != Vector3.zero)
                player.IsWalking = true;

            if (inputManager.Movement(player.ID) == Vector3.zero)
                player.IsWalking = false;
        }
        else if (!player.IsGrounded && inputManager.Movement(player.ID) != Vector3.zero)
            player.IsWalking = false;
    }
    private void Jump()
    {
        if (player.IsGrounded && CanJump() && !player.IsExhausted)
        {
            if (inputManager.JumpButtonDown(player.ID))
            {
                lastJump = Time.time;
                player.IsJumping = true;
                StartCoroutine("JumpReset");
            }
        }
    }
    private bool CanJump()
    {
        if ((Time.time - lastJump) >= jumpDelay && Time.timeScale != 0.0f && !player.IsTaunting && !player.Opponent.IsTaunting)
            return true;
        else
            return false;
    }
    private void Dash()
    {
        if (player.IsGrounded && !player.IsKnockedBack && player.CanDash && !player.IsDefending && !player.IsTaunting && !player.IsAttacking && !player.IsWalking && Time.timeScale != 0.0f)
        {
            if (inputManager.DashButton(player.ID) && player.AttackCounter == 0)
            {
                player.IsDashing = true;
            }
        }
    }
    private IEnumerator JumpReset()
    {
        yield return resetJump;
        player.IsJumping = false;
    }
}
