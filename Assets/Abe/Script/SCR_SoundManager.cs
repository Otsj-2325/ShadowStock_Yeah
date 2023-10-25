using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_SoundManager : MonoBehaviour
{
   public static SCR_SoundManager instance;

    private void Awake()
    {
        if (instance == null)
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
    
}
