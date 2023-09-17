using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_ETAState_Patrol : SCR_StateBase
{
    [SerializeField] private SCR_ETAMove scr_Move;
    [SerializeField] private List<Transform> m_PatrolPoints = new List<Transform>();

    private int m_Num = 0;

    public override void OnEnter()
    {
        scr_Move.SetTargetPos(m_PatrolPoints[m_Num].position);
    }

    public override void OnExit()
    {
        m_Num = 0;
    }

    public override int StateUpdate()
    {
        if (scr_Move.IsArrival())
        {
            ++m_Num;
            if(m_Num > m_PatrolPoints.Count) { m_Num = 0; }

            scr_Move.SetTargetPos(m_PatrolPoints[m_Num].position);
        }

        return (int)m_StateType;
    }
}
