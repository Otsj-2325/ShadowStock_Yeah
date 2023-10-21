using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PyonManager : MonoBehaviour
{
    public enum STATE { 
        Idle = 0,
        Chase,
        Contact,
        KnockBack,
    }


    [Header("Š´’m”ÍˆÍ")]
    public Vector3 m_SearchScale;

    [SerializeField]
    public AnimationCurve m_JumpCurve;

    private STATE m_State;

    private GameObject m_SearchArea = null;
    private SCR_PyonSearch scr_Search = null;
    private Rigidbody cp_Rb = null;
    private Vector3 m_Velocity;
    private bool m_IsFind = false;

    private bool m_Jump = false;
    private float m_lapseTime = 0.0f;

    void Start()
    {
        m_State = STATE.Idle;
        m_SearchArea = transform.GetChild(1).gameObject;
        scr_Search = m_SearchArea.GetComponent<SCR_PyonSearch>();
        cp_Rb = gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        switch (m_State) {
            case STATE.Idle:
                IdleProc();
                break;
            case STATE.Chase:
                ChaseProc();
                break;
            case STATE.Contact:
                ContactProc();
                break;
            case STATE.KnockBack:
                KnockBackProc();
                break;
            default:
                Debug.Log("Out of Range : Pyon State");
                break;
        }
    }

    private void IdleProc()
    {
        ;
    }

    private void ChaseProc()
    {
        ;
    }

    private void ContactProc()
    {
        ;
    }

    private void KnockBackProc()
    {
        ;
    }

    private void JumpProc()
    {

    }
}
