using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_FallStoneArea : MonoBehaviour
{
    [SerializeField] GameObject fallStone;
    [SerializeField] List<GameObject> FallPosition;
    [SerializeField] float fallInterval;
    float m_fCnt = 0.0f;
    bool m_CanProcess;

    // Start is called before the first frame update
    void Start()
    {
        if(fallStone){
            if(FallPosition.Count > 0){
                m_CanProcess = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(m_CanProcess){
            if (m_fCnt > fallInterval)
            {
                GameObject obj = GameObject.Instantiate(fallStone);

                int rand = Random.Range(0,FallPosition.Count);
                int rand2 = Random.Range(0,10);

                fallStone.transform.position = FallPosition[rand].gameObject.transform.position;
                fallStone.transform.eulerAngles = new Vector3(0.0f, 10 * rand2, 0.0f);
                m_fCnt = 0.0f;
            }

            m_fCnt += 1.0f;
        }
    }

}
