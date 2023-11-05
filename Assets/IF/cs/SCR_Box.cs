using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Box : MonoBehaviour
{
    public Rigidbody cp_Rigidbody;
    public bool m_IsKick;
    public int m_IsKickCount;
    private GameObject m_PlayerObj;
    [SerializeField] private GameObject m_KickPower;

    // Start is called before the first frame update
    void Start()
    {
        m_IsKick = false;
        m_IsKickCount = 0;

        m_PlayerObj = FindObjectOfType<PlayerController>().gameObject;

        // ����
        GameObject childObject = gameObject.transform.GetChild(0).gameObject;
        SCR_IsKick kickObject = childObject.GetComponent<SCR_IsKick>();
        if(kickObject != null )
        {
            kickObject.SetIsKick(IsKick);
        }

        cp_Rigidbody = GetComponent<Rigidbody>();
       // cp_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_IsKick == true)
        {
            m_IsKickCount++;
            if (m_IsKickCount >= 120)
            {
                m_IsKick = false;
                m_IsKickCount = 0;
            }

         
        }
        else
        {
            // �Œ�
            //cp_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }

    }

    //protected override void OnTriggerEnter(Collider other)
    //{
    //    //if (other.CompareTag("Kick"))
    //    //{
    //    //    cp_Rigidbody.constraints =
    //    //     RigidbodyConstraints.None
    //    //   | RigidbodyConstraints.FreezeRotation;

    //    //    cp_Rigidbody.velocity = Vector3.zero;

    //    //    // �L�b�N����I�u�W�F�N�g�̒��S�_�Ƃ̍������߂�
    //    //    Vector3 direction = transform.position - m_PlayerObj.transform.position;
    //    //    // y�������𖳎����Đ��K��
    //    //    direction.y = 0;
    //    //    direction.Normalize();


    //    //    cp_Rigidbody.AddForce(direction * 20, ForceMode.Impulse);

    //    //    m_IsKick = true;
    //    //}

    //}

    // ����
    private void IsKick()
    {
        Debug.Log("�R��ꂽ");

        //cp_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        cp_Rigidbody.velocity = Vector3.zero;

        // �L�b�N����I�u�W�F�N�g�̒��S�_�Ƃ̍������߂�
        Vector3 direction = transform.position - m_PlayerObj.transform.position;
        // y�������𖳎����Đ��K��
        direction.y = 5;
        direction.Normalize();


        cp_Rigidbody.AddForce(direction * 10, ForceMode.Impulse);

        //m_IsKick = true;
    }

   
}
