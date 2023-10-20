using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PyonSearch : MonoBehaviour
{
    private bool m_IsFind = false;

    public bool IsFind()
    {
        return m_IsFind;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_IsFind = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_IsFind = false;
        }
    }
}
