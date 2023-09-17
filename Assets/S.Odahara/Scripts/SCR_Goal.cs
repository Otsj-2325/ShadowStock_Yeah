using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SCR_Goal : MonoBehaviour
{
    private SCR_ChangeScene scr_ChangeScene;
    private float m_Countnum;
    float m_time;

    private bool m_IsClearflg;//アニメーション用
    private bool m_IsGameOverflg;
    // Start is called before the first frame update
    void Start()
    {
        scr_ChangeScene = GetComponent<SCR_ChangeScene>();
        m_IsClearflg = false;
        m_IsGameOverflg = false;
        m_Countnum = 300.0f;
        m_time = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        m_time += Time.deltaTime;
        if (m_time >= 1.0f) 
        { 
            m_Countnum--;
            m_time = 0.0f;
        }

        if (m_Countnum <= 0.0f) 
        { scr_ChangeScene.Change("GameOverScene"); }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            float cleartime = 300 - m_Countnum;
            SCR_GameManager.SaveScore(cleartime);
            m_IsClearflg = true;
            scr_ChangeScene.Change();
        }
    }
}
