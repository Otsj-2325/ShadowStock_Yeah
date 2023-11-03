using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Stone : MonoBehaviour
{
    [SerializeField] float deathInterval;
    float m_fCnt = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_fCnt > deathInterval){
            Destroy(this.gameObject);
        }

        m_fCnt += 1.0f;
    }
}
