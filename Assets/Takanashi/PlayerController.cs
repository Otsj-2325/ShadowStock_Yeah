using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    // �ړ��֘A�̃p�����[�^
    [Header("�R���g���[���[����̃X�s�[�h")]
    [SerializeField] private float m_Speed = 1.0f;

    [Header("�L�[�{�[�h����̃X�s�[�h")]
    [SerializeField] private float speedKeyborad = 2.0f;

    [Header("�W�����v���X�s�[�h(���i�̃X�s�[�h�̊���)")]
    [SerializeField] private float speedJump = 0.5f;

    [Header("����������̒�~����")]
    [SerializeField] private float stopTime = 1.5f;

    // �W�����v�֌W
    public float m_JumpPower;

    [SerializeField] private float m_JumpVelocity = 0.0f;

    [SerializeField] private bool m_IsJumping = false;

    public float knockBackPower;   // �m�b�N�o�b�N�������


    private Vector3 m_Velocity;

    private Rigidbody cp_Rigidbody;

    private float rayUnderLength;   // �n�ʒ���������̃��C�L���X�g�̒���

    // �n�ʂ��痣�ꂽ�ۂ̏����̕ϐ�
    private Vector3 leaveGroundPosition;    // �n�ʂ��痣�ꂽ���̍��W
    private Vector3 beforePosition;         // ���t���[���O�̍��W
    private int beforeFrameNum = 10;        // ��̃t���[�����̒���
    private int frameCount;

    private Action warp;

    private bool isFellDown;    // ����������
    private float time;         // ������̒�~���ԑ���

    private bool isCreateNewMeshObj = false;    // ���݉e����̃I�u�W�F�N�g�쐬����

    // �X�e�[�g�}�V��
    private enum STATE
    {
        GROUND,
        AIR
    }
    private STATE stateNow;     // ���݂̃X�e�[�g

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

            default:
                break;
        }

        ActionProc();
        CutProc();
        PasteProc();

        // beforeFrameNum���̃t���[���O�̍��W���擾
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
        // �W�����v
        if (jump)
        {
            cp_Rigidbody.velocity = Vector3.zero;
            cp_Rigidbody.AddForce(Vector3.up * m_JumpPower);
            stateNow = STATE.AIR;
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
        // ���C�L���X�g�ɂ��n�ʂɓ������������f
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
            Debug.Log("�A�N�V������");
        }

        if (Input.GetKey(KeyCode.J))
        {
            Debug.Log("�A�N�V������");
        }

    }

    void CutProc()
    {
        if (Gamepad.current.buttonNorth.isPressed)
        {
            Debug.Log("�J�b�g��");
        }

        if (Input.GetKey(KeyCode.K))
        {
            Debug.Log("�J�b�g��");
        }
    }

    void PasteProc()
    {
        if (Gamepad.current.buttonWest.isPressed)
        {
            Debug.Log("�y�[�X�g��");
        }

        if (Input.GetKey(KeyCode.P))
        {
            Debug.Log("�y�[�X�g��");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �G�ɐG�ꂽ��m�b�N�o�b�N
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("��������");
            cp_Rigidbody.velocity = Vector3.zero;

            // �����̈ʒu�ƐڐG���Ă����I�u�W�F�N�g�̈ʒu�Ƃ��v�Z���āA�����ƕ������o���Đ��K��(���x�x�N�g�����Z�o)
            Vector3 distination = (transform.position - collision.transform.position).normalized;

            cp_Rigidbody.AddForce(distination * knockBackPower, ForceMode.VelocityChange);
        }

        // ���������猳�����ꏊ�ɖ߂�
        if (collision.gameObject.CompareTag("PlayerDeleteFloor"))
        {
            transform.position = leaveGroundPosition;
            isFellDown = true;
        }
    }

    // �v���C���[�̈ړ�
    private void PlayerMove(float controllerSpeed, float keyboradSpeed)
    {
        if (isCreateNewMeshObj) return;

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
            if(m_Velocity != Vector3.zero)
            {
                Quaternion Rotation = Quaternion.LookRotation(m_Velocity.normalized, Vector3.up);
                if (m_Velocity.magnitude != 0)
                {
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

        cp_Rigidbody.AddForce(m_Velocity);
    }
}
