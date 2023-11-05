using System.Collections;
using UnityEngine;
using DG.Tweening;

public enum BGM_Type
{ //BGM����
    TITLE = 0,
    SELECT,
    GAME,
    RESULT,
    SILENCE//�ꉞ�����p
}

public enum SE_Type
{ //SE����
    Camera_In,
    Camera_Out,
    System_Decision,
    System_Pose,
    System_Select,
    Player_Walk,
    Player_Jump,
    Player_Landing,
    Player_KnockBack,
    Player_Kick,
    Player_Cut,
    Player_Paste,
    Gimmick_Rubble,
    Gimmick_Light,
    System_Goal,
    System_NextLevel,
    PYON_Landing,
    PYON_Jump,
    KAKU_Walk,
}
public class SCR_SoundManager : MonoBehaviour
{
    //�V���O���g��
    public static SCR_SoundManager instance;

    [Header("BGM�؂�ւ�����")]
    [SerializeField]
    private float m_CrossFadeTime = 2.0f;

    [Header("BGM�{�����[��")]
    [SerializeField]
    private float m_BgmVolume = 1.0f;

    [Header("TITLE��SELECT��GAME��RESULT�̏���")]
    public AudioClip[] m_BgmClips;
    [Header("SE_Type�̏���")]
    public AudioClip[] m_SeClips;

    private AudioSource[] m_BgmSources = new AudioSource[2];//����(�t�F�[�h�p)�ɗ�����BGM 
    private AudioSource[] m_SeSources = new AudioSource[30];//�����ɗ�����SE 

    private bool m_CrossFading;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            //Scene�ړ��ŏ�����Ȃ��悤�ɂ���
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //���ɐ�������Ă���̂ł���Ώ���
            Destroy(this);
        }

        // AudioSource�ǉ�
        m_BgmSources[0] = gameObject.AddComponent<AudioSource>();
        m_BgmSources[1] = gameObject.AddComponent<AudioSource>();

        for (int i = 0; i < m_SeSources.Length; i++)
        {
            m_SeSources[i] = gameObject.AddComponent<AudioSource>();
        }
    }



    void Update()
    {
        // �{�����[���ݒ�
        if (!m_CrossFading)
        {
            m_BgmSources[0].volume = m_BgmVolume;
            m_BgmSources[1].volume = m_BgmVolume;
        }
    }

    //���[�v�������Ȃ����̂ݑ�������false
    public void PlayBGM(BGM_Type bgmType, bool loopFlg = true)
    {
        // BGM�Ȃ��̏�Ԃɂ���ꍇ            
        if (bgmType == BGM_Type.SILENCE)
        {
            StopAllBGM();
            return;
        }

        int index = (int)bgmType;

        if (index < 0 || m_BgmClips.Length <= index){ return; }

        // ����BGM�̏ꍇ�͉������Ȃ�
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
            //�N���X�t�F�[�h�̂��߂̃R���[�`��
            StartCoroutine(CrossFadeChangeBGM(index, loopFlg));
        }
    }

    //�N���X�t�F�[�h����
    private IEnumerator CrossFadeChangeBGM(int index, bool loopFlg)
    {
        m_CrossFading = true;
        if (m_BgmSources[0].clip != null)// [0]�̉��ʂ����X�ɉ����āA[1]���Đ�
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
        else // [1]�̉��ʂ����X�ɉ����āA[0]���Đ�
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

    //BGM�ꎞ��~
    public void PauseBGM()
    {
        m_BgmSources[0].Stop();
        m_BgmSources[1].Stop();
    }

    //BGM���X�^�[�g
    public void RestartBGM()
    {
        m_BgmSources[0].Play();
        m_BgmSources[1].Play();
    }

    //BGM�S����
    public void StopAllBGM()
    {
        m_BgmSources[0].Stop();
        m_BgmSources[0].clip = null;
        m_BgmSources[1].Stop();
        m_BgmSources[1].clip = null;
    }

    //BGM�{�����[���ݒ�(0.0f�`1.0f)
    public void SetVolumeBGM(float vol)
    {
        if(vol > 1.0f) { vol = 1.0f; }
        if(vol < 0.0f) { vol = 0.0f; }

        m_BgmVolume = vol;
    }

    /*�ȉ�SE���\�b�h*/

    // SE�Đ�    
    public void PlaySE(SE_Type seType, bool loopFlg = false, float vol = 1.0f)
    {
        bool play = false;
        int index = (int)seType;
        if (index < 0 || m_SeClips.Length <= index) { return; }

        // �Đ����ł͂Ȃ�AudioSource��������SE��炷
        foreach (AudioSource s in m_SeSources)
        {
            if (s.isPlaying) { continue; }

            s.clip = m_SeClips[index];
            s.loop = loopFlg;
            s.volume = vol;
            s.Play();
            play = true;
            break;
        }

        if (!play) { Debug.Log("Can't Play SE : m_SeClips Over"); }
    }

   //SE�S����
    public void StopAllSE()
    {
        foreach (AudioSource s in m_SeSources)
        {
            s.Stop();
            s.clip = null;
        }
    }

    public void StopLoopSE()
    {
        foreach (AudioSource s in m_SeSources)
        {
            if (s.loop)
            {
                s.Stop();
                s.clip = null;
            }
        }
    }
}
