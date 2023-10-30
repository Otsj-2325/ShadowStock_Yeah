using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SCR_Clock : MonoBehaviour
{
    [SerializeField] private Image timerImage = default!;
    [SerializeField] private Text timerText = default!;
    [SerializeField] private SCR_ChangeScene scr_ChangeScene = default!;
    [SerializeField] private SCR_Goal scr_Goal = default!;

    [Header("�J�E���g���n�߂�܂ł̎���")]
    [SerializeField] private int countdown = default!;
    [Header("��������")]
    [SerializeField] private int stageTime = default!;

    private DateTime startDateTime;
    private TimeSpan totalTime;
    private int limitTime;

    public int cullentScoreTime;

    private void Start()
    {
        startDateTime = DateTime.Now;
        limitTime = countdown + stageTime;
        timerText.text = $"{stageTime}";
    }

    private void Update()
    {
        totalTime = DateTime.Now - startDateTime;

        if (totalTime.Seconds > countdown && !scr_Goal.m_IsClearflg)
        {

            timerText.text = $"{ limitTime - totalTime.Seconds}";
            timerImage.fillAmount = (float)(limitTime - totalTime.Seconds) / limitTime;

            if (totalTime.Seconds > limitTime)//�������Ԃ𒴂�����
            {
                scr_ChangeScene.Change();//�Q�[���I�[�o�[�V�[���ɑJ��
            }
            cullentScoreTime = totalTime.Seconds - countdown;//�����������Ԃ�ۑ�
        }

    }

}
