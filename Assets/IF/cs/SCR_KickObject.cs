using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_KickObject : MonoBehaviour
{
    public Rigidbody cp_Rigidbody;
    public bool m_IsKick;
    public int m_IsKickCount;

    //[SerializeField] GameObject m_PlayerObj;

    // Start is called before the first frame update
    void Start()
    {
        m_IsKick = false;
        m_IsKickCount = 0;
    }

    // Update is called once per frame
    void Update()
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
            // ŒÅ’è
            cp_Rigidbody.constraints =
              RigidbodyConstraints.FreezePositionX
            | RigidbodyConstraints.FreezePositionZ
            | RigidbodyConstraints.FreezeRotation;
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Kick"))
        {
           Debug.Log("ƒLƒbƒN‚³‚ê‚½");
        }

    }
}
