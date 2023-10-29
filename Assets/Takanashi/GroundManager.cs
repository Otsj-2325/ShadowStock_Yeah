using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    [Header("�n�ʂ̃I�u�W�F�N�g")]
    [SerializeField] private GameObject[] groundObjects;

    // �����̍��W���猩���n�ʂ̏ꏊ(���݉e�̃I�u�W�F�N�g�����̂ݎg�p)
    public Vector3 GetGroundPosition(Vector3 position, out Vector3 scale)
    {
        for(int i = 0; i < groundObjects.Length; i++)
        {
            if (i == groundObjects.Length - 1)
            {
                scale = groundObjects[i].transform.localScale;
                return groundObjects[i].transform.position;
            }

            if (position.y < groundObjects[i + 1].transform.position.y)
            {
                if(i == 0)
                {
                    // �������S���̏����Ⴏ��΃G���[
                    if (position.y < groundObjects[i].transform.position.y)
                    {
                        scale = Vector3.zero;
                        return Vector3.zero;
                    }
                        
                }
                scale = groundObjects[i].transform.localScale;
                return groundObjects[i].transform.position;
            }
        }

        scale = Vector3.zero;
        return Vector3.zero;
    }
}
