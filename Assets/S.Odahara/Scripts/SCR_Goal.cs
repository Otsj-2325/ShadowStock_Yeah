using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SCR_Goal : MonoBehaviour
{
    [Header("Clockのobject")]
    [SerializeField] SCR_Clock scr_Clock = default!;
    [Header("Resultのobject")]
    [SerializeField] private GameObject m_Result = default!;
    [Header("Resultのobjectの中にあるScoreを0からS→Dの順番で入れる")]
    [SerializeField] private GameObject[] m_ResultScoreArray = default!;
    [Header("スコアS判定時間（かかった時間）")]
    [SerializeField] private int m_scoreSTime = default!;
    [Header("スコアA判定時間")]  
    [SerializeField] private int m_scoreATime = default!;
    [Header("スコアB判定時間")]  
    [SerializeField] private int m_scoreBTime = default!;
    [Header("スコアC判定時間(これより時間かかったらスコアD)")]  
    [SerializeField] private int m_scoreCTime = default!;

    [SerializeField] public bool m_IsClearflg;//アニメーション用

    private SCR_VCamManager scr_VCamManager;
    private int m_ScoreImageListNum;

    // Start is called before the first frame update
    void Start()
    {
        scr_VCamManager = FindObjectOfType<SCR_VCamManager>();

    }

    // Update is called once per frame
    void Update()
    {
        //デバッグ用
        if(m_IsClearflg)
        {
            scr_VCamManager.SwitchVCam(3);
            SCR_GameManager.SaveScore(scr_Clock.cullentScoreTime, JudgeScore(scr_Clock.cullentScoreTime));
            m_Result.SetActive(true);
            for (int i = 0; i < m_ResultScoreArray.Length; i++)
            {
                m_ResultScoreArray[i].SetActive(i == m_ScoreImageListNum);
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            m_IsClearflg = true;
            SCR_SoundManager.instance.PlaySE(SE_Type.System_Goal);
            SCR_SoundManager.instance.PlayBGM(BGM_Type.RESULT);
        }
    }

    //Rank判定
    public string JudgeScore(int score)
    {
        if (score <= m_scoreSTime)
        {
            m_ScoreImageListNum = 0;
            return "S";
        }
        else if (score > m_scoreSTime && score <= m_scoreATime)
        {
            m_ScoreImageListNum = 1;
            return "A";
        }
        else if (score > m_scoreATime && score <= m_scoreBTime)
        {
            m_ScoreImageListNum = 2;
            return "B";
        }
        else if (score > m_scoreBTime && score <= m_scoreCTime)
        {
            m_ScoreImageListNum = 3;
            return "C";
        }
        else 
        {
            m_ScoreImageListNum = 4;
            return "D";
        }
    }
}
