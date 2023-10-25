using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    [Header("地面のオブジェクト")]
    [SerializeField] private GameObject[] groundObjects;

    // 引数の座標から見た地面の場所(現在影のオブジェクト生成のみ使用)
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
                    // もしも全部の床より低ければエラー
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
