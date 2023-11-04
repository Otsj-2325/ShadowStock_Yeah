using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SCR_FadeManager : MonoBehaviour
{

    //フェード用のCanvasとImage
    private static Canvas m_fadeCanvas;
    private static Image m_fadeImage;

    //フェード用Imageの透明度
    private static float m_alpha = 0.0f;

    //フェードインアウトのフラグ
    public static bool m_isFadeIn = false;
    public static bool m_isFadeOut = false;

    public static bool m_isFadeInFinish = false;
    public static bool m_isFadeOutFinish = false;


    private static float m_fadeTime = 1.5f;//フェードしたい時間（単位は秒

    //遷移先のシーン番号
    private static string m_nextScene;
    //フェード用のCanvasとImage生成
    static void Init()
    {
        //フェード用のCanvas生成
        GameObject FadeCanvasObject = new GameObject("CanvasFade");
        m_fadeCanvas = FadeCanvasObject.AddComponent<Canvas>();
        FadeCanvasObject.AddComponent<GraphicRaycaster>();
        m_fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        FadeCanvasObject.AddComponent<SCR_FadeManager>();

        //最前面になるよう適当なソートオーダー設定
        m_fadeCanvas.sortingOrder = 100;

        //フェード用のImage生成
        m_fadeImage = new GameObject("ImageFade").AddComponent<Image>();
        m_fadeImage.transform.SetParent(m_fadeCanvas.transform, false);
        m_fadeImage.rectTransform.anchoredPosition = Vector3.zero;

        //Imageサイズは適当に大きく設定
        m_fadeImage.rectTransform.sizeDelta = new Vector2(9999, 9999);
    }

    //フェードイン開始
    public static void FadeIn(Color col, float fadetime)
    {
        if (m_fadeImage == null) Init();
        m_fadeImage.color = col;
        m_fadeTime = fadetime;
        m_alpha = 1.0f;
        m_isFadeInFinish = false;
        m_isFadeIn = true;
        Time.timeScale = 0f;//時止める
    }

    //フェードアウト開始
    public static void FadeOut(string scenename, Color col, float fadetime)
    {
        if (m_fadeImage == null) Init();
        m_nextScene = scenename;
        m_fadeTime = fadetime;
        m_fadeImage.color = col;
        m_fadeCanvas.enabled = true;
        m_alpha = 0.0f;
        m_isFadeOut = true;
        m_isFadeOutFinish = false;
        Time.timeScale = 0f; 
        }
    void Update()
    {
        //フラグ有効なら毎フレームフェードイン/アウト処理
        if (m_isFadeIn)
        {
            //経過時間から透明度計算
            m_alpha -= Time.unscaledDeltaTime / m_fadeTime;

            //フェードイン終了判定
            if (m_alpha <= 0.0f)
            {
                m_isFadeIn = false;
                m_alpha = 0.0f;
                m_fadeCanvas.enabled = false;
                Time.timeScale = 1f;//再開
                m_isFadeInFinish = true;
                
            }

            //フェード用Imageの色・透明度設定
            m_fadeImage.color = new Color(0.0f, 0.0f, 0.0f, m_alpha);
        }
        else if (m_isFadeOut)
        {
            //経過時間から透明度計算
            m_alpha += Time.unscaledDeltaTime / m_fadeTime;

            //フェードアウト終了判定
            if (m_alpha >= 1.0f)
            {
                m_isFadeOut = false;
                m_alpha = 1.0f;
                Time.timeScale = 1f;//再開
                m_isFadeOutFinish = true;
                //次のシーンへ遷移
                SceneManager.LoadScene(m_nextScene);
            }

            //フェード用Imageの色・透明度設定
            m_fadeImage.color = new Color(0.0f, 0.0f, 0.0f, m_alpha);
        }
    }
}
