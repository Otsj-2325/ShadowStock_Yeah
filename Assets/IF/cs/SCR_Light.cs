using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SCR_Light : MonoBehaviour
{
    private bool m_IsLight;
    public Rigidbody cp_Rigidbody;
    public bool m_IsKick;

    // Start is called before the first frame update
    void Start()
    {
        m_IsKick = false;

        gameObject.GetComponent<Renderer>().material.color = Color.white;
        m_IsLight = false;

        // ここ
        GameObject childObject = gameObject.transform.GetChild(0).gameObject;
        SCR_IsKick kickObject = childObject.GetComponent<SCR_IsKick>();
        if (kickObject != null)
        {
            kickObject.SetIsKick(IsKick);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_IsLight)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.yellow;
            Debug.Log("ライトオン");
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.color = Color.white;
            Debug.Log("ライトオフ");
        }
    }

    private void IsKick()
    {
        if (m_IsLight == false)
        {
            m_IsLight = true;
        }
        else if (m_IsLight == true)
        {
            m_IsLight = false;
        }
    }
}
