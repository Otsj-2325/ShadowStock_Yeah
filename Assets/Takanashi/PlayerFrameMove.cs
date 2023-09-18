using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFrameMove : MonoBehaviour
{
    [Header("動かすスピード")]
    [SerializeField] private float moveSpeed;

    private Vector3 beforePosition;     // 前フレーム座標
    private Vector2 wallRightUpPos = Vector2.zero;         // 壁の右上端
    private Vector2 wallLeftDownPos = Vector2.zero;         // 壁の右上端
    private List<Transform> frameObj = new List<Transform>();   // 子オブジェクトの枠

    // Start is called before the first frame update
    void Start()
    {
        beforePosition = gameObject.transform.position;

        // 今いる壁を探知
        float posZ = gameObject.transform.position.z;
        GameObject[] gameObjAll = GameObject.FindGameObjectsWithTag("Wall");
        foreach (GameObject temp in gameObjAll)
        {
            float tempZ = temp.gameObject.transform.position.z;

            // 壁のz座標と自分のz座標が離れていれば処理しない
            if (posZ - 1.0f > tempZ || posZ + 1.0f < tempZ) return;

            Vector3 tempScale = temp.transform.localScale / 2;

            // 端点を取得
            wallRightUpPos.x = temp.transform.position.x + tempScale.x;
            wallRightUpPos.y = temp.transform.position.y + tempScale.y;
            wallLeftDownPos.x = temp.transform.position.x - tempScale.x;
            wallLeftDownPos.y = temp.transform.position.y - tempScale.y;
        }

        // 子オブジェクトを取得
        frameObj.Add(transform.GetChild(0));    // left
        frameObj.Add(transform.GetChild(1));    // right
        frameObj.Add(transform.GetChild(2));    // up
        frameObj.Add(transform.GetChild(3));    // down
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        // ゲームパッドが接続されていないとnullになる
        if (Gamepad.current == null) return;

        beforePosition = gameObject.transform.position;

        Vector2 move = Vector3.zero;
        Vector2 rightStick = Gamepad.current.rightStick.ReadValue();

        // 右スティック横
        if (rightStick.x > 0.1f || rightStick.x < -0.1f)
        {
            move.x += rightStick.x;
        }
        // 右スティック縦
        if (rightStick.y > 0.1f || rightStick.y < -0.1f)
        {
            move.y += rightStick.y;
        }

        move = move.normalized;
        move *= moveSpeed;

        Vector3 tempPosition = new Vector3(move.x, move.y, 0.0f);
        gameObject.transform.position += tempPosition;

        // 壁の中に枠があるか
        for(int i = 0; i < frameObj.Count; i++)
        {
            switch (i)
            {
                // Left
                case 0:
                    // 枠内にあれば処理しない
                    if (frameObj[i].position.x > wallLeftDownPos.x) break;

                    gameObject.transform.position = beforePosition;
                    break;

                // Right
                case 1:
                    // 枠内にあれば処理しない
                    if (frameObj[i].position.x < wallRightUpPos.x) break;

                    gameObject.transform.position = beforePosition;
                    break;

                // Up
                case 2:
                    if (frameObj[i].position.y < wallRightUpPos.y) break;

                    gameObject.transform.position = beforePosition;
                    break;

                // Down
                case 3:
                    if (frameObj[i].position.y > wallLeftDownPos.y) break;

                    gameObject.transform.position = beforePosition;
                    break;

                default:
                    break;
            }
        }
    }
}
