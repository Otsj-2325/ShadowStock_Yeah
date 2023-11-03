using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_Score : MonoBehaviour
{
    [SerializeField] private Text[] m_ScoreTexts = default!;

    [Header("ステージの最大数")]
    [SerializeField] private int m_StageNum = default!;

    void Start()
    {
        
        for(int i = 0; i < m_StageNum; i++)
        {
            var scoreString = PlayerPrefs.GetString($"Stage{i}Score", "---");
            var timeNum = PlayerPrefs.GetInt($"Stage{i}Time", 0);

            m_ScoreTexts[i].text = timeNum <= 0 ? $"SCORE :{scoreString}\n  TIME : ---" : $"SCORE :{scoreString}\n  TIME : {timeNum}"; 
        }
    }
}
