using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_GameManager : MonoBehaviour
{
    public static SCR_GameManager instance;
    [SerializeField]public static float[] m_StageScorenum = new float[5];//�ꉞPlayerPrefs
    [SerializeField] public static string[] m_StageScore = new string[5];

    private static int m_stageNum;

    void Awake()
    {
        if (instance == null)
        {
            //���̃I�u�W�F�N�g���V�[���J�ڎ��ɕێ�����
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            // ���ɑ��݂���ꍇ�͏d�����Ȃ��悤�ɍ폜
            Destroy(this.gameObject);
        }
        Application.targetFrameRate = 60; 
    }


    //�X�R�A��ۑ�
    public static void SaveScore(int scoreTime, string score)
    {    
        if (SceneManager.GetActiveScene().name == "Stage1Scene")// �X�e�[�W1�V�[��
        {
            m_stageNum = 1; 
        }
        else if (SceneManager.GetActiveScene().name == "Stage2Scene")//�X�e�[�W2
        {
            m_stageNum = 2;
        }
        else if (SceneManager.GetActiveScene().name == "Stage3Scene")//�X�e�[�W2
        {
            m_stageNum = 3;
        }
        else if (SceneManager.GetActiveScene().name == "Stage4Scene")//�X�e�[�W2
        {
            m_stageNum = 4;
        }
        else if (SceneManager.GetActiveScene().name == "Stage5Scene")//�X�e�[�W2
        {
            m_stageNum = 5;
        }

        PlayerPrefs.SetInt($"Stage{m_stageNum}Time", scoreTime);
        PlayerPrefs.SetString($"Stage{m_stageNum}Score", score);
        m_StageScorenum[m_stageNum - 1] = scoreTime;
        m_StageScore[m_stageNum - 1] = score;
    }

    //�f�[�^���폜
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
