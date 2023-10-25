using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_SoundManager : MonoBehaviour
{
   public static SCR_SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            //Scene�ړ��ŏ�����Ȃ��悤�ɂ���
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //���ɐ�������Ă���̂ł���Ώ���
            Destroy(this);
        }
    }
    
}
