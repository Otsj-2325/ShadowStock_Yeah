using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    //ƒV[ƒ“Ø‚è‘Ö‚¦
    void Change()
    {
        if (SCR_ChangeScene.loadBeforeScene == default) { SCR_FadeManager.FadeOut("Stage1Scene", Color.black, 0.4f);}
        else { SCR_FadeManager.FadeOut(SCR_ChangeScene.loadBeforeScene, Color.black, 0.4f); }
    }
}
