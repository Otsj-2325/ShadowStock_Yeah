using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_GroundTrigger : MonoBehaviour
{
    private bool m_IsGround = false;

    public bool IsGround()
    {
        return m_IsGround;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            m_IsGround = true;
        }
    }
    /// <summary>
    /// •s–{ˆÓ
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            m_IsGround = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            m_IsGround = false;
        }
    }
}  