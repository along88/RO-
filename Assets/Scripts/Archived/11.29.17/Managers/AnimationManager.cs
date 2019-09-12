using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationManager : MonoBehaviour
{

    private Animator anim;
    private Player player;
    private WaitForSeconds jumpDelay = new WaitForSeconds(0.5f);
    private InputManager inputManager;
    [SerializeField]
    private float attackDelay;
    [SerializeField]
    private float resetDelay;
    [SerializeField]
    private WaitForSeconds hypeDelay;
    [SerializeField]
    private float attackDelayMultiplier;
    public bool canAttack
    {
        get
            {
            return CanAttack();
            }
        
     }

    private void Start()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();
        inputManager = GetComponent<InputManager>();
        if(player.name == string.Format("Dukez"))
            hypeDelay = new WaitForSeconds(.5f);
        else
            hypeDelay = new WaitForSeconds(1.5f);
        

    }
    private void Update()
    {
        ResetAttackCounter();
        Dash();
        HypeTaunt();
        HypeAttack();
        Attack();
        IsFalling();
        Walk();
        Jump();
        Block();
        HypeHit();
        Exhausted();
    }
    private void LateUpdate()
    {
        IsKnockedBack();
        Hit();
    }

    private void IsFalling()
    {
        if (!player.IsHypeHit)
        {
            anim.SetBool("isGrounded", player.IsGrounded);
            if (!player.IsGrounded && anim.GetBool("isWalking"))
                player.IsWalking = false;
        }
    }
    private void Walk()
    {
        if (inputManager.Movement(player.ID) != Vector3.zero)
        {
            if (player.IsGrounded && player.AttackCounter <= 0 && !player.IsExhausted && !player.IsKnockedBack && !player.IsHypeAttack && !player.IsDefending && !player.IsTaunting && !player.IsDashing && player.CanMove && Time.timeScale != 0.0f)
            {
                anim.Play("Walking");
                player.IsWalking = true;
            }
        }
        else
            player.IsWalking = false;


    }
    private void Block()
    {
        //anim.SetBool("isBlocking", player.IsDefending);
        if (player.IsGrounded)
        { 
            if (player.CanBlock && inputManager.DefendButton(player.ID) && (Time.time - player.LastSuccessfulAttack) > 1.0f && !player.IsTaunting && !player.IsExhausted && !player.IsKnockedBack && !player.IsDashing)
            {
                anim.Play("Block");
                player.IsDefending = true;
            }
            else if (!inputManager.DefendButton(player.ID))
            {
                player.IsDefending = false;
            }
        }
        
    }
    private void Attack()
    {
        
            if (CanAttack() && inputManager.AttackButtonDown(player.ID))
            {
                 player.CanBlock = false;
                player.CanMove = false;
                if (!player.IsHyped && !player.IsDefending && !player.IsDashing)
                {
                   //player.IsAttacking = true;
                    AttackManager();
                
                }
             }
        
    }
    private void IsKnockedBack()
    {
        if (player.IsKnockedBack)
           anim.Play("KnockBack");
        
    }
    private void HypeTaunt()
    {
        if (player.IsTaunting)
        {

            
            StartCoroutine("ResetTaunt");
        }
    }
    private void HypeAttack()
    {
        if ((inputManager.AttackButtonDown(player.ID)))
        {
            if (player.IsHyped && player.IsGrounded && !player.IsDashing && !player.IsKnockedBack && !player.IsTaunting && !player.IsDefending)
            {
                
                anim.Play("HypeAttack");
                player.IsHypeAttack = true;
                StartCoroutine("ResetHype");
            }
        }
           
    }
    private void Jump()
    {
        anim.SetBool("isJumping", player.IsJumping);
        StartCoroutine("ResetJump", player.IsJumping);
    }
    private void HypeHit()
    {
        // anim.SetBool("hypeHit", player.IsHypeHit);
        if (player.IsHypeHit)
        {
            anim.Play("HypeHit");
            StartCoroutine("ResetHypeHit");
        }
    }
    private void Hit()
    {
        if (player.IsHit)
        {
           
            anim.Play("Hit");
            //anim.PlayInFixedTime("Hit");

        }
    }
    private void Exhausted()
    {
        if (player.IsExhausted)
        {
            anim.Play("Exhausted");
            StartCoroutine("ExhaustReset");
        }
    }

    private void AttackManager()
    {

        if (player.IsGrounded && !player.IsDashing)
        {


            if (player.AttackCounter == 0)
            {
                player.IsAttacking = true;

                player.AttackCounter++;
                anim.Play("Attack");
                player.LastSuccessfulAttack = Time.time;
               if(player.name == string.Format("Marie(Clone)"))
                {
                    resetDelay = 2.0f;
                   // attackDelay = .1f;
                }
                else
                {

                    resetDelay = 0.8f;
                    var projectile = GetComponent<ProjectileSpawner>();
                    //need to pass which firePoint to use here
                    projectile.SpawnFX(0);

                }

            }
            else if (player.AttackCounter == 1)
            {
                player.IsAttacking = true;
                player.AttackCounter++;
                anim.Play("Attack2");
                player.LastSuccessfulAttack = Time.time;
                if (player.name == string.Format("Marie(Clone)"))
                {
                    resetDelay = 1.5f;
                   // attackDelay = 1.0f;
                }
                else
                {
                    resetDelay = 0.8f;
                    var projectile = GetComponent<ProjectileSpawner>();
                    projectile.SpawnFX(1);
                }

            }
            else if (player.AttackCounter == 2)
            {
                player.IsAttacking = true;
                player.AttackCounter++;
                anim.Play("Attack3");
                player.LastSuccessfulAttack = Time.time;
                if (player.name == string.Format("Marie(Clone)"))
                {
                    resetDelay = 1.5f;
                }
                else
                {
                    resetDelay = 0.6f;
                    var projectile = GetComponent<ProjectileSpawner>();
                    projectile.SpawnFX(1);
                }
                    

            }
            else if (player.AttackCounter >= 3)
            {
                player.IsAttacking = true;
                player.AttackCounter = 1;
                anim.Play("Attack");
                player.LastSuccessfulAttack = Time.time;
                if (player.name == string.Format("Marie(Clone)"))
                {
                    resetDelay = 2.0f;
                    // attackDelay = .1f;
                }
                else
                    resetDelay = 0.8f;
            }
            
            
        }
        else
        {
            anim.Play("JumpAttack");
           // player.LastSuccessfulAttack = (Time.time);
           // player.IsAttacking = true;
            //attackDelay = 0.0f;
        }

        }
    private void Dash()
    {
        if (inputManager.DashButton(player.ID) && !player.IsKnockedBack && !player.IsExhausted && !player.IsHypeAttack && player.CanDash && !player.IsTaunting && player.AttackCounter == 0)
        {
            player.IsDashing = true;
            anim.Play("Dash");
        }
       
    }
    private bool CanAttack()
    {

        if ((Time.time - player.LastSuccessfulAttack) >= attackDelay && Time.timeScale != 0.0f && !player.IsKnockedBack && !player.Opponent.IsTaunting)
            return true;
        
        else
        {
            player.IsAttacking = false;
            return false;

        }
        
   }
    private IEnumerator ResetJump(bool value)
    {
        yield return jumpDelay;
        value = false;
    }
    //private IEnumerator ResetHit( )
    //{
    //    yield return null;
    //    player.IsHit = false;
    //}
    private IEnumerator ExhaustReset()
    {
        WaitForEndOfFrame exhaustWait = new WaitForEndOfFrame();
        WaitForSeconds waitForSeconds = new WaitForSeconds(2.0f);
        yield return waitForSeconds;
        player.IsExhausted = false;
    }

    private float ResetAttackCounter()
    {

        if ((Time.time - player.LastSuccessfulAttack) >= resetDelay)
            player.AttackCounter = 0;
        return player.AttackCounter;
    }
    private IEnumerator ResetHype()
    {
        yield return hypeDelay;
        player.IsHyped = false;
        player.IsExhausted = true;
        player.CanMove = true;
        player.IsHypeAttack = false;


    }
    private IEnumerator ResetTaunt()
    {
        player.Opponent.gameObject.active = false;
        anim.Play("HypeTaunt");
        WaitForSeconds delay = new WaitForSeconds(2.0f);
        yield return delay;
        player.IsTaunting = false;
        player.Opponent.gameObject.active = true;
        
    }
    private IEnumerator ResetHypeHit()
    {
        
        WaitForSeconds wait = new WaitForSeconds(2.0f);
        yield return wait;
        player.IsHypeHit = false;
    }



}