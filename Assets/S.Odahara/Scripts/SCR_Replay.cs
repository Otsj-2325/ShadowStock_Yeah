using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_Replay: MonoBehaviour
{
    //シーン切り替え
    public void Change()
    {
        if (SCR_ChangeScene.loadBeforeScene == default) { SCR_FadeManager.FadeOut("Stage1Scene", Color.black, 0.4f);}
        else { SCR_FadeManager.FadeOut(SCR_ChangeScene.loadBeforeScene, Color.black, 0.4f); }
    }
}
