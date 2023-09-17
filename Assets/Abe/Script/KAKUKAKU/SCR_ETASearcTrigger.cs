using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_ETASearchTrigger : MonoBehaviour
{
    public Vector3 m_SearchPos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_SearchPos = other.gameObject.transform.position;
            //StCtrl.FindPlayer();
        }
    }
}
