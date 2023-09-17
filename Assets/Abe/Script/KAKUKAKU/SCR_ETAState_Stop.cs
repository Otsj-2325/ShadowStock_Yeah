using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_ETAState_Stop : SCR_StateBase
{
    [SerializeField] private SCR_ETAMove scr_Move;

    public override void OnEnter()
    {
        scr_Move.SetStop(true);
    }

    public override void OnExit()
    {
        scr_Move.SetStop(false);
    }

    public override int StateUpdate()
    {
        return (int)m_StateType;
    }
}
