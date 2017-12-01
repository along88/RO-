using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Physics : MonoBehaviour
{
    [SerializeField]
    private float dashDelayLength;
    private WaitForSeconds dashDelay;
    protected Rigidbody rb;
    protected Player player;
    protected InputManager inputManager;
    protected Vector3 direction;
    [SerializeField]
    protected float fallMultipler;
    [SerializeField]
    protected float jumpHeight;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float knockBackDistance; //This may be better off as a Vector3
    [SerializeField]
    protected float lastAttack;
    [SerializeField]
    protected float moveDelay;
    protected float getUpDelay = 1.5f;
    protected WaitForSeconds wait;
    protected Vector3 defaultPosition;
    protected float defaultSpeed;
    [SerializeField]
    protected float dashSpeed = 0.0f;
    private void LateUpdate()
    {
        Jump();
        Gravity();
        AttackMovementRestriction();
        Hit();
        KnockedBack();
        Dash();
        UpdatePositon();
        UpdateRotation();
        RingOut();

    }

    private void AttackMovementRestriction()
    {
        if (player.IsGrounded)
        {
            if (!player.IsDefending && player.IsAttacking)
                lastAttack = Time.time;
        }
    }

    private void UpdatePositon()
    {
        if (CanMove() && player.IsGrounded || !player.IsKnockedBack)
        {
            if (!player.IsDefending && player.IsWalking)
                transform.position += inputManager.Movement(player.ID) * speed * Time.deltaTime;
        }


    }
    private void UpdateRotation()
    {
        if (!player.IsExhausted && !player.IsTaunting && inputManager.Movement(player.ID) != Vector3.zero)
            transform.forward = inputManager.Movement(player.ID);

            //rb.rotation = Quaternion.LookRotation(inputManager.Movement(player.ID));

    }
    private void Dash()
    {
        if (player.IsDashing && player.CanDash)
        {
           
            StartCoroutine("Dashing");
        }
           
        
    }
    private void Jump()
    {
        if (player.IsJumping && !player.IsKnockedBack)
            rb.velocity += (Vector3.up * jumpHeight) + (inputManager.Movement(player.ID) * speed);

    }
    private void Gravity()
    {
        if (rb.velocity.y < 0)
            rb.velocity += (-inputManager.Movement(player.ID) + Vector3.up) * UnityEngine.Physics.gravity.y * (fallMultipler - 1) * Time.deltaTime;
    }
    private void RingOut()
    {
        if (player.IsHypeHit)
            rb.velocity += player.Opponent.transform.forward * 30 * Time.time;

    }
    private void Hit()
    {
        if (player.IsHit)
        {
            
            StartCoroutine("HitKnockBack");
        }
           
    }
    private void KnockedBack()
    {
        if (player.IsKnockedBack)
            StartCoroutine("GetUp");
    }
    private void Push()
    {
        if (player.IsPushed)
            player.Opponent.transform.position += inputManager.Movement(player.Opponent.ID);
    }


    private IEnumerator GetUp()
    {

        float knockBackForce = 10.0f;
        player.transform.forward = -player.Opponent.HitDirection;
        rb.position += player.Opponent.HitDirection * knockBackForce * Time.deltaTime;
        yield return wait;
        player.IsKnockedBack = false;
        //player.CanMove = true;
        //  player.transform.eulerAngles = defaultPosition;
    }

    private IEnumerator HitKnockBack()
    {
        player.IsWalking = false;
        player.CanMove = false;

        float knockBackForce = 200.0f;
        player.transform.forward = -player.Opponent.HitDirection;
        rb.position += player.Opponent.HitDirection * knockBackForce * Time.deltaTime;
        WaitForSeconds delay = new WaitForSeconds(0.01f);
        yield return delay;
        player.IsHit = false;
        player.CanMove = true;
    }
    private IEnumerator Dashing()
    {

        //float knockBackForce = 10.0f;
        //player.transform.forward = -player.Opponent.HitDirection;
        //rb.position += player.Opponent.HitDirection * knockBackForce * Time.deltaTime;
        
        float dashSpeed_ = dashSpeed;
        rb.position += player.transform.forward * dashSpeed * Time.deltaTime;
        dashDelay = new WaitForSeconds(dashDelayLength);
        yield return dashDelay;
        player.IsDashing = false;
        player.CanDash = true;
        //player.CanMove = true;
        //  player.transform.eulerAngles = defaultPosition;
    }
    private bool CanMove()
    {
        if ((Time.time - player.LastSuccessfulAttack) >= moveDelay && !player.IsDashing && !player.IsKnockedBack && !player.IsTaunting && !player.Opponent.IsTaunting && !player.IsExhausted)
        {
            speed = defaultSpeed;
            player.CanMove = true;
            return true;
        }
        else
        {
            speed = 0.0f;
            player.CanMove = false;
            return false;
        }

    }
    protected void Initialize(float _speed, float _fallMultipler)
    {
        if (_speed <= 0)
            _speed = defaultSpeed;
        if (_fallMultipler <= 0.0f)
            _fallMultipler = 2.5f;
        speed = _speed;
        fallMultipler = _fallMultipler;
    }
}