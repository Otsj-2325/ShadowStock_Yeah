using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SCR_Goal : MonoBehaviour
{
    private SCR_VCamManager scr_VCamManager;
    private float m_Countnum;
    float m_time;

    [SerializeField]
    private bool m_IsClearflg;//�A�j���[�V�����p
    [SerializeField]
    private GameObject m_ResultPanel;//�A�j���[�V�����p

    private bool m_IsGameOverflg;
    // Start is called before the first frame update
    void Start()
    {
        scr_VCamManager = FindObjectOfType<SCR_VCamManager>();
        m_IsClearflg = true;//�f�o�b�O�̂���true
        m_IsGameOverflg = false;
        m_Countnum = 300.0f;
        m_time = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //�f�o�b�O�p
        if(m_IsClearflg)
        {
            scr_VCamManager.SwitchVCam(3);
            m_ResultPanel.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            float cleartime = 300 - m_Countnum;
            SCR_GameManager.SaveScore(cleartime);
            m_IsClearflg = true;
            scr_VCamManager.SwitchVCam(3);
            m_ResultPanel.SetActive(true);
        }
    }
}
