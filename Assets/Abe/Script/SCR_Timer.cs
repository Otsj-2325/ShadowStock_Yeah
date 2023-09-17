using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_Timer : MonoBehaviour
{
    [SerializeField] [Header("")] private float m_StartTime = 0.0f;
    private float m_Time;
    private Text cp_Tex;

    void Start()
    {
        cp_Tex = this.GetComponent<Text>();
        m_Time = m_StartTime;
    }
    void Update()
    {


    }

    private void FixedUpdate()
    {
        if(m_Time < 0.0f)
        {
            TimeUp();
        }
        else
        {
            cp_Tex.text = string.Format("‚Ì‚±‚è:{0:00}", m_Time);

            m_Time -= Time.deltaTime;
        }
    }

    private void TimeUp()
    {
        cp_Tex.text = string.Format("Time UP!!!");
    }
}
