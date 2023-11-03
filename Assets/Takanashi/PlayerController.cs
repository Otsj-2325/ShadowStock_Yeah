using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    // 移動関連のパラメータ
    [Header("コントローラー操作のスピード")]
    [SerializeField] private float m_Speed = 1.0f;

    [Header("キーボード操作のスピード")]
    [SerializeField] private float speedKeyborad = 2.0f;

    [Header("ジャンプ中スピード(普段のスピードの割合)")]
    [SerializeField] private float speedJump = 0.5f;

    [Header("落下した後の停止時間")]
    [SerializeField] private float stopTime = 1.5f;

    // ジャンプ関係
    public float m_JumpPower;

    [SerializeField] private float m_JumpVelocity = 0.0f;

    [SerializeField] private bool m_IsJumping = false;

    public float knockBackPower;   // ノックバックさせる力


    private Vector3 m_Velocity;

    private Rigidbody cp_Rigidbody;

    private float rayUnderLength;   // 地面着いた判定のレイキャストの長さ

    // 地面から離れた際の処理の変数
    private Vector3 leaveGroundPosition;    // 地面から離れた時の座標
    private Vector3 beforePosition;         // 数フレーム前の座標
    private int beforeFrameNum = 10;        // 上のフレーム数の調整
    private int frameCount;

    private Action warp;

    private bool isFellDown;    // 落下したか
    private float time;         // 落下後の停止時間測定

    private bool isCreateNewMeshObj = false;    // 現在影からのオブジェクト作成中か

    // ステートマシン
    private enum STATE
    {
        GROUND,
        AIR
    }
    private STATE stateNow;     // 現在のステート

    /*==================AbeZone====================*/
    private SCR_GroundTrigger scr_GT;
    /*=============================================*/

    public void SetWarp(Action action)
    {
        warp = action;
    }

    public void SetVectorZero()
    {
        m_Velocity = Vector3.zero;
    }

    public void CreateNewMeshObjFromShadow()
    {
        isCreateNewMeshObj = true;
    }

    public void CreateEndNewMeshObjFromShadow()
    {
        isCreateNewMeshObj = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        cp_Rigidbody = GetComponent<Rigidbody>();


        stateNow = STATE.GROUND;

        rayUnderLength = transform.localScale.y / 2 - 0.01f;

        leaveGroundPosition = transform.position;
        beforePosition = transform.position;
        frameCount = 0;

        isFellDown = false;
        time = 0.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 落下した
        if (isFellDown)
        {
            time += Time.deltaTime;
         
            // 停止時間を過ぎていないなら処理しない
            if (time < stopTime) return;

            isFellDown = false;
            time = 0f;
        }

        // ステートマシン
        switch (stateNow)
        {
            case STATE.GROUND:
                MoveProc();
                break;

            case STATE.AIR:
                JumpProc();
                break;

            default:
                break;
        }

        ActionProc();
        CutProc();
        PasteProc();

        // beforeFrameNum数のフレーム前の座標を取得
        if (++frameCount > beforeFrameNum)
        {
            frameCount = 0;
            beforePosition = transform.position;
        }
    }

    void MoveProc()
    {
        bool jump = false;
        if(Gamepad.current == null)
        {
            if(Input.GetKeyDown(KeyCode.Space))     jump = true;
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Space) || Gamepad.current.buttonSouth.isPressed)
                jump = true;
        }
        // ジャンプ
        if (jump)
        {
            cp_Rigidbody.velocity = Vector3.zero;
            cp_Rigidbody.AddForce(Vector3.up * m_JumpPower);
            stateNow = STATE.AIR;
            return;     // ジャンプしたらステートマシン変更のため処理終了
        }

        // ジャンプせずに落下したら
        if (cp_Rigidbody.velocity.y < -0.98f)
        {
            stateNow = STATE.AIR;
            leaveGroundPosition = beforePosition;
            return;     // 落下したらステートマシン変更のため処理終了
        }

        PlayerMove(m_Speed, speedKeyborad);
    }

    void JumpProc()
    {
        // レイキャストにより地面に当たったか判断
        if (cp_Rigidbody.velocity.y == 0.0f)
        {
            cp_Rigidbody.velocity = Vector3.zero;
            stateNow = STATE.GROUND;
            return;
        }

        PlayerMove(m_Speed * speedJump, speedKeyborad * speedJump);
    }

    void ActionProc()
    {
        if (Gamepad.current.buttonEast.isPressed)
        {
            Debug.Log("アクション中");
        }

        if (Input.GetKey(KeyCode.J))
        {
            Debug.Log("アクション中");
        }

    }

    void CutProc()
    {
        if (Gamepad.current.buttonNorth.isPressed)
        {
            Debug.Log("カット中");
        }

        if (Input.GetKey(KeyCode.K))
        {
            Debug.Log("カット中");
        }
    }

    void PasteProc()
    {
        if (Gamepad.current.buttonWest.isPressed)
        {
            Debug.Log("ペースト中");
        }

        if (Input.GetKey(KeyCode.P))
        {
            Debug.Log("ペースト中");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 敵に触れたらノックバック
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("当たった");
            cp_Rigidbody.velocity = Vector3.zero;

            // 自分の位置と接触してきたオブジェクトの位置とを計算して、距離と方向を出して正規化(速度ベクトルを算出)
            Vector3 distination = (transform.position - collision.transform.position).normalized;

            cp_Rigidbody.AddForce(distination * knockBackPower, ForceMode.VelocityChange);
        }

        // 落下したら元居た場所に戻る
        if (collision.gameObject.CompareTag("PlayerDeleteFloor"))
        {
            transform.position = leaveGroundPosition;
            isFellDown = true;
        }
    }

    // プレイヤーの移動
    private void PlayerMove(float controllerSpeed, float keyboradSpeed)
    {
        if (isCreateNewMeshObj) return;

        // 初期化
        m_Velocity = Vector3.zero;

        // コントローラー
        if (Gamepad.current != null)
        {
            Vector2 leftStick = Gamepad.current.leftStick.ReadValue();
            if (leftStick.x > 0.1f || leftStick.x < -0.1f)
            {
                m_Velocity.x = controllerSpeed * leftStick.x;
            }
            if (leftStick.y > 0.1f || leftStick.y < -0.1f)
            {
                m_Velocity.z = controllerSpeed * leftStick.y;
            }

            // 前を向かせる
            if(m_Velocity != Vector3.zero)
            {
                Quaternion Rotation = Quaternion.LookRotation(m_Velocity.normalized, Vector3.up);
                if (m_Velocity.magnitude != 0)
                {
                    this.transform.rotation = Rotation;
                }
            }
        }

        // キーボード
        // Wキー（前方移動）
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 temp = new Vector3(0.0f, 0.0f, 0.1f * keyboradSpeed);
            transform.position += temp;
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        // Sキー（後方移動）
        if (Input.GetKey(KeyCode.S))
        {
            //transform.position -= m_Speed * transform.forward * Time.deltaTime;
            Vector3 temp = new Vector3(0.0f, 0.0f, -0.1f * keyboradSpeed);
            transform.position += temp;
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
        // Dキー（右移動）
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 temp = new Vector3(0.1f * keyboradSpeed, 0.0f, 0.0f);
            transform.position += temp;
            transform.eulerAngles = new Vector3(0f, 90.0f, 0f);
        }
        // Aキー（左移動）
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 temp = new Vector3(-0.1f * keyboradSpeed, 0.0f, 0.0f);
            transform.position += temp;
            transform.eulerAngles = new Vector3(0f, -90.0f, 0f);
        }

        cp_Rigidbody.AddForce(m_Velocity);
    }
}
