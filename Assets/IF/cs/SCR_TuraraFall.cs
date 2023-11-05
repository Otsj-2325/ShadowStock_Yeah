using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]

public class SCR_TuraraFall : MonoBehaviour
{
    private Rigidbody cp_Rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        cp_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" )
        {
            cp_Rigidbody.useGravity = true;
        }
    }

    void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag == ("Ground"))
        {
            //Debug.Log("è∞Ç…ìñÇΩÇ¡ÇΩÇ©ÇÁè¡Ç∑ÇÊ");
            //Destroy(this.gameObject, 0.1f);
            SCR_EffectManager.instance.EFF_Rock(transform.position, transform.rotation);
            SCR_SoundManager.instance.PlaySE(SE_Type.Gimmick_Rubble);
        }
        if (collider.gameObject.tag == ("Player"))
        {
            //Debug.Log("ÉvÉåÉCÉÑÅ[Ç…ìñÇΩÇ¡ÇΩÇ©ÇÁè¡Ç∑ÇÊ");
            // Destroy(this.gameObject, 0.1f);
            SCR_EffectManager.instance.EFF_Rock(transform.position, transform.rotation);
            SCR_SoundManager.instance.PlaySE(SE_Type.Gimmick_Rubble);
        }
    }
}
