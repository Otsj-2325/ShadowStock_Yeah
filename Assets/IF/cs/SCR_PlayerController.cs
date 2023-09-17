using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class SCR_PlayerController : MonoBehaviour
{
    // 移動関連のパラメータ
    [Header("速さの最低値")]
    [SerializeField]
    private float m_MinSpeed = 1.0f;
    [Header("速さの最高値")]
    [SerializeField]
    private float m_MaxSpeed = 3.0f;
    // 移動速度
    private Vector3 m_Velocity;
    float m_Speed = 3.0f;
    // ジャンプ関係
    public float m_JumpPower;
    [SerializeField] private float m_JumpVelocity = 0.0f;

    private Rigidbody cp_Rigidbody;
    [SerializeField] private bool m_IsJumping = false;

    private float m_Glavity = 9.8f;

    public float knockBackPower;   // ノックバックさせる力

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
    }

// Update is called once per frame
    void Update()
    {
        // ゲームパッドが接続されていないとnullになる。
        if (Gamepad.current == null) return;

        MoveProc();
        JumpProc();
        ActionProc();
        CutProc();
        PasteProc();

        m_Velocity.y = m_JumpVelocity;
        cp_Rigidbody.velocity = m_Velocity;
    }

    void MoveProc()
    {
        // 初期化
        m_Velocity = Vector3.zero;

        // コントローラー
        Vector2 leftStick = Gamepad.current.leftStick.ReadValue();
        if (leftStick.x > 0.1f || leftStick.x < -0.1f)
        {
            m_Velocity.x = m_Speed * leftStick.x;
        }
        if (leftStick.y > 0.1f || leftStick.y < -0.1f)
        {
            m_Velocity.z = m_Speed * leftStick.y;
        }


        // キーボード
        // Wキー（前方移動）
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += m_Speed * transform.forward * Time.deltaTime;
        }
        // Sキー（後方移動）
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= m_Speed * transform.forward * Time.deltaTime;
        }
        // Dキー（右移動）
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += m_Speed * transform.right * Time.deltaTime;
        }
        // Aキー（左移動）
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= m_Speed * transform.right * Time.deltaTime;
        }


        Quaternion Rotation = Quaternion.LookRotation(m_Velocity.normalized, Vector3.up);
        if(m_Velocity.magnitude != 0)
        {
            this.transform.rotation = Rotation;
        }

       
    }
    void JumpProc()
    {
        if (Gamepad.current.buttonSouth.isPressed && !m_IsJumping)
        {
            m_JumpVelocity = m_JumpPower;
            //Debug.Log("ジャンプ");
            m_IsJumping = true;
        }
        
        if (Input.GetKey(KeyCode.Space) && !m_IsJumping)
        {
            cp_Rigidbody.velocity = Vector3.up * m_JumpPower;
            //Debug.Log("ジャンプ");
            m_IsJumping = true;
        }

        // 落下
        if (m_IsJumping == true || !scr_GT.IsGround())
        {
            m_JumpVelocity -= m_Glavity * Time.deltaTime;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            m_IsJumping = false;
            m_JumpVelocity = 0.0f;
        }

        // 敵に触れたらノックバック
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("当たった");
            cp_Rigidbody.velocity = Vector3.zero;

            // 自分の位置と接触してきたオブジェクトの位置とを計算して、距離と方向を出して正規化(速度ベクトルを算出)
            Vector3 distination = (transform.position - collision.transform.position).normalized;

            cp_Rigidbody.AddForce(distination * knockBackPower, ForceMode.VelocityChange);
        }

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

}

