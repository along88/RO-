using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GamePhysics : MonoBehaviour
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
    [SerializeField]
    private float dashDistance = 15f;
    [SerializeField]
    private string dashAnimationTag = "Dash";
    private Coroutine dashCoroutine;
    private Animator animator;
    [SerializeField]
    private float dashAnimationTimeout = 0.5f;
    [SerializeField]
    private float dashMoveDuration = 0.15f;
    private void Awake()
    {
        
        
    }

    private void Start()
    {
        defaultSpeed = speed;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        inputManager = GetComponent<InputManager>();

    }
    private void LateUpdate()
    {
        Jump();
        Gravity();
        AttackMovementRestriction();
        Hit();
        KnockedBack();
        //Dash();
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
        if ( !player.IsKnockedBack &&!player.IsExhausted && !player.IsTaunting && inputManager.Movement(player.ID) != Vector3.zero)
            transform.forward = inputManager.Movement(player.ID);

            //rb.rotation = Quaternion.LookRotation(inputManager.Movement(player.ID));

    }
    //private void Dash()
    //{
    //    TryDash();



    //}
    public bool TryDash()
    {
        // A dash coroutine is already running.
        if (dashCoroutine != null)
        {
            return false;
        }

        // Centralized dash eligibility check.
        if (!player.CanDash ||
            player.IsDashing ||
            !player.IsGrounded ||
            player.IsKnockedBack ||
            player.IsDefending ||
            player.IsTaunting ||
            player.IsAttacking ||
            player.IsExhausted ||
            player.AttackCounter != 0 ||
            Time.timeScale == 0.0f)
        {
            return false;
        }

        dashCoroutine = StartCoroutine(Dashing());
        return true;
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
        var HitDirection = player.Opponent.HitDirection;
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
        var HitDirection = player.Opponent.HitDirection;
        speed = 0f;
        player.IsWalking = false;
        player.CanMove = false;

        float knockBackForce = 200.0f;
        player.transform.forward = -HitDirection;
        rb.position += HitDirection * knockBackForce * Time.deltaTime;
        WaitForSeconds delay = new WaitForSeconds(0.01f);
        yield return delay;
        player.IsHit = false;
        player.CanMove = true;
        speed = defaultSpeed;
    }
    private IEnumerator Dashing()
    {
        player.IsDashing = true;
        player.IsWalking = false;
        player.CanMove = false;

        Vector3 startPosition = rb.position;
        Vector3 dashDirection = transform.forward.normalized;
        Vector3 targetPosition =
            startPosition + dashDirection * dashDistance;

        // AnimationManager sees IsDashing and plays "Dash".
        float timeout = dashAnimationTimeout;

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Dash"))
        {
            timeout -= Time.deltaTime;

            if (timeout <= 0.0f)
            {
                Debug.LogWarning(
                    gameObject.name +
                    " did not enter the Dash animation state."
                );

                FinishDash();
                yield break;
            }

            yield return null;
        }

        // Move the fixed dash distance over dashMoveDuration.
        float elapsedTime = 0.0f;

        while (elapsedTime < dashMoveDuration)
        {
            elapsedTime += Time.fixedDeltaTime;

            float dashProgress = Mathf.Clamp01(
                elapsedTime / dashMoveDuration
            );

            Vector3 nextPosition = Vector3.Lerp(
                startPosition,
                targetPosition,
                dashProgress
            );

            rb.MovePosition(nextPosition);

            yield return new WaitForFixedUpdate();
        }

        // Ensure the full distance is reached.
        rb.MovePosition(targetPosition);

        /*
         * Movement has finished, but keep IsDashing true until
         * the animation finishes. This prevents another dash
         * during the remaining animation frames.
         */
        while (true)
        {
            AnimatorStateInfo dashState =
                animator.GetCurrentAnimatorStateInfo(0);

            if (!dashState.IsName("Dash") ||
                dashState.normalizedTime >= 1.0f)
            {
                break;
            }

            yield return null;
        }

        FinishDash();
    }
    private void FinishDash()
    {
        player.IsDashing = false;
        player.CanMove = true;

        dashCoroutine = null;
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