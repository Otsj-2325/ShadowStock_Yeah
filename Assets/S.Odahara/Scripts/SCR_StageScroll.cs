using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SCR_StageScroll : MonoBehaviour
{
    [SerializeField] List<Sprite> m_StageSpriteList;
    [SerializeField] List<GameObject> m_StageScoreList;
    [SerializeField] List<Vector3> m_StagePosList;

    [SerializeField] int m_PosIndex = 0;
    [SerializeField] bool m_IsStageSelect = false;

    [SerializeField] private Image ScoreImage;

    [Header("キーボード操作のdelaytime")]
    [SerializeField] private float m_KeyBoardDelaytime = 0.4f;
    [Header("コントローラー操作のdelaytime")]
    [SerializeField] private float m_ControllerDelaytime = 1.0f;

    // レフトスティックの入力による選択の制約
    private float m_LeftStickSensitivity = 0.99f; // レフトスティックの感度（値を大きくすると感度が下がる）

    private float m_Time = 0.0f;

    private int m_OldPosIndex = 0;

    private GameObject m_StageImageObj;
    private RectTransform m_StageRectTransform;
    private SCR_StageAnime scr_StageAnime;
    private Image image;
    private SCR_ChangeScene scr_ChangeScene;
    //private SCR_StageSelectAnime scr_StageSelectAnime;



    void Start()
    {
        m_StageImageObj = GameObject.Find("StageImage");
        m_StageRectTransform = m_StageImageObj.GetComponent<RectTransform>();
        scr_StageAnime = m_StageImageObj.GetComponent<SCR_StageAnime>();
        image = GetComponent<Image>();
        scr_ChangeScene = GetComponent<SCR_ChangeScene>();
        image.sprite = m_StageSpriteList[m_PosIndex];

    }

    void Update()
    {
        m_OldPosIndex = m_PosIndex;
        m_Time += Time.unscaledDeltaTime;
        if(scr_StageAnime.m_IsAnimeFin)
        {
            if (m_IsStageSelect)
            {
                m_StageScoreList[m_PosIndex].SetActive(true);
                m_StageRectTransform.anchoredPosition = m_StagePosList[m_PosIndex];

            }

            if (m_Time > m_KeyBoardDelaytime)
            {
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))//入力された場合
                {
                    SCR_SoundManager.instance.PlaySE(SE_Type.System_Select, false, 0.5f);
                    // レフトスティックの上下の傾きに応じて選択肢を変更
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        if (m_PosIndex != m_StageSpriteList.Count - 1) m_PosIndex += 1;
                        else m_PosIndex = 0;
                    }
                    else if (Input.GetKeyDown(KeyCode.S))
                    {
                        if (m_PosIndex != 0) m_PosIndex -= 1;
                        else m_PosIndex = m_StageSpriteList.Count - 1;
                    }


                    m_Time = m_KeyBoardDelaytime - 0.2f; //ディレイをリセット
                    if (m_IsStageSelect)
                    {
                        m_StageScoreList[m_OldPosIndex].SetActive(false);
                        m_StageScoreList[m_PosIndex].SetActive(true);
                        m_StageRectTransform.anchoredPosition = m_StagePosList[m_PosIndex];
                    }
                    image.sprite = m_StageSpriteList[m_PosIndex];
                    //scr_StageSelectAnime.Anime();
                }


                // エンターキーの入力
                if (Input.GetKeyDown(KeyCode.Return) && m_IsStageSelect)
                {
                    SCR_SoundManager.instance.PlaySE(SE_Type.System_Decision, false, 0.5f);
                    switch (m_PosIndex)
                    {
                        case 0:
                            scr_ChangeScene.Change("Stage1Scene");
                            break;
                        case 1:
                            scr_ChangeScene.Change("Stage2Scene");
                            break;
                        case 2:
                            scr_ChangeScene.Change("Stage3Scene");
                            break;
                        case 3:
                            scr_ChangeScene.Change("Stage4Scene");
                            break;
                        case 4:
                            scr_ChangeScene.Change("Stage5Scene");
                            break;
                        default:
                            break;
                    }
                }
            }

            if (m_Time > m_ControllerDelaytime)
            {
                //ゲームパッド処理
                if (Gamepad.current == null) { return; }
                else
                {
                    // レフトスティックの入力取得
                    Vector2 leftStickInput = Gamepad.current.leftStick.ReadValue();

                    if (Mathf.Abs(leftStickInput.y) > m_LeftStickSensitivity)//感度を超えている場合
                    {
                        SCR_SoundManager.instance.PlaySE(SE_Type.System_Select, false, 0.5f);
                        // レフトスティックの上下の傾きに応じて選択肢を変更
                        if (leftStickInput.y > 0)
                        {
                            if (m_PosIndex != m_StageSpriteList.Count - 1) m_PosIndex += 1;
                            else m_PosIndex = 0;
                        }

                        else if (leftStickInput.y < 0)
                        {
                            if (m_PosIndex != 0) m_PosIndex -= 1;
                            else m_PosIndex = m_StageSpriteList.Count - 1;
                        }


                        m_Time = 0.0f; //ディレイをリセット
                        image.sprite = m_StageSpriteList[m_PosIndex];
                        if (m_IsStageSelect)
                        {
                            m_StageScoreList[m_OldPosIndex].SetActive(false);
                            m_StageScoreList[m_PosIndex].SetActive(true);
                            m_StageRectTransform.anchoredPosition = m_StagePosList[m_PosIndex];
                        }
                    }

                    // Aボタンの入力
                    if (Gamepad.current.buttonSouth.isPressed && m_IsStageSelect)
                    {
                        SCR_SoundManager.instance.PlaySE(SE_Type.System_Decision, false, 0.5f);
                        SCR_SoundManager.instance.PlayBGM(BGM_Type.GAME);
                        switch (m_PosIndex)
                        {
                            case 0:
                                scr_ChangeScene.Change("Stage1Scene");
                                break;
                            case 1:
                                scr_ChangeScene.Change("Stage2Scene");
                                break;
                            case 2:
                                scr_ChangeScene.Change("Stage3Scene");
                                break;
                            case 3:
                                scr_ChangeScene.Change("Stage4Scene");
                                break;
                            case 4:
                                scr_ChangeScene.Change("Stage5Scene");
                                break;
                            default:
                                break;
                        }

                    }
                }
            }
        }
        
    }
}
