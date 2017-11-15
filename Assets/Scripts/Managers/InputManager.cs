using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputManager : MonoBehaviour
{
    [SerializeField]
    protected int controlNo;
    public int ControlNo { get {return controlNo; } }

    public abstract float GetHorizontal(int playerID);
    public abstract float GetVertical(int playerID);
    public abstract Vector3 Movement(int playerID);
    public abstract bool AttackButtonDown(int playerID);
    public abstract bool DefendButton(int playerID);
    public abstract bool JumpButtonDown(int playerID);
    public abstract bool PauseButton();
    public abstract bool DashButton(int playerID);
}
