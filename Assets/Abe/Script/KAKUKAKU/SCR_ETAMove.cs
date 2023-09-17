

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_ETAMove : MonoBehaviour
{
    [SerializeField] private float m_Speed = 0.0f;
    [SerializeField] private int m_MoveFrame = 70;
    [SerializeField] private int m_SleepFrame = 30;

    private Vector3 m_TargetPos;
    private int m_Count;

    private bool m_Stop;
    private bool m_IsMove;
    private bool m_IsClimb;
    private bool m_IsFall;
    private bool m_Arrival;

    private void Start()
    {
        m_TargetPos = this.transform.position;
    }

    private void FixedUpdate()
    {
        if (m_Stop || m_Arrival) return;

        ++m_Count;

        if (m_IsMove) {//Move

            MoveProc();

            if (m_Count > m_MoveFrame) {
                m_Count = 0;
                m_IsMove = false;
            }
        }
        else { //Sleep
            if(m_Count > m_SleepFrame) {
                m_Count = 0;
                m_IsMove = true;
            }
        }
    }


    private void MoveProc()
    {
        Vector3 pos = this.transform.position;
        if (m_IsClimb)
        {
            pos.y += 100;
            this.transform.position = Vector3.MoveTowards(this.transform.position, pos, m_Speed * Time.deltaTime);
        }
        else if (m_IsFall)
        {
            pos.y -= 100;
            this.transform.position = Vector3.MoveTowards(this.transform.position, pos, m_Speed * Time.deltaTime);
        }
        else
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, m_TargetPos, m_Speed * Time.deltaTime);
        }


        if (0.1f > Vector3.Distance(this.transform.position, m_TargetPos))
        {
            m_Count = 0;
            m_Arrival = true;
        }
    }


    /// <summary>
    /// 到着確認
    /// </summary>
    /// <returns></returns>
    public bool IsArrival()
    {
        return m_Arrival;
    }

    /// <summary>
    /// 目標地点設定
    /// </summary>
    /// <param name="目標地点"></param>
    public void SetTargetPos(Vector3 pos)
    {
        m_TargetPos = pos;
        m_Arrival = false;
    }

    public void SetClimbMode(bool flag)
    {
        m_IsClimb = flag;
        m_IsFall = false;
    }

    public void SetFallMode(bool flag)
    {
        m_IsFall = flag;
        m_IsClimb = false;
    }

    public void SetStop(bool flag)
    {
        m_Stop = flag;
    }

}
