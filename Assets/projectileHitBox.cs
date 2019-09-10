using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileHitBox : MonoBehaviour
{
    [SerializeField]
    private Collider Hitbox;
   void Hit()
    {
        var cols = Physics.OverlapBox(Hitbox.bounds.center, Hitbox.bounds.extents, Hitbox.transform.rotation, LayerMask.GetMask("Hitbox"));

        foreach (var hitbox in cols)
        {

            if (hitbox.transform.parent.parent.tag != this.tag)
            {
                //var opponent = hitbox.transform.GetComponentInParent<CombatManager>();
                //opponent.Hit(Damage);
                //Instantiate(hitspark, Hitbox.transform.position, hitbox.transform.rotation);
                //hitConfirm = true;
                //if (this.tag == TagList.PLAYER)
                //    ActionSequenceManager.Initiate(this.gameObject, hitbox.transform.parent.parent.gameObject);

                //break;
            }
        }

    }
}
