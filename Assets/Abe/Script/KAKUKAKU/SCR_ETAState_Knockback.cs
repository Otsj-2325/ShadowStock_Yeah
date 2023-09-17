using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_ETAState_Knockback : SCR_StateBase
{
    public override void OnEnter()
    {
        ;
    }

    public override void OnExit()
    {
        ;
    }

    public override int StateUpdate()
    {
        return (int)m_StateType;
    }
}
