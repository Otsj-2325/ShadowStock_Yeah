using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_IsKick : MonoBehaviour, IFCGimmck
{
    private Action isKick;

    public void SetIsKick(Action action)
    {
        isKick = action;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GimmckAct()
    {
        isKick?.Invoke();
        // �R��ꂽ���̃M�~�b�N�A�N�V����
        Debug.Log("�R��ꂽ��");
    }
}
