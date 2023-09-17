using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_ETAState_Find : SCR_StateBase {
    [SerializeField] private SCR_ETABrain scr_Brain;
    [SerializeField] private SCR_ETAMove scr_Move;

    private Vector3 m_FindPos;
    private bool m_MoveZ, m_MoveX;

    public override void OnEnter()
    {
        m_FindPos = scr_Brain.SearchPlayerPos();
        m_MoveX = false;
        m_MoveZ = true;
        var pos = this.transform.position;
        pos.z = m_FindPos.z;
        scr_Move.SetTargetPos(pos);
    }

    public override void OnExit()
    {
        ;
    }

    public override int StateUpdate()
    {
        if (m_MoveZ)
        {
            if (scr_Move.IsArrival())
            {
                m_MoveZ = false;
                m_MoveX = true;
                var pos = this.transform.position;
                pos.x = m_FindPos.x;
                scr_Move.SetTargetPos(pos);
            }
        }

        if (m_MoveX)
        {
            if (scr_Move.IsArrival())
            {
                m_MoveX = false;
                return (int)ETAStateType.Chase;
            }
        }

        return (int)m_StateType;
    }
}
