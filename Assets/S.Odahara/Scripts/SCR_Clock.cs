using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//まだ途中
public class SCR_Clock : MonoBehaviour
{
    [SerializeField] private Image timerImage = default!;
    [SerializeField] private Transform clockHand = default!;
    [SerializeField] private Text timerText = default!;
    [SerializeField] private SCR_Goal scr_Goal = default!;

    [Header("カウントし始めるまでの時間")]
    [SerializeField] private int countdown = default!;
    [Header("制限時間")]
    [SerializeField] private int stageTime = default!;

    private DateTime startDateTime;
    private TimeSpan totalTime;
    private int limitTime;
    private int previousTime; 
    private int cullentTime;
    private int scoreTotalTime;

    public int cullentScoreTime;

    private void Start()
    {
        startDateTime = DateTime.Now;
        limitTime = countdown + stageTime;
        timerText.text = $"{stageTime}";
    }

    private void Update()
    {
        previousTime = totalTime.Seconds;
        totalTime = DateTime.Now - startDateTime;
        cullentTime = totalTime.Seconds;
        if (cullentTime != previousTime) scoreTotalTime += 1;

        if (scoreTotalTime > countdown && !scr_Goal.m_IsClearflg)
        {
            if(cullentTime != previousTime)
            {

                timerText.text = $"{ stageTime - scoreTotalTime + countdown}";
                timerImage.fillAmount = (float)(stageTime - (scoreTotalTime - countdown)) / stageTime;

                clockHand.DOLocalRotate(new Vector3(0.0f, 0.0f, -360.0f / stageTime * (scoreTotalTime - countdown)), 0.0f);

                cullentScoreTime = scoreTotalTime - countdown;//かかった時間を保存
            }

            if (scoreTotalTime == limitTime)//制限時間を超えたら
            {
                cullentScoreTime = 0;
                SCR_FadeManager.FadeOut("GameOverScene", Color.black, 0.4f);//ゲームオーバーシーンに遷移
            }

        }

    }

}
