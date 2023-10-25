using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrial : MonoBehaviour
{
    private float Vol = 0.0f;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) { SCR_SoundManager.instance.PlaySE(SE_Type.Camera_In); }
        if (Input.GetKeyDown(KeyCode.X)) { SCR_SoundManager.instance.PlaySE(SE_Type.Camera_Out); }
        if (Input.GetKeyDown(KeyCode.C)) { SCR_SoundManager.instance.PlaySE(SE_Type.System_Decision); }
        if (Input.GetKeyDown(KeyCode.V)) { SCR_SoundManager.instance.PlaySE(SE_Type.System_Pose); }
        if (Input.GetKeyDown(KeyCode.B)) { SCR_SoundManager.instance.PlaySE(SE_Type.System_Select); }
        if (Input.GetKeyDown(KeyCode.N)) { SCR_SoundManager.instance.PlaySE(SE_Type.Player_Walk); }
        if (Input.GetKeyDown(KeyCode.M)) { SCR_SoundManager.instance.PlaySE(SE_Type.Player_Jump); }
        if (Input.GetKeyDown(KeyCode.L)) { SCR_SoundManager.instance.PlaySE(SE_Type.Player_Landing); }
        if (Input.GetKeyDown(KeyCode.K)) { SCR_SoundManager.instance.PlaySE(SE_Type.Player_KnockBack); }
        if (Input.GetKeyDown(KeyCode.J)) { SCR_SoundManager.instance.PlaySE(SE_Type.Player_Kick); }
        if (Input.GetKeyDown(KeyCode.UpArrow)) { Vol += 0.1f; SCR_SoundManager.instance.SetVolumeSE(Vol); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { Vol -= 0.1f; SCR_SoundManager.instance.SetVolumeSE(Vol); }
    }
}
