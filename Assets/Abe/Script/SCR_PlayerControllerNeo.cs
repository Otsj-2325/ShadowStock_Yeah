using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class SCR_PlayerControllerNeo : MonoBehaviour
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

    [Header("落下速度倍率")]
    [SerializeField] private float m_FallMultiplier;

    [Header("ノックバック力横軸")]
    [SerializeField] private float knockBackPower;   // ノックバックさせる力

    [Header("ノックバック力縦軸")]
    [SerializeField] private float knockBackUpPower;   // ノックバックさせる力

    [Header("ノックバック操作負荷秒")]
    [SerializeField] private float m_LapseTime = 1.0f;

    private float m_LapseTimer = 0.0f;

    // ジャンプ関係
    public float m_JumpPower;

    [SerializeField] private float m_JumpVelocity = 0.0f;

    [SerializeField] private bool m_IsJumping = false;

    private Animator cp_AMC;

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
    private bool m_isKnockBack = false;
    // ステートマシン
    private enum STATE
    {
        GROUND,
        AIR,
        LAPSE,
    }
    private STATE stateNow;     // 現在のステート

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

    public bool IsKnockBack() { return m_isKnockBack; }
    
    public void SpecialAction() {
        cp_AMC.SetBool("Special", true);
        cp_AMC.SetBool("Walk", false);
        cp_AMC.SetBool("Jump", false);
    }

    public void KnockBack(Transform t) {
        m_JumpVelocity = -1.0f;//着地処理のためのテコ入れ

        cp_Rigidbody.velocity = Vector3.zero;

        // 自分の位置と接触してきたオブジェクトの位置とを計算して、距離と方向を出して正規化(速度ベクトルを算出)
        Vector3 distination = (transform.position - t.position).normalized;
        distination.y = 0.0f;//地面埋まり防止
        cp_Rigidbody.velocity = (distination * knockBackPower) + new Vector3(0.0f, knockBackUpPower, 0.0f);

        m_isKnockBack = true;
        cp_AMC.SetBool("KnockBack", true);
        cp_AMC.SetBool("Walk", false);
        cp_AMC.SetBool("Jump", false);
        cp_AMC.SetBool("Special", false);
        SCR_SoundManager.instance.PlaySE(SE_Type.Player_KnockBack);
        SCR_EffectManager.instance.EFF_KnockBack(transform.position + new Vector3(0.0f, transform.localScale.y, 0.0f), transform.rotation);
        stateNow = STATE.AIR;
    }

    // Start is called before the first frame update
    void Start()
    {
        cp_Rigidbody = GetComponent<Rigidbody>();
        cp_AMC = transform.GetChild(0).gameObject.GetComponent<Animator>();

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

            case STATE.LAPSE:
                LapseProc();
                break;

            default:
                break;
        }

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
        if (Gamepad.current == null)
        {
            if (Input.GetKeyDown(KeyCode.Space)) jump = true;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) || Gamepad.current.buttonSouth.IsPressed())
                jump = true;
        }
        // ジャンプ
        if (jump)
        {
            cp_Rigidbody.velocity = Vector3.zero;
            m_JumpVelocity = m_JumpPower;
            stateNow = STATE.AIR;

            cp_AMC.SetBool("Jump", true);
            SCR_SoundManager.instance.PlaySE(SE_Type.Player_Jump);
            SCR_EffectManager.instance.EFF_DirSmoke(transform.position, transform.rotation);
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
        //RayCast着地処理
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayUnderLength) && cp_Rigidbody.velocity.y < 0)
        {
            if (hit.transform.gameObject.CompareTag("Ground")){
                cp_AMC.SetBool("Jump", false);
                SCR_EffectManager.instance.EFF_UniformSmoke(transform.position, transform.rotation);
            }
        }

        if(m_isKnockBack) cp_Rigidbody.velocity += new Vector3 (0.0f, Physics.gravity.y * Time.deltaTime, 0.0f);
        else { m_JumpVelocity += Physics.gravity.y * Time.deltaTime * m_FallMultiplier; }    

        PlayerMove(m_Speed * speedJump, speedKeyborad * speedJump);
    }

    private void LapseProc()
    {   
        m_LapseTimer += Time.deltaTime;

        if (m_LapseTimer > m_LapseTime)
        {
            m_LapseTimer = 0.0f;
            m_isKnockBack = false;
            stateNow = STATE.GROUND;
            cp_AMC.SetBool("KnockBack", false);
            Debug.Log("End Lapse");
            return;
        }     
      
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 敵に触れたらノックバック
        if (collision.gameObject.CompareTag("Enemy"))
        {
            KnockBack(collision.gameObject.transform);
        }

        // 落下したら元居た場所に戻る
        if (collision.gameObject.CompareTag("PlayerDeleteFloor"))
        {
            transform.position = leaveGroundPosition;
            isFellDown = true;
        }


        if (collision.gameObject.CompareTag("Ground") && m_JumpVelocity < 0.0f)
        {
            m_JumpVelocity = 0.0f;
            cp_Rigidbody.velocity = Vector3.zero;
            SCR_SoundManager.instance.PlaySE(SE_Type.Player_Landing);

            if (m_isKnockBack) { stateNow = STATE.LAPSE; }
            else { stateNow = STATE.GROUND; }
        }
    }

    // プレイヤーの移動
    private void PlayerMove(float controllerSpeed, float keyboradSpeed)
    {
        if (isCreateNewMeshObj) return;
        if (m_isKnockBack) return;
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
            if (m_Velocity != Vector3.zero)
            {
                Quaternion Rotation = Quaternion.LookRotation(m_Velocity.normalized, Vector3.up);
                if (m_Velocity.magnitude != 0)
                {
                    Debug.Log("rotate");
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

        if (stateNow == STATE.GROUND && m_Velocity != Vector3.zero) { cp_AMC.SetBool("Walk", true); }
        else { cp_AMC.SetBool("Walk", false); }

        m_Velocity.y = m_JumpVelocity;
        m_Velocity.y += Physics.gravity.y * Time.deltaTime * m_FallMultiplier;

        cp_Rigidbody.velocity = m_Velocity;
    }
}
