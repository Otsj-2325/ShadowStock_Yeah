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

            //Scene移動で消されないようにする
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //既に生成されているのであれば消す
            Destroy(this);
        }
    }
    
}
