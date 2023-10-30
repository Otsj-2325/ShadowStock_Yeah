using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_StoneWall : MonoBehaviour
{
    [SerializeField] private GameObject stone;
    private Rigidbody stoneRb = null;
    [SerializeField] private bool m_isFall = false;

    // Start is called before the first frame update
    void Start()
    {
        if(stone){
            stoneRb = stone.GetComponent<Rigidbody>();
            stoneRb.isKinematic = true;
            Debug.Log("Get stone RB");
        }
        else{
            m_isFall = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!m_isFall){
            stoneRb.isKinematic = false;
            Debug.Log("Fall stone");
        }
    }
}
