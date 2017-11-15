using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TorsoTrigger : TriggerManager
{
    
    protected override void ActivateTriggers(Collider hitbox)
    {
        
        if(hitbox.name == opponentsHitbox)
            player.IsPushed = true;
        else
            player.IsPushed = false;
    }

}

