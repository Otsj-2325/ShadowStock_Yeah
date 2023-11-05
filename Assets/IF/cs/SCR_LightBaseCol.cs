using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_LightBaseCol : MonoBehaviour
{
    [SerializeField]GameObject m_LightObject;
    private Rigidbody m_LightRigdbody;
    public bool m_FallFlag;

    // Start is called before the first frame update
    void Start()
    {
        if(m_LightObject)
        {
            m_LightRigdbody = m_LightObject.GetComponent<Rigidbody>();
        }
        m_FallFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_FallFlag && m_LightObject)
        {
            m_LightRigdbody.useGravity = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player"){
            m_FallFlag = true;
        }
    }

}
