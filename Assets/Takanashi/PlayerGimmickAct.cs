using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGimmickAct : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        // êGÇÍÇΩÉMÉ~ÉbÉNÇçÏìÆÇ≥ÇπÇÈ
        if (other.gameObject.GetComponent<IFCGimmck>() != null)
        {
            other.gameObject.GetComponent<IFCGimmck>().GimmckAct();
            SCR_SoundManager.instance.PlaySE(SE_Type.Player_Kick);
            SCR_EffectManager.instance.EFF_Kick(transform.position + transform.forward , transform.rotation);
            gameObject.SetActive(false);
        }
    }
}
