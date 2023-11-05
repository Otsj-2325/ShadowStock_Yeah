using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SCR_CursorRectTransform : MonoBehaviour
{
    [SerializeField] List<RectTransform> m_PositionList;
    [SerializeField] List<UnityEvent> m_ButtonEventList;

    [Header("上下にスティックを動かすか（左スティックのみ対応）") ]
    [SerializeField] bool m_IsVerticalStick;

    [Header("transform オフセット")]
    [SerializeField] Vector3 offset = default;

    private int m_PosIndex = 0;
    private float m_Delaytime = 0.4f;
    private float m_Time = 0.0f;

    // レフトスティックの入力による選択の制約
    private float m_LeftStickSensitivity = 0.9f; // レフトスティックの感度（値を大きくすると感度が下がる）

    void Start()
    {
        SCR_SoundManager.instance.PlayBGM(BGM_Type.TITLE);
        SCR_SoundManager.instance.SetVolumeBGM(0.8f);
        transform.position = m_PositionList[0].position + offset;
    }

    void Update()
    {
        m_Time += Time.unscaledDeltaTime;
        if (m_Time > m_Delaytime)
        {
            transform.position = m_PositionList[m_PosIndex].position + offset;

            //キーボード処理
            if (m_IsVerticalStick)
            {
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))//入力された場合
                {
                    // レフトスティックの上下の傾きに応じて選択肢を変更
                    if (Input.GetKeyDown(KeyCode.W)) m_PosIndex += -1;
                    else if (Input.GetKeyDown(KeyCode.S)) m_PosIndex += 1;

                    m_PosIndex = Mathf.Clamp(m_PosIndex, 0, m_PositionList.Count - 1);// 選択肢の範囲を制限

                    m_Time = 0.2f; //ディレイをリセット

                    SCR_SoundManager.instance.PlaySE(SE_Type.System_Select, false, 0.5f);
                }
            }
            if (!m_IsVerticalStick)
            {
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))//入力された場合
                {
                    // レフトスティックの左右の傾きに応じて選択肢を変更
                    if (Input.GetKeyDown(KeyCode.D)) m_PosIndex += 1;
                    else if (Input.GetKeyDown(KeyCode.A)) m_PosIndex += -1;

                    m_PosIndex = Mathf.Clamp(m_PosIndex, 0, m_PositionList.Count - 1);// 選択肢の範囲を制限

                    m_Time = 0.2f; //ディレイをリセット
                    SCR_SoundManager.instance.PlaySE(SE_Type.System_Select, false, 0.5f);
                }
            }

            // エンターキーの入力
            if (Input.GetKeyDown(KeyCode.Return))
            {
                m_ButtonEventList[m_PosIndex].Invoke();
                SCR_SoundManager.instance.PlaySE(SE_Type.System_Decision, false, 0.5f);
            }


            //ゲームパッド処理
            if (Gamepad.current == null) { return; }
            else {
                // レフトスティックの入力取得
                Vector2 leftStickInput = Gamepad.current.leftStick.ReadValue();
                if (m_IsVerticalStick)
                {
                    if (Mathf.Abs(leftStickInput.y) > m_LeftStickSensitivity)//感度を超えている場合
                    {
                        // レフトスティックの上下の傾きに応じて選択肢を変更
                        if (leftStickInput.y > 0) m_PosIndex += -1;
                        else if (leftStickInput.y < 0) m_PosIndex += 1;

                        m_PosIndex = Mathf.Clamp(m_PosIndex, 0, m_PositionList.Count - 1);// 選択肢の範囲を制限

                        m_Time = 0.2f; //ディレイをリセット
                        SCR_SoundManager.instance.PlaySE(SE_Type.System_Select, false, 0.5f);
                    }
                }
                if (!m_IsVerticalStick)
                {
                    if (Mathf.Abs(leftStickInput.x) > m_LeftStickSensitivity)//感度を超えている場合
                    {
                        // レフトスティックの左右の傾きに応じて選択肢を変更
                        if (leftStickInput.x > 0) m_PosIndex += 1;
                        else if (leftStickInput.x < 0) m_PosIndex += -1;

                        m_PosIndex = Mathf.Clamp(m_PosIndex, 0, m_PositionList.Count - 1);// 選択肢の範囲を制限

                        m_Time = 0.2f; //ディレイをリセット
                        SCR_SoundManager.instance.PlaySE(SE_Type.System_Select, false, 0.5f);
                    }
                }
                // Aボタンの入力
                if (Gamepad.current.buttonSouth.isPressed)
                {
                    m_ButtonEventList[m_PosIndex].Invoke();
                    SCR_SoundManager.instance.PlaySE(SE_Type.System_Decision, false, 0.5f);
                }
            }
        }
    }
}
