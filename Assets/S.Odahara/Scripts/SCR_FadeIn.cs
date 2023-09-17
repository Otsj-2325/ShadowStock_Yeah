using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_FadeIn : MonoBehaviour
{
    //FadeInするためだけのスクリプト
    //Sceneにあるオブジェクトにアタッチするだけ、、だけどそんな面倒なことやだから要改良

    [SerializeField] Color m_FadeInColor = Color.black;
    [SerializeField] float m_FadeInTime = 0.4f;
    // Start is called before the first frame update
    void Start()
    {
        SCR_FadeManager.FadeIn(m_FadeInColor, m_FadeInTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
