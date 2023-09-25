using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // �ړ��֘A�̃p�����[�^
    [Header("�����̍Œ�l")]
    [SerializeField]
    private float m_MinSpeed = 1.0f;
    [Header("�����̍ō��l")]
    [SerializeField]
    private float m_MaxSpeed = 3.0f;

    // �W�����v�֌W
    public float m_JumpPower;

    [SerializeField] private float m_JumpVelocity = 0.0f;

    [SerializeField] private bool m_IsJumping = false;

    public float knockBackPower;   // �m�b�N�o�b�N�������

    // �ړ����x
    private Vector3 m_Velocity;
    float m_Speed = 3.0f;

    private Rigidbody cp_Rigidbody;

    private float rayUnderLength;   // �n�ʒ���������̃��C�L���X�g�̒���

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

    // Start is called before the first frame update
    void Start()
    {
        cp_Rigidbody = GetComponent<Rigidbody>();

        /*==================AbeZone====================*/
        scr_GT = this.transform.Find("GroundTrigger").GetComponent<SCR_GroundTrigger>();
        /*=============================================*/

        stateNow = STATE.GROUND;

        rayUnderLength = transform.localScale.y / 2 + 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        // �Q�[���p�b�h���ڑ�����Ă��Ȃ���null�ɂȂ�B
        if (Gamepad.current == null) return;

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

        //MoveProc();
        //JumpProc();
        ActionProc();
        CutProc();
        PasteProc();

        //m_Velocity.y = m_JumpVelocity;
        //cp_Rigidbody.velocity = m_Velocity;
    }

    void MoveProc()
    {
        // �W�����v
        if (Input.GetKey(KeyCode.Space) || Gamepad.current.buttonSouth.isPressed)
        {
            cp_Rigidbody.velocity = Vector3.zero;
            cp_Rigidbody.AddForce(Vector3.up * m_JumpPower);
            Debug.Log("�W�����v");
            stateNow = STATE.AIR;
            return;     // �W�����v������X�e�[�g�}�V���ύX�̂��ߏ����I��
        }

        // ������
        m_Velocity = Vector3.zero;

        // �R���g���[���[
        Vector2 leftStick = Gamepad.current.leftStick.ReadValue();
        if (leftStick.x > 0.1f || leftStick.x < -0.1f)
        {
            m_Velocity.x = m_Speed * leftStick.x;
        }
        if (leftStick.y > 0.1f || leftStick.y < -0.1f)
        {
            m_Velocity.z = m_Speed * leftStick.y;
        }

        // �L�[�{�[�h
        // W�L�[�i�O���ړ��j
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += m_Speed * transform.forward * Time.deltaTime;
        }
        // S�L�[�i����ړ��j
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= m_Speed * transform.forward * Time.deltaTime;
        }
        // D�L�[�i�E�ړ��j
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += m_Speed * transform.right * Time.deltaTime;
        }
        // A�L�[�i���ړ��j
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= m_Speed * transform.right * Time.deltaTime;
        }


        Quaternion Rotation = Quaternion.LookRotation(m_Velocity.normalized, Vector3.up);
        if (m_Velocity.magnitude != 0)
        {
            this.transform.rotation = Rotation;
        }

        cp_Rigidbody.AddForce(m_Velocity);
    }

    void JumpProc()
    {
        // ���C�L���X�g�ɂ��n�ʂɓ������������f
        // ��
        RaycastHit hit;
        Vector3 temp = transform.position;
        temp.x -= transform.localScale.x / 2;
        if (Physics.Raycast(temp, Vector3.down, out hit, rayUnderLength))
        {
            stateNow = STATE.GROUND;
            //Debug.Log("land");
            return;
        }
        // �E
        temp = transform.position;
        temp.x += transform.localScale.x / 2;
        if (Physics.Raycast(temp, Vector3.down, out hit, rayUnderLength))
        {
            stateNow = STATE.GROUND;
            //Debug.Log("land");
            return;
        }
        // ��O
        temp = transform.position;
        temp.z -= transform.localScale.z / 2;
        if (Physics.Raycast(temp, Vector3.down, out hit, rayUnderLength))
        {
            stateNow = STATE.GROUND;
            //Debug.Log("land");
            return;
        }
        // ��
        temp = transform.position;
        temp.z += transform.localScale.z / 2;
        if (Physics.Raycast(temp, Vector3.down, out hit, rayUnderLength))
        {
            stateNow = STATE.GROUND;
            //Debug.Log("land");
            return;
        }
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
        //if (collision.gameObject.CompareTag("Ground"))
        //{
        //    m_IsJumping = false;
        //    m_JumpVelocity = 0.0f;
        //}

        // �G�ɐG�ꂽ��m�b�N�o�b�N
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("��������");
            cp_Rigidbody.velocity = Vector3.zero;

            // �����̈ʒu�ƐڐG���Ă����I�u�W�F�N�g�̈ʒu�Ƃ��v�Z���āA�����ƕ������o���Đ��K��(���x�x�N�g�����Z�o)
            Vector3 distination = (transform.position - collision.transform.position).normalized;

            cp_Rigidbody.AddForce(distination * knockBackPower, ForceMode.VelocityChange);

            return;
        }

    }
}
