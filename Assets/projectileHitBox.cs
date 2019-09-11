using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileHitBox : TriggerManager
{
    [SerializeField]
    private Collider Hitbox;
    public string parentTag;
    [SerializeField]
    private float speed;

    private void Start()
    {
        GetComponenets();
    }
    private void Update()
    {
        this.transform.position += Vector3.forward * speed * Time.deltaTime;

    }

    protected override void ActivateTriggers(Collider hitbox)
    {
        if (hitbox.name == opponentDefenseHitbox || hitbox.name == opponentsHitbox)
        {
            player.HitDirection = player.transform.forward;
            var cols = Physics.OverlapBox(Hitbox.bounds.center, Hitbox.bounds.extents, Hitbox.transform.rotation, LayerMask.GetMask("Hitbox"));

            foreach (var _hitbox in cols)
            {

                if (_hitbox.transform.parent.parent.tag != parentTag)
                {
                    Debug.Log(_hitbox.transform.parent.parent.tag + " hit!");

                }
            }
        }
    }
}
