using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_ETABrain : MonoBehaviour
{
    [SerializeField] private SCR_ETAStateController scr_StCtrl;

    private Vector3 m_SearchPos;

    void Start()
    {
        scr_StCtrl.Initialize((int)ETAStateType.Patrol);
    }

    private void FixedUpdate()
    {

        scr_StCtrl.UpdateSequence();
    }

    public Vector3 SearchPlayerPos()
    {
        return m_SearchPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            m_SearchPos = other.gameObject.transform.position;
            scr_StCtrl.SetStateFind();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Shadow")
        {
            scr_StCtrl.SetStateStop();
        }


    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Shadow")
        {
            scr_StCtrl.SetStatePatrol();
        }
    }
}
