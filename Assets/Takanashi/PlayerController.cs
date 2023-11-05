using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    [Header("�S�[���̃I�u�W�F�N�g")]
    [SerializeField] private SCR_Goal m_SCR_Goal = default!;

    [Header("���U���g��\������J�����@->�@Vcam04")]
    [SerializeField] private Transform resultCamera;

    // �ړ��֘A�̃p�����[�^
    [Header("�R���g���[���[����̃X�s�[�h")]
    [SerializeField] private float m_Speed = 1.0f;

    [Header("�L�[�{�[�h����̃X�s�[�h")]
    [SerializeField] private float speedKeyborad = 2.0f;

    [Header("�W�����v���X�s�[�h(���i�̃X�s�[�h�̊���)")]
    [SerializeField] private float speedJump = 0.5f;

    [Header("����������̒�~����")]
    [SerializeField] private float stopTime = 1.5f;

    [Header("�������x�{��")]
    [SerializeField] private float m_FallMultiplier;

    [Header("�m�b�N�o�b�N�͉���")]
    [SerializeField] private float knockBackPower;   // �m�b�N�o�b�N�������

    [Header("�m�b�N�o�b�N�͏c��")]
    [SerializeField] private float knockBackUpPower;   // �m�b�N�o�b�N�������

    [Header("�m�b�N�o�b�N���앉�וb")]
    [SerializeField] private float m_LapseTime = 1.0f;

    [Header("�L�b�N�L���b��")]
    [SerializeField] private float m_KickTime = 0.5f;

    [Header("�L�b�N�����b��")]
    [SerializeField] private float m_LapseKickTime = 0.25f;


    [SerializeField] PlayerCreateShadowMulti scr_PCS;

    private GameObject m_GimmickAct;

    private float m_LapseTimer = 0.0f;
    private float m_KickTimer = 0.0f;

    // �W�����v�֌W
    [SerializeField] public float m_JumpPower;

    [SerializeField] private float m_JumpVelocity = 0.0f;

    [SerializeField] private bool m_IsJumping = false;

    private Animator cp_AMC;

    private Vector3 m_Velocity;

    private Rigidbody cp_Rigidbody;

    private float rayUnderLength;   // �n�ʒ���������̃��C�L���X�g�̒���

    // �n�ʂ��痣�ꂽ�ۂ̏����̕ϐ�
    private Vector3 leaveGroundPosition;    // �n�ʂ��痣�ꂽ���̍��W
    private Vector3 beforePosition;         // ���t���[���O�̍��W
    private int beforeFrameNum = 25;        // ��̃t���[�����̒���
    private int frameCount;

    private Action warp;

    private bool isFellDown;    // ����������
    private float time;         // ������̒�~���ԑ���

    private bool isCreateNewMeshObj = false;    // ���݉e����̃I�u�W�F�N�g�쐬����
    private bool m_isKnockBack = false;
    public bool m_CanKick = true;
    // �X�e�[�g�}�V��
    private enum STATE
    {
        GROUND,
        AIR,
        LAPSE,
    }
    private STATE stateNow;     // ���݂̃X�e�[�g


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
        cp_AMC.SetBool("Cut", false);
        SCR_SoundManager.instance.PlaySE(SE_Type.Player_Paste, false ,0.4f);
    }

    public bool IsKnockBack() { return m_isKnockBack; }

    public void KickAction()
    {
        SCR_SoundManager.instance.StopLoopSE();
        cp_AMC.SetBool("Special", true);
        cp_AMC.SetBool("Walk", false);
        cp_AMC.SetBool("Jump", false);
    }

    public void KnockBack(Transform t)
    {
        m_JumpVelocity = -1.0f;//���n�����̂��߂̃e�R����

        cp_Rigidbody.velocity = Vector3.zero;

        // �����̈ʒu�ƐڐG���Ă����I�u�W�F�N�g�̈ʒu�Ƃ��v�Z���āA�����ƕ������o���Đ��K��(���x�x�N�g�����Z�o)
        Vector3 distination = (transform.position - t.position).normalized;
        distination.y = 0.0f;//�n�ʖ��܂�h�~
        cp_Rigidbody.velocity = (distination * knockBackPower) + new Vector3(0.0f, knockBackUpPower, 0.0f);

        m_isKnockBack = true;
        cp_AMC.SetBool("KnockBack", true);
        cp_AMC.SetBool("Walk", false);
        cp_AMC.SetBool("Jump", false);
        cp_AMC.SetBool("Special", false);
        SCR_SoundManager.instance.StopLoopSE();
        SCR_SoundManager.instance.PlaySE(SE_Type.Player_KnockBack);
        SCR_EffectManager.instance.EFF_KnockBack(transform.position + new Vector3(0.0f, transform.localScale.y, 0.0f), transform.rotation);
        stateNow = STATE.AIR;
    }
    // Start is called before the first frame update
    void Start()
    {
        cp_Rigidbody = GetComponent<Rigidbody>();
        cp_AMC = transform.GetChild(0).gameObject.GetComponent<Animator>();
        m_GimmickAct = transform.GetChild(1).gameObject;
        m_GimmickAct.SetActive(false);

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
        if (m_SCR_Goal.m_IsClearflg)
        {
            m_Velocity = Vector3.zero;
            transform.LookAt(resultCamera);
            cp_AMC.SetBool("Result", true);
            return;
        }
        else
        {
            // ��������
            if (isFellDown)
            {
                time += Time.deltaTime;

                // ��~���Ԃ��߂��Ă��Ȃ��Ȃ珈�����Ȃ�
                if (time < stopTime) return;

                isFellDown = false;
                time = 0f;
            }

            // �X�e�[�g�}�V��
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

            KickProc();
            CutProc();

            // beforeFrameNum���̃t���[���O�̍��W���擾
            if (++frameCount > beforeFrameNum)
            {
                frameCount = 0;
                beforePosition = transform.position;
            }
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
        // �W�����v
        if (jump)
        {
            cp_Rigidbody.velocity = Vector3.zero;
            m_JumpVelocity = m_JumpPower;
            stateNow = STATE.AIR;

            cp_AMC.SetBool("Jump", true);
            SCR_SoundManager.instance.StopLoopSE();
            SCR_SoundManager.instance.PlaySE(SE_Type.Player_Jump);
            SCR_EffectManager.instance.EFF_DirSmoke(transform.position, transform.rotation);
            return;     // �W�����v������X�e�[�g�}�V���ύX�̂��ߏ����I��
        }

        // �W�����v�����ɗ���������
        if (cp_Rigidbody.velocity.y < -0.98f)
        {
            stateNow = STATE.AIR;
            leaveGroundPosition = beforePosition;
            return;     // ����������X�e�[�g�}�V���ύX�̂��ߏ����I��
        }

        PlayerMove(m_Speed, speedKeyborad);

    }

    void JumpProc()
    {
        //RayCast���n�A�j���[�V��������
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayUnderLength) && cp_Rigidbody.velocity.y < 0)
        {
            if (hit.transform.gameObject.CompareTag("Ground"))
            {
                cp_AMC.SetBool("Jump", false);
                SCR_EffectManager.instance.EFF_UniformSmoke(transform.position, transform.rotation);
            }
        }

        if (m_isKnockBack) cp_Rigidbody.velocity += new Vector3(0.0f, Physics.gravity.y * Time.deltaTime, 0.0f);
        else { m_JumpVelocity += Physics.gravity.y * Time.deltaTime * 2 ; }

        PlayerMove(m_Speed * speedJump, speedKeyborad * speedJump);
    }

    private void KickProc()
    {
        if (m_isKnockBack) return;
        if (isCreateNewMeshObj) return;

        if (m_CanKick)
        {
            Debug.Log("CanKick");

            bool input = false;
            // �M�~�b�N�쓮
            // �L�[��������Ă��Ȃ���Ώ������Ȃ�
            if (Gamepad.current != null)
            {
                if (Input.GetKeyDown(KeyCode.E) || Gamepad.current.leftTrigger.isPressed) input = true;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.E)) input = true;
            }

            if (input)
            {
                Debug.Log("Kick");
                KickAction();
                m_GimmickAct.SetActive(true);
                m_CanKick = false;
            }
        }
        else if (m_GimmickAct.activeSelf && !m_CanKick)
        {
            m_KickTimer += Time.deltaTime;
            if (m_KickTime < m_KickTimer)
            {
                m_KickTimer = 0.0f;
                m_GimmickAct.SetActive(false);
            }
        }
        else
        {
            m_KickTimer += Time.deltaTime;
            if (m_LapseKickTime < m_KickTimer)
            {
                m_KickTimer = 0.0f;
                cp_AMC.SetBool("Special", false);
                m_CanKick = true;
            }
        }        
    }

    private void CutProc()
    {
        if (m_isKnockBack) return;
        if (isCreateNewMeshObj) return;

        if (Gamepad.current == null)
        {
            // R�L�[����������e�̌`����I�u�W�F�N�g�𐶐�
            if (!Input.GetKeyDown(KeyCode.R)) return;
        }
        else
        {
            // R�L�[���p�b�h�̉E�{�^������������e�̌`����I�u�W�F�N�g�𐶐�
            if (!Gamepad.current.rightTrigger.isPressed &&
                !Input.GetKeyDown(KeyCode.R)) return;
        }

        if (scr_PCS.SetStartProc())
        {
            cp_AMC.SetBool("Cut", true);
            cp_AMC.SetBool("Special", false);
            cp_AMC.SetBool("Walk", false);
            cp_AMC.SetBool("Jump", false);
            SCR_SoundManager.instance.PlaySE(SE_Type.Player_Cut);
            m_Velocity = Vector3.zero;
        }   
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
        // �G�ɐG�ꂽ��m�b�N�o�b�N
        if (collision.gameObject.CompareTag("Enemy"))
        {
            KnockBack(collision.gameObject.transform);
        }

        // ���������猳�����ꏊ�ɖ߂�
        if (collision.gameObject.CompareTag("PlayerDeleteFloor"))
        {
            transform.position = leaveGroundPosition;
            isFellDown = true;
        }


        if (collision.gameObject.CompareTag("Ground") && m_JumpVelocity < 0.0f || 
            collision.gameObject.CompareTag("LightingObject") && m_JumpVelocity < 0.0f)
        {
            m_JumpVelocity = 0.0f;
            cp_Rigidbody.velocity = Vector3.zero;
            SCR_SoundManager.instance.PlaySE(SE_Type.Player_Landing);

            if (m_isKnockBack) { stateNow = STATE.LAPSE; }
            else { stateNow = STATE.GROUND; }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���������猳�����ꏊ�ɖ߂�
        if (other.CompareTag("PlayerDeleteFloor"))
        {
            transform.position = leaveGroundPosition;
            isFellDown = true;
        }
    }

    // �v���C���[�̈ړ�
    private void PlayerMove(float controllerSpeed, float keyboradSpeed)
    {
        if (isCreateNewMeshObj) return;
        if (m_isKnockBack) return;
        // ������
        m_Velocity = Vector3.zero;

        // �R���g���[���[
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

            // �O����������
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

        // �L�[�{�[�h
        // W�L�[�i�O���ړ��j
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 temp = new Vector3(0.0f, 0.0f, 0.1f * keyboradSpeed);
            transform.position += temp;
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        // S�L�[�i����ړ��j
        if (Input.GetKey(KeyCode.S))
        {
            //transform.position -= m_Speed * transform.forward * Time.deltaTime;
            Vector3 temp = new Vector3(0.0f, 0.0f, -0.1f * keyboradSpeed);
            transform.position += temp;
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
        // D�L�[�i�E�ړ��j
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 temp = new Vector3(0.1f * keyboradSpeed, 0.0f, 0.0f);
            transform.position += temp;
            transform.eulerAngles = new Vector3(0f, 90.0f, 0f);
        }
        // A�L�[�i���ړ��j
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 temp = new Vector3(-0.1f * keyboradSpeed, 0.0f, 0.0f);
            transform.position += temp;
            transform.eulerAngles = new Vector3(0f, -90.0f, 0f);
        }

        if (stateNow == STATE.GROUND && m_Velocity != Vector3.zero) {
            if (!cp_AMC.GetBool("Walk"))
            {
                cp_AMC.SetBool("Walk", true);
                SCR_SoundManager.instance.PlaySE(SE_Type.Player_Walk, true, 0.2f);
            }            
        }
        else {
            cp_AMC.SetBool("Walk", false);
            SCR_SoundManager.instance.StopLoopSE();
        }

        m_Velocity.y = m_JumpVelocity;
        m_Velocity.y += Physics.gravity.y * Time.deltaTime * m_FallMultiplier;

        cp_Rigidbody.velocity = m_Velocity;
    }
}
