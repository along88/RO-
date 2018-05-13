using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

    public abstract class TriggerManager : MonoBehaviour
    {
        protected Player player;
        [SerializeField]
        protected string opponentDefenseHitbox;
        [SerializeField]
        protected string opponentsHitbox;

        private void Start()
        {
            player = GetComponentInParent<Player>();
            opponentDefenseHitbox = string.Format( "BlockArea{0}",player.Opponent.ID.ToString());
            opponentsHitbox = string.Format("Body{0}",player.Opponent.ID.ToString());
        }

        private void OnTriggerEnter(Collider hitbox)
        {
            ActivateTriggers(hitbox);
        }

        protected abstract void ActivateTriggers(Collider opponentHitbox);
         
    }

