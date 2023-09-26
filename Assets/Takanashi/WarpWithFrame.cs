using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpWithFrame : MonoBehaviour
{
    [SerializeField] [Header("���[�v��")] private GameObject m_DetnWarp;
    [SerializeField] [Header("�J�ڐ�J����")] private int m_VCamNum;

    // FrameManager����Ăяo��
    public event Action warp;                   // ���[�v�Ƀv���C���[���G�ꂽ
    public event Action<Vector3> getPosition;   // �ړ���̍��W���擾

    private Transform m_WarpPos;
    private GameObject m_Player;

    private SCR_WarpTrigger scr_Wt;
    private SCR_VCamManager scr_VM;

    private bool m_OnWarp;

    private void Start()
    {
        m_WarpPos = m_DetnWarp.transform.Find("WarpPoint").transform;
        m_Player = GameObject.FindGameObjectsWithTag("Player")[0];

        scr_Wt = this.transform.GetChild(0).gameObject.GetComponent<SCR_WarpTrigger>();
        scr_VM = FindObjectOfType<SCR_VCamManager>();
    }

    private void FixedUpdate()
    {
        var contact = scr_Wt.m_Contact;
        var canwarp = m_OnWarp && contact;

        if (canwarp)
        {
            scr_VM.SwitchVCam(m_VCamNum);
            m_Player.transform.position = m_WarpPos.transform.position;
            m_DetnWarp.GetComponent<WarpWithFrame>().Arrival();

            getPosition?.Invoke(m_DetnWarp.transform.position);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_OnWarp = true;

            warp?.Invoke();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_OnWarp = false;
            if (!this.transform.Find("WarpTrigger").gameObject.activeSelf)
            {
                this.transform.Find("WarpTrigger").gameObject.SetActive(true);
            }
        }
    }

    public void Arrival()
    {
        this.transform.Find("WarpTrigger").gameObject.SetActive(false);
    }
}
