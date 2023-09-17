using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_WarpTrigger : MonoBehaviour
{
    public bool m_Contact;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player") 
        {
            m_Contact = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_Contact = false;
        }
    }

}
