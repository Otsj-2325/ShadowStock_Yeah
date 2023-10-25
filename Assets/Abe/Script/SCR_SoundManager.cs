using System.Collections;
using UnityEngine;
using DG.Tweening;

public enum BGM_Type
{ //BGM命名
    TITLE = 0,
    SELECT,
    GAME,
    RESULT,
    SILENCE//一応無音用
}

public enum SE_Type
{ //SE命名
    Test01,
    Test02,

}
public class SCR_SoundManager : MonoBehaviour
{
    //シングルトン
    public static SCR_SoundManager instance;

    [Header("BGM切り替え時間")]
    [SerializeField]
    private float m_CrossFadeTime = 2.0f;

    [Header("BGMボリューム")]
    [SerializeField]
    private float m_BgmVolume = 1.0f;

    [Header("SEボリューム")]
    [SerializeField]
    private float m_SeVolume = 1.0f;

    [Header("TITLE→SELECT→GAME→RESULTの順で")]
    public AudioClip[] m_BgmClips;
    [Header("SE_Typeの順で")]
    public AudioClip[] m_SeClips;

    private AudioSource[] m_BgmSources = new AudioSource[2];//同時(フェード用)に流せるBGM 2
    private AudioSource[] m_SeSources = new AudioSource[10];//同時に流せるSE 10

    private bool m_CrossFading;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            //Scene移動で消されないようにする
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //既に生成されているのであれば消す
            Destroy(this);
        }

        // AudioSource追加
        m_BgmSources[0] = gameObject.AddComponent<AudioSource>();
        m_BgmSources[1] = gameObject.AddComponent<AudioSource>();

        for (int i = 0; i < m_SeSources.Length; i++)
        {
            m_SeSources[i] = gameObject.AddComponent<AudioSource>();
        }
    }



    void Update()
    {
        // ボリューム設定
        if (!m_CrossFading)
        {
            m_BgmSources[0].volume = m_BgmVolume;
            m_BgmSources[1].volume = m_BgmVolume;
        }

        foreach (AudioSource s in m_SeSources)
        {
            s.volume = m_SeVolume;
        }
    }

    //ループしたくない時のみ第二引数にfalse
    public void PlayBGM(BGM_Type bgmType, bool loopFlg = true)
    {
        // BGMなしの状態にする場合            
        if (bgmType == BGM_Type.SILENCE)
        {
            StopAllBGM();
            return;
        }

        int index = (int)bgmType;

        if (index < 0 || m_BgmClips.Length <= index){ return; }

        // 同じBGMの場合は何もしない
        if (m_BgmSources[0].clip == m_BgmClips[index] && m_BgmSources[0].clip != null) { return; }
        else if (m_BgmSources[1].clip == m_BgmClips[index] && m_BgmSources[1].clip != null){ return; }

        if (m_BgmSources[0].clip == null && m_BgmSources[1].clip == null)
        {
            m_BgmSources[0].loop = loopFlg;
            m_BgmSources[0].clip = m_BgmClips[index];
            m_BgmSources[0].Play();
        }
        else
        {
            //クロスフェードのためのコルーチン
            StartCoroutine(CrossFadeChangeBGM(index, loopFlg));
        }
    }

    //クロスフェード処理
    private IEnumerator CrossFadeChangeBGM(int index, bool loopFlg)
    {
        m_CrossFading = true;
        if (m_BgmSources[0].clip != null)// [0]の音量を徐々に下げて、[1]を再生
        {
            m_BgmSources[1].volume = 0;
            m_BgmSources[1].clip = m_BgmClips[index];
            m_BgmSources[1].loop = loopFlg;
            m_BgmSources[1].Play();

            m_BgmSources[1].DOFade(1.0f, m_CrossFadeTime).SetEase(Ease.Linear);
            m_BgmSources[0].DOFade(0, m_CrossFadeTime).SetEase(Ease.Linear);

            yield return new WaitForSeconds(m_CrossFadeTime);
            m_BgmSources[0].Stop();
            m_BgmSources[0].clip = null;
        }
        else // [1]の音量を徐々に下げて、[0]を再生
        {
            m_BgmSources[0].volume = 0;
            m_BgmSources[0].clip = m_BgmClips[index];
            m_BgmSources[0].loop = loopFlg;
            m_BgmSources[0].Play();

            m_BgmSources[0].DOFade(1.0f, m_CrossFadeTime).SetEase(Ease.Linear);
            m_BgmSources[1].DOFade(0, m_CrossFadeTime).SetEase(Ease.Linear);

            yield return new WaitForSeconds(m_CrossFadeTime);
            m_BgmSources[1].Stop();
            m_BgmSources[1].clip = null;
        }
        m_CrossFading = false;
    }

    //BGM一時停止
    public void PauseBGM()
    {
        m_BgmSources[0].Stop();
        m_BgmSources[1].Stop();
    }

    //BGMリスタート
    public void RestartBGM()
    {
        m_BgmSources[0].Play();
        m_BgmSources[1].Play();
    }

    //BGM全消し
    public void StopAllBGM()
    {
        m_BgmSources[0].Stop();
        m_BgmSources[0].clip = null;
        m_BgmSources[1].Stop();
        m_BgmSources[1].clip = null;
    }

    //BGMボリューム設定(0.0f〜1.0f)
    public void SetVolumeBGM(float vol)
    {
        if(vol > 1.0f) { vol = 1.0f; }
        if(vol < 0.0f) { vol = 0.0f; }

        m_BgmVolume = vol;
    }

    /*以下SEメソッド*/

    // SE再生    
    public void PlaySE(SE_Type seType)
    {
        bool play = false;
        int index = (int)seType;
        if (index < 0 || m_SeClips.Length <= index) { return; }

        // 再生中ではないAudioSourceをつかってSEを鳴らす
        foreach (AudioSource s in m_SeSources)
        {
            if (s.isPlaying) { continue; }

            s.clip = m_SeClips[index];
            s.Play();
            play = true;
            break;
        }

        if (!play) { Debug.Log("Can't Play SE : m_SeClips Over"); }
    }

   //SE全消し
    public void StopAllSE()
    {
        foreach (AudioSource s in m_SeSources)
        {
            s.Stop();
            s.clip = null;
        }
    }

    //SEボリューム設定(0.0f〜1.0f)
    public void SetVolumeSE(float vol)
    {
        if (vol > 1.0f) { vol = 1.0f; }
        if (vol < 0.0f) { vol = 0.0f; }

        m_SeVolume = vol;
    }
}
