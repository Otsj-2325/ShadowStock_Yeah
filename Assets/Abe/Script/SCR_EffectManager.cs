using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_EffectManager : MonoBehaviour
{
    //ÉVÉìÉOÉãÉgÉì
    public static SCR_EffectManager instance;

    [Header("01Å`12ÇÃî‘çÜèáÇ≈ì¸ÇÍÇÈ")]
    [SerializeField] private List<GameObject> m_EffectList = new List<GameObject>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

            //Sceneà⁄ìÆÇ≈è¡Ç≥ÇÍÇ»Ç¢ÇÊÇ§Ç…Ç∑ÇÈ
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //ä˘Ç…ê∂ê¨Ç≥ÇÍÇƒÇ¢ÇÈÇÃÇ≈Ç†ÇÍÇŒè¡Ç∑
            Destroy(this);
        }
    }

   //001
    public GameObject EFF_UniformSmoke(Vector3 pos, Quaternion rot)
    {
        if (m_EffectList[0])
        {
            return Instantiate(m_EffectList[0], pos, rot);
        }
        else
        {
            Debug.Log("UniformSmoke Effect is not set to m_EffectList[0]");
            return null;
        }
    }

    //002
    public GameObject EFF_DirSmoke(Vector3 pos, Quaternion rot)
    {
        if (m_EffectList[1])
        {
            return Instantiate(m_EffectList[1], pos, rot);
        }
        else
        {
            Debug.Log("DirSmoke Effect is not set to m_EffectList[1]");
            return null;
        }
    }

    //003
    public GameObject EFF_Cut(Vector3 pos, Quaternion rot)
    {
        if (m_EffectList[2])
        {
            return Instantiate(m_EffectList[2], pos, rot);
        }
        else
        {
            Debug.Log("Cut Effect is not set to m_EffectList[2]");
            return null;
        }
    }

    //004
    public GameObject EFF_Paste(Vector3 pos, Quaternion rot)
    {
        if (m_EffectList[3])
        {
            return Instantiate(m_EffectList[3], pos, rot);
        }
        else
        {
            Debug.Log("Paste Effect is not set to m_EffectList[3]");
            return null;
        }
    }

    //005
    public GameObject EFF_KnockBack(Vector3 pos, Quaternion rot)
    {
        if (m_EffectList[4])
        {
            return Instantiate(m_EffectList[4], pos, rot);
        }
        else
        {
            Debug.Log("KnockBack Effect is not set to m_EffectList[4]");
            return null;
        }
    }

    //006
    public GameObject EFF_Wave(Vector3 pos, Quaternion rot)
    {
        if (m_EffectList[5])
        {
            return Instantiate(m_EffectList[5], pos, rot);
        }
        else
        {
            Debug.Log("Wave Effect is not set to m_EffectList[5]");
            return null;
        }
    }

    //007
    public GameObject EFF_Shatter(Vector3 pos, Quaternion rot)
    {
        if (m_EffectList[6])
        {
            return Instantiate(m_EffectList[6], pos, rot);
        }
        else
        {
            Debug.Log("Shatter Effect is not set to m_EffectList[6]");
            return null;
        }
    }

    //008
    public GameObject EFF_Kick(Vector3 pos, Quaternion rot)
    {
        if (m_EffectList[7])
        {
            return Instantiate(m_EffectList[7], pos, rot);
        }
        else
        {
            Debug.Log("Kick Effect is not set to m_EffectList[7]");
            return null;
        }
    }

    //009
    public GameObject EFF_Light(Vector3 pos, Quaternion rot)
    {
        if (m_EffectList[8])
        {
            return Instantiate(m_EffectList[8], pos, rot);
        }
        else
        {
            Debug.Log("Light Effect is not set to m_EffectList[8]");
            return null;
        }
    }

    //010
    public GameObject EFF_Puff(Vector3 pos, Quaternion rot)
    {
        if (m_EffectList[9])
        {
            return Instantiate(m_EffectList[9], pos, rot);
        }
        else
        {
            Debug.Log("Puff Effect is not set to m_EffectList[9]");
            return null;
        }
    }

    //011
    public GameObject EFF_Goal(Vector3 pos, Quaternion rot)
    {
        if (m_EffectList[10])
        {
            return Instantiate(m_EffectList[10], pos, rot);
        }
        else
        {
            Debug.Log("Goal Effect is not set to m_EffectList[10]");
            return null;
        }
    }

    //012
    public GameObject EFF_Rock(Vector3 pos, Quaternion rot)
    {
        if (m_EffectList[11])
        {
            return Instantiate(m_EffectList[11], pos, rot);
        }
        else
        {
            Debug.Log("Rock Effect is not set to m_EffectList[11]");
            return null;
        }
    }

}
