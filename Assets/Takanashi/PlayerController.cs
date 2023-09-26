using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // 移動関連のパラメータ
    [Header("コントローラー操作のスピード")]
    [SerializeField] private float m_Speed = 1.0f;

    [Header("キーボード操作のスピード")]
    [SerializeField] private float speedKeyborad = 2.0f;

    [Header("ジャンプ中スピード(普段のスピードの割合)")]
    [SerializeField] private float speedJump = 0.5f;

    // ジャンプ関係
    public float m_JumpPower;

    [SerializeField] private float m_JumpVelocity = 0.0f;

    [SerializeField] private bool m_IsJumping = false;

    public float knockBackPower;   // ノックバックさせる力


    private Vector3 m_Velocity;

    private Rigidbody cp_Rigidbody;

    private float rayUnderLength;   // 地面着いた判定のレイキャストの長さ

    private Vector3 leaveGroundPosition;    // 地面から離れた時の座標
    private Vector3 beforePosition;         // 1フレーム前の座標

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

    // Start is called before the first frame update
    void Start()
    {
        cp_Rigidbody = GetComponent<Rigidbody>();

        /*==================AbeZone====================*/
        scr_GT = this.transform.Find("GroundTrigger").GetComponent<SCR_GroundTrigger>();
        /*=============================================*/

        stateNow = STATE.GROUND;

        rayUnderLength = transform.localScale.y / 2 - 0.01f;

        leaveGroundPosition = transform.position;
        beforePosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
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

        beforePosition = transform.position;
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
            {
                jump = true;
            }
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
            Debug.Log("back" + leaveGroundPosition.y);
            transform.position = leaveGroundPosition;
        }
    }

    // プレイヤーの移動
    private void PlayerMove(float controllerSpeed, float keyboradSpeed)
    {
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

    //private bool PlayerOnGround()
    //{
    //    // レイキャストにより地面に当たったか判断
    //    // 左
    //    RaycastHit hit;
    //    Vector3 temp = transform.position;
    //    temp.x -= transform.localScale.x / 2;
    //    if (Physics.Raycast(temp, Vector3.down, out hit, rayUnderLength))
    //    {
    //        //Debug.Log("land");
    //        return true;
    //    }
    //    // 右
    //    temp = transform.position;
    //    temp.x += transform.localScale.x / 2;
    //    if (Physics.Raycast(temp, Vector3.down, out hit, rayUnderLength))
    //    {
    //        //Debug.Log("land");
    //        return true;
    //    }
    //    // 手前
    //    temp = transform.position;
    //    temp.z -= transform.localScale.z / 2;
    //    if (Physics.Raycast(temp, Vector3.down, out hit, rayUnderLength))
    //    {
    //        //Debug.Log("land");
    //        return true;
    //    }
    //    // 奥
    //    temp = transform.position;
    //    temp.z += transform.localScale.z / 2;
    //    if (Physics.Raycast(temp, Vector3.down, out hit, rayUnderLength))
    //    {
    //        //Debug.Log("land");
    //        return true;
    //    }

    //    return false;
    //}
}
