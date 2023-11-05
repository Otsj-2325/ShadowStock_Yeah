using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGimmickAct : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        // �M�~�b�N�쓮
        // �L�[��������Ă��Ȃ���Ώ������Ȃ�
        if (Gamepad.current == null)
        {
            if (!Input.GetKeyDown(KeyCode.E))
            {
                return;
            }
        }
        else
        {
            if (!Input.GetKeyDown(KeyCode.E) &&
               !Gamepad.current.leftTrigger.isPressed)
            {
                return;
            }
        }

        // �G�ꂽ�M�~�b�N���쓮������
        if (other.gameObject.GetComponent<IFCGimmck>() != null)
        {
            other.gameObject.GetComponent<IFCGimmck>().GimmckAct();
            SCR_SoundManager.instance.PlaySE(SE_Type.Player_Kick);
            SCR_EffectManager.instance.EFF_Kick(transform.position, transform.rotation);
        }
    }
}
