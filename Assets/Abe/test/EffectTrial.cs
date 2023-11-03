using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTrial : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) { SCR_EffectManager.instance.EFF_UniformSmoke(this.transform.position, this.transform.rotation); }
        if (Input.GetKeyDown(KeyCode.W)) { SCR_EffectManager.instance.EFF_DirSmoke(this.transform.position, this.transform.rotation); }
        if (Input.GetKeyDown(KeyCode.E)) { SCR_EffectManager.instance.EFF_Cut(this.transform.position, this.transform.rotation); }
        if (Input.GetKeyDown(KeyCode.R)) { SCR_EffectManager.instance.EFF_Goal(this.transform.position, this.transform.rotation); }
        if (Input.GetKeyDown(KeyCode.T)) { SCR_EffectManager.instance.EFF_Kick(this.transform.position, this.transform.rotation); }
        if (Input.GetKeyDown(KeyCode.Y)) { SCR_EffectManager.instance.EFF_KnockBack(this.transform.position, this.transform.rotation); }
        if (Input.GetKeyDown(KeyCode.U)) { SCR_EffectManager.instance.EFF_Light(this.transform.position, this.transform.rotation); }
        if (Input.GetKeyDown(KeyCode.I)) { SCR_EffectManager.instance.EFF_Paste(this.transform.position, this.transform.rotation); }
        if (Input.GetKeyDown(KeyCode.O)) { SCR_EffectManager.instance.EFF_Puff(this.transform.position, this.transform.rotation); }
        if (Input.GetKeyDown(KeyCode.P)) { SCR_EffectManager.instance.EFF_Rock(this.transform.position, this.transform.rotation); }
        if (Input.GetKeyDown(KeyCode.A)) { SCR_EffectManager.instance.EFF_Shatter(this.transform.position, this.transform.rotation); }
        if (Input.GetKeyDown(KeyCode.S)) { SCR_EffectManager.instance.EFF_Wave(this.transform.position, this.transform.rotation); }
        if (Input.GetKeyDown(KeyCode.D)) { SCR_EffectManager.instance.EFF_KumoAuraRed(this.transform.position, this.transform.rotation); }
        if (Input.GetKeyDown(KeyCode.F)) { SCR_EffectManager.instance.EFF_KumoAuraYellow(this.transform.position, this.transform.rotation); }
        if (Input.GetKeyDown(KeyCode.G)) { SCR_EffectManager.instance.EFF_HakoAuraRed(this.transform.position, this.transform.rotation); }
        if (Input.GetKeyDown(KeyCode.H)) { SCR_EffectManager.instance.EFF_KumoAuraYellow(this.transform.position, this.transform.rotation); }
    }
}
