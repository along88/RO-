using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

    public abstract class TriggerManager : MonoBehaviour
    {
        public Player player;
        [SerializeField]
        protected string opponentDefenseHitbox;
        [SerializeField]
        protected string opponentsHitbox;
        [SerializeField]
        internal GameObject hitSFX;

        
        private void Start()
        {
            GetComponenets();
        }
    private void OnTriggerEnter(Collider hitbox)
        {
            ActivateTriggers(hitbox);
        }

        public void GetComponenets()
        {
            player = GetComponentInParent<Player>();
            opponentDefenseHitbox = string.Format("BlockArea{0}", player.Opponent.ID.ToString());
            opponentsHitbox = string.Format("Body{0}", player.Opponent.ID.ToString());
        }
        protected abstract void ActivateTriggers(Collider opponentHitbox);
         
    }

