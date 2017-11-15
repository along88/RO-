using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanInput : InputManager {


    public override float GetHorizontal(int playerID)
    {
        return Input.GetAxis("Horizontal" + playerID.ToString());
    }
    public override float GetVertical(int playerID)
    {
        return Input.GetAxis("Vertical" + playerID.ToString());
    }
    public override Vector3 Movement(int playerID)
    {
        return new Vector3( GetHorizontal(playerID), 0, GetVertical(playerID));
    }
    public override bool AttackButtonDown(int playerID)
    {
        return Input.GetButtonDown("Attack" + playerID.ToString());
    }
    public override bool DefendButton(int playerID)
    {
        return Input.GetButton("Block" + playerID.ToString());
    }
    public override bool JumpButtonDown(int playerID)
    {
        return Input.GetButtonDown("Jump" + playerID.ToString());
    }

    public override bool PauseButton()
    {
        return Input.GetButtonDown("Submit");
    }

    public override bool DashButton(int playerID)
    {
        return Input.GetButtonDown("Dash"+ playerID.ToString());
    }
}
