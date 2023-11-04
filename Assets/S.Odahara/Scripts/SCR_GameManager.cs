using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_GameManager : MonoBehaviour
{
    public static SCR_GameManager instance;
    [SerializeField]public static float[] m_StageScorenum = new float[5];//一応PlayerPrefs
    [SerializeField] public static string[] m_StageScore = new string[5];

    private static int m_stageNum;

    void Awake()
    {
        if (instance == null)
        {
            //このオブジェクトをシーン遷移時に保持する
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            // 既に存在する場合は重複しないように削除
            Destroy(this.gameObject);
        }
        Application.targetFrameRate = 60; 
    }


    //スコアを保存
    public static void SaveScore(int scoreTime, string score)
    {    
        if (SceneManager.GetActiveScene().name == "Stage1Scene")// ステージ1シーン
        {
            m_stageNum = 1; 
        }
        else if (SceneManager.GetActiveScene().name == "Stage2Scene")//ステージ2
        {
            m_stageNum = 2;
        }
        else if (SceneManager.GetActiveScene().name == "Stage3Scene")//ステージ2
        {
            m_stageNum = 3;
        }
        else if (SceneManager.GetActiveScene().name == "Stage4Scene")//ステージ2
        {
            m_stageNum = 4;
        }
        else if (SceneManager.GetActiveScene().name == "Stage5Scene")//ステージ2
        {
            m_stageNum = 5;
        }

        PlayerPrefs.SetInt($"Stage{m_stageNum}Time", scoreTime);
        PlayerPrefs.SetString($"Stage{m_stageNum}Score", score);
        m_StageScorenum[m_stageNum - 1] = scoreTime;
        m_StageScore[m_stageNum - 1] = score;
    }

    //データを削除
    public static void DeleteScore()
    {
        for(int i = 0; i < 5; i++)
        {
            m_StageScorenum[i] = 0;
            m_StageScore[i] = "";
        }
        PlayerPrefs.DeleteAll();
    }
}
