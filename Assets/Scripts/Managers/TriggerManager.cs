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
            opponentDefenseHitbox = "BlockArea" + player.Opponent.ID.ToString();
            opponentsHitbox = "Body" + player.Opponent.ID.ToString();
        }

        private void OnTriggerEnter(Collider other)
        {
            ActivateTriggers(other);
        }

        protected abstract void ActivateTriggers(Collider opponentHitbox);
         
    }

