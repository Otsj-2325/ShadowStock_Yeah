using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Sphere : MonoBehaviour
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

        // ここ
        GameObject childObject = gameObject.transform.GetChild(0).gameObject;
        SCR_IsKick kickObject = childObject.GetComponent<SCR_IsKick>();
        if (kickObject != null)
        {
            kickObject.SetIsKick(IsKick);
        }

        cp_Rigidbody = GetComponent<Rigidbody>();
        cp_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
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
            // 固定
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
        SCR_EffectManager.instance.EFF_Puff(transform.position, transform.rotation);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Wall"))
    //    {
    //        // 衝突面の法線ベクトルを取得
    //        Vector3 normal = collision.contacts[0].normal;
    //        // キックの方向に力を加える


    //        cp_Rigidbody.AddForce(normal * m_KickForce, ForceMode.Impulse);

    //        m_IsKick = false;
    //        m_IsKickCount = 0;
    //    }
    //}

}
