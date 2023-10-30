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

    [Header("カウントし始めるまでの時間")]
    [SerializeField] private int countdown = default!;
    [Header("制限時間")]
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

            if (totalTime.Seconds > limitTime)//制限時間を超えたら
            {
                scr_ChangeScene.Change();//ゲームオーバーシーンに遷移
            }
            cullentScoreTime = totalTime.Seconds - countdown;//かかった時間を保存
        }

    }

}
