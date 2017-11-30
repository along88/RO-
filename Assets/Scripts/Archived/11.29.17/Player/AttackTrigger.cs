using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : TriggerManager
{
    [SerializeField]
    private static float lastHit;
    [SerializeField]
    private float maxHitCounter;
    [SerializeField]
    private BoxCollider attackCollider;
    private void Awake()
    {
        InitializeMaxCounter(maxHitCounter);
        attackCollider = GetComponent<BoxCollider>();
    }
    private void Update()
    {
        //if ((Time.time - lastHit) > 1.5f)
        //{
        //    maxHitCounter = 0;
        //}
    }
    protected override void ActivateTriggers(Collider hitbox)
    {
        if (hitbox.name == opponentDefenseHitbox || hitbox.name == opponentsHitbox)
        {
            player.HitDirection = player.transform.forward;
            if (!player.Opponent.IsDefending || isBackAttack())
            {
                //maxHitCounter++;
                //if (player.AttackCounter == 1 && maxHitCounter > 1)
                //{
                //    player.Opponent.IsKnockedBack = true;
                //    maxHitCounter = 0;
                //}
                //if (player.AttackCounter == 2)
                //{
                //    maxHitCounter = 0;
                //}
                if (player.AttackCounter == 3)
                {
                    player.Opponent.IsKnockedBack = true;
                    maxHitCounter = 0;
                }
            }
            player.Opponent.IsHit = true;
            //lastHit = Time.time;
            attackCollider.enabled = false;
        }
        
    }

    /// <summary>
    /// Need to implement logic for checking if the opponent back is towards us at moment of attack registering here
    /// </summary>
    /// <returns></returns>
    private bool isBackAttack()
    {
        Debug.Log(Vector3.Dot(player.transform.position, player.Opponent.transform.position).ToString());
        //if (Vector3.Dot(player.transform.position, player.Opponent.transform.position) != 35.0f)
        if (!player.Opponent.IsDefending)
            return true;
        else
            return false;
        
    }
    private void InitializeMaxCounter(float _maxHitCounter)
    {
        if (_maxHitCounter == 0 )
            _maxHitCounter = 0;
        maxHitCounter = _maxHitCounter;
    }

}
