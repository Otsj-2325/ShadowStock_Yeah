using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETAStateType
{
    Patrol,
    Find,
    Chase,
    Knockback,
    Stop,
}

public class SCR_ETAStateController : SCR_StateControllerBase
{
    public override void Initialize(int initDarumaStateType)
    {
        m_StateDic[(int)ETAStateType.Patrol] = this.gameObject.GetComponent<SCR_ETAState_Patrol>();
        m_StateDic[(int)ETAStateType.Patrol].Initialize((int)ETAStateType.Patrol);

        m_StateDic[(int)ETAStateType.Find] = this.gameObject.GetComponent<SCR_ETAState_Find>();
        m_StateDic[(int)ETAStateType.Find].Initialize((int)ETAStateType.Find);

        m_StateDic[(int)ETAStateType.Chase] = this.gameObject.GetComponent<SCR_ETAState_Chase>();
        m_StateDic[(int)ETAStateType.Chase].Initialize((int)ETAStateType.Chase);
                       
        m_StateDic[(int)ETAStateType.Knockback] = this.gameObject.GetComponent<SCR_ETAState_Knockback>();
        m_StateDic[(int)ETAStateType.Knockback].Initialize((int)ETAStateType.Knockback);
                       
        m_StateDic[(int)ETAStateType.Stop] = this.gameObject.GetComponent<SCR_ETAState_Stop>();
        m_StateDic[(int)ETAStateType.Stop].Initialize((int)ETAStateType.Stop);

        m_CurrentState = initDarumaStateType;
        m_StateDic[m_CurrentState].OnEnter();
    }

    private void FixedUpdate()
    {
        ;
    }

    public void SetStateFind()
    {
        m_CurrentState = (int)ETAStateType.Find;
        m_StateDic[m_CurrentState].OnEnter();
    }

    public void SetStateStop()
    {
        m_CurrentState = (int)ETAStateType.Stop;
        m_StateDic[m_CurrentState].OnEnter();
    }

    public void SetStatePatrol()
    {
        m_CurrentState = (int)ETAStateType.Patrol;
        m_StateDic[m_CurrentState].OnEnter();
    }
}
