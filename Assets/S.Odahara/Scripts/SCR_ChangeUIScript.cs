using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

//ボタンのOnClickをスクリプトから作動させ、UI表示
public class SCR_ChangeUIScript : MonoBehaviour
{
    //消すUI
    [SerializeField] GameObject m_UI;
    //前に表示していたUI
    [SerializeField] GameObject m_PreviousUI;
    //次表示するUI
    [SerializeField] GameObject m_NextUI;


    [Header("戻るUIがあるか")]
    [SerializeField] bool m_Returnflg;//戻ることができるか
    [Header("ポーズに使用するか")]
    [SerializeField] bool m_Pauseflg;//ポーズか
    [Header("Aボタンで作動するか")]
    [SerializeField] bool m_BottonAflg;//Aボタン

    private Button m_Button;


    // Start is called before the first frame update
    void Start()
    {
        m_Button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        //キーボード処理
        if (m_Returnflg)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Return();
            }
        }

        if (m_Pauseflg)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Pause();
            }
        }

        if (m_BottonAflg)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Next();
            }
        }

        //ゲームパッド処理
        if (Gamepad.current == null) { return; }
        else{
            if (m_Returnflg)
            {
                if (Gamepad.current.bButton.isPressed)//Bボタンキーボードで戻る処理を後で追加
                {
                    Return();
                }
            }

            if (m_Pauseflg)
            {
                if (Gamepad.current.startButton.wasPressedThisFrame)
                {
                    Pause();
                }
            }

            if (m_BottonAflg)
            {
                if (Gamepad.current.buttonSouth.isPressed)
                {
                    Next();
                }
            }
        }

    }


    //次のUI切り替え
    public void Next()
    {

        //非表示
        if (m_UI != null) m_UI.SetActive(false);

        //次のUIを表示
        if (m_NextUI != null) m_NextUI.SetActive(true);

        ////イベントシステムに最初に選択しているボタンを設定
        //GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(m_nextButton);
    }

    //前のUI切り替え
    public void Return()
    {
        if(m_Pauseflg) Time.timeScale = 1f;//再開

        //非表示
        if (m_UI != null) m_UI.SetActive(false);

        //次のUIを表示
        if (m_PreviousUI != null) m_PreviousUI.SetActive(true);


    }
    public void Pause()
    {
        //非表示
        if(m_UI != null) m_UI.SetActive(false);

        //次のUIを表示
        if (m_NextUI != null) m_NextUI.SetActive(true);

        Time.timeScale = 0f;//時止める
    }
}
