using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SCR_PlayerMove : MonoBehaviour
{
    private Rigidbody cp_rb;
    private Vector3 m_moveVel;
    void Start()
    {
        cp_rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        m_moveVel = Vector3.zero;

        ReadController();

        cp_rb.velocity = m_moveVel;
    }



    private void ReadController()
    {
        var stick = Gamepad.current.leftStick.ReadValue();

        if (stick.x > 0.25f || stick.x < -0.25f)
        {
            m_moveVel.x = 3.0f * stick.x;
        }

        if (stick.y > 0.25f || stick.y < -0.25f)
        {
            m_moveVel.z = 3.0f * stick.y;
        }

        if(m_moveVel.magnitude > 0.01f)
        {
            this.transform.rotation = Quaternion.LookRotation(m_moveVel, Vector3.up);
        }
    }

}
