using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class SCR_LightFall : MonoBehaviour
{
    public float m_KickForce = 1.0f;


    public Rigidbody cp_Rigidbody;
    public bool m_IsKick;
    public int m_IsKickCount;
    [SerializeField] private GameObject m_PlayerObj;

    // Start is called before the first frame update
    void Start()
    {
        m_IsKick = false;
        m_IsKickCount = 0;
        // Ç±Ç±
        GameObject childObject = gameObject.transform.GetChild(0).gameObject;
        SCR_IsKick kickObject = childObject.GetComponent<SCR_IsKick>();
        if (kickObject != null)
        {
            kickObject.SetIsKick(IsKick);
        }

        cp_Rigidbody = GetComponent<Rigidbody>();
        cp_Rigidbody.useGravity = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_IsKick == true)
        {
            
            m_IsKickCount++;
            if (m_IsKickCount > 60)
            {
                m_IsKick = false;
                m_IsKickCount = 0;
            }
        }
        else
        {
            // å≈íË
            //cp_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }

    }

    private void IsKick()
    {
        m_IsKick = true;

        Vector3 direction = transform.position - m_PlayerObj.transform.position;
        direction.Normalize();

        cp_Rigidbody.AddForce(direction * m_KickForce, ForceMode.Impulse);
        cp_Rigidbody.AddForce(Vector3.up * m_KickForce * 2, ForceMode.Impulse);

        SCR_EffectManager.instance.EFF_Light(transform.position, transform.rotation);
        SCR_SoundManager.instance.PlaySE(SE_Type.Gimmick_Light);//SEçƒê∂
    }
}
