using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PyonManager : MonoBehaviour, IFCGimmck
{
    public enum STATE {//PyonState
        Patrol = 0,
        Chase,
        Contact,
        KnockBack,
    }

    [Header("CurveJump�L��")]
    [SerializeField]
    public bool UseCurveJump = false;

    [Header("CurveJump����")]
    [SerializeField]
    public AnimationCurve m_JumpCurve;

    [Header("�W�����v����(Curve�p)")]
    [SerializeField]
    private float m_JumpMaxTime;

    [Header("�ړ����x")]
    [SerializeField]
    private float m_MoveSpeed;

    [Header("�W�����v��")]
    [SerializeField]
    private float m_JumpPower;

    [Header("�d�͔{��")]
    [SerializeField]
    private float m_FallMultiplier;

    [Header("�W�����v�N�[���^�C��")]
    [SerializeField]
    private float m_lapseTime;

    [Header("�m�b�N�o�b�N�������̗�")]
    [SerializeField]
    private float m_kbSidePower;
    [Header("�m�b�N�o�b�N�c�����̗�")]
    [SerializeField]
    private float m_kbUpperPower;

    private STATE m_State;
    private GameObject m_Target;
    private Animator m_Anim = null;
    private Rigidbody cp_Rb = null;
    private Vector3 m_Velocity;

    private bool m_IsFind = false;
    private bool m_IsKnockBack = false;
    private bool m_Jump = false;
    private float m_lapseTimer = 0.0f;
    private float m_JumpTimer = 0.0f;//Curve�p

    void Start()
    {
        if (transform.GetChild(1).gameObject)
        {
            m_Anim = transform.GetChild(1).gameObject.GetComponent<Animator>();
        }
        else
        {
            Debug.Log("Not Find : Model");
        }

        m_State = STATE.Patrol;
        cp_Rb = gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        switch (m_State) {
            case STATE.Patrol:
                PatrolProc();
                break;
            case STATE.Chase:
                ChaseProc();
                break;
            case STATE.Contact:
                ContactProc();
                break;
            case STATE.KnockBack:
                KnockBackProc();
                break;
            default:
                Debug.Log("Out of Range : Pyon State");
                break;
        }
    }

    private void PatrolProc()
    {
        if (m_IsFind) {//�������X�e�[�g�ȍ~
            m_State = STATE.Chase;
            return;
        }

        if (m_Jump)
        {
            if (UseCurveJump) { CurveJumpProc(); }
            else { VeloFallProc(); }
        }
        else
        {
            m_lapseTimer += Time.deltaTime;//�N�[���^�C���v��

            if(m_lapseTimer > m_lapseTime / 2)//�W�����v�Ăѓ��앪�����Ă�
            {
                m_Anim.SetBool("Jump", true);
                m_Anim.SetBool("Landing", false);
            }

            if (m_lapseTimer > m_lapseTime)
            {
                transform.rotation = transform.rotation * Quaternion.Euler(0.0f, 180.0f, 0.0f); //�U�����

                m_Jump = true;
                m_lapseTimer = 0.0f;
                m_JumpTimer = 0.0f;

                SCR_SoundManager.instance.PlaySE(SE_Type.PYON_Jump);//SE�Đ�

                //�ʏ�W�����v�̏ꍇ���̃^�C�~���O�Œ���
                if (!UseCurveJump) { cp_Rb.velocity = (transform.forward * m_MoveSpeed) + new Vector3(0.0f, m_JumpPower, 0.0f); }
            }
        }
    }

    private void ChaseProc()
    {
        if (m_Jump)
        {
            if (UseCurveJump) { CurveJumpProc(); }
            else { VeloFallProc(); }
        }
        else
        {
            m_lapseTimer += Time.deltaTime;//�N�[���^�C���v��

            if (m_lapseTimer > m_lapseTime / 2)//�W�����v�Ăѓ��앪�����Ă�
            {
                m_Anim.SetBool("Jump", true);
                m_Anim.SetBool("Landing", false);
            }

            if (m_lapseTimer > m_lapseTime)
            {
                var v = m_Target.transform.position - transform.position;//�v���C���ւ̃x�N�g��
                v.y = 0.0f;//Y�����͕s�v�̂���0
                transform.rotation = Quaternion.LookRotation(v.normalized, Vector3.up);//�v���C��������

                m_Jump = true;
                m_lapseTimer = 0.0f;
                m_JumpTimer = 0.0f;

                SCR_SoundManager.instance.PlaySE(SE_Type.PYON_Jump);//SE�Đ�

                //�ʏ�W�����v�̏ꍇ���̃^�C�~���O�Œ���
                if (!UseCurveJump) { cp_Rb.velocity = (transform.forward * m_MoveSpeed) + new Vector3(0.0f, m_JumpPower, 0.0f); }
            }
        }
    }

    private void ContactProc()
    {
        if (m_Jump)
        {
            VeloFallProc();//���n�܂ő҂�
            return;
        }

        m_lapseTimer += Time.deltaTime;//�ҋ@�v��
        if(m_lapseTimer > 1.0f)//��b�o��
        {
            m_IsFind = false;
            m_State = STATE.Patrol;
        }
    }

    private void KnockBackProc()
    {
        if(m_IsKnockBack) { VeloFallProc(); }
        else
        {
            if (m_lapseTimer == 0.0f) { SCR_EffectManager.instance.EFF_KumoAuraYellow(transform.position, transform.rotation); }

            m_lapseTimer += Time.deltaTime;//�ҋ@�v��
         
            if (m_lapseTimer > 1.0f)//��b�o��
            {
                m_IsFind = false;
                m_IsKnockBack = false;
                m_Anim.SetBool("KnockBack", false);
                m_State = STATE.Patrol;
            }
        }
    }

    //�J�[�u�W�����v����
    private void CurveJumpProc()
    {
        m_Velocity = Vector3.zero;
        m_Velocity += (transform.forward * m_MoveSpeed);
        
        m_JumpTimer += Time.deltaTime;

        float t = m_JumpTimer / m_JumpMaxTime;
        if (t > 1.0f) {
            return;
        }
        
        m_Velocity.y += (m_JumpCurve.Evaluate((t))) * m_JumpPower;
        cp_Rb.AddForce(m_Velocity);
        
    }

    //�ʏ�W�����v��������
    private void VeloFallProc()
    {
        cp_Rb.velocity += Physics.gravity * Time.deltaTime * m_FallMultiplier;
    }

    private void KnockBack()//�m�b�N�o�b�N
    {
        var v = m_Target.transform.position - transform.position;
        v.y = 0.0f;
        transform.rotation = Quaternion.LookRotation(v.normalized, Vector3.up);//�v���C���̕���������

        cp_Rb.velocity = (-transform.forward * m_kbSidePower) + new Vector3(0.0f, m_kbUpperPower, 0.0f);//�������Ƀm�b�N�o�b�N
        m_IsKnockBack = true;

        m_Anim.SetBool("Jump", false);
        m_Anim.SetBool("Landing", false);
        m_Anim.SetBool("KnockBack", true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            m_Jump = false;
            m_IsKnockBack = false;
            cp_Rb.velocity = Vector3.zero;

            m_Anim.SetBool("Jump", false);
            m_Anim.SetBool("KnockBack", false);
            m_Anim.SetBool("Landing", true);

            SCR_SoundManager.instance.PlaySE(SE_Type.PYON_Landing);//SE�Đ�
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            m_State = STATE.Contact;
            m_lapseTimer = 0.0f;
            if (UseCurveJump) { m_JumpTimer = 0; }

            Debug.Log("contact Player");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!m_IsFind)
            {
                SCR_EffectManager.instance.EFF_KumoAuraRed(transform.position, transform.rotation);
                m_Target = other.gameObject;
            }
            m_IsFind = true;
            Debug.Log("Find");
        }
    }

    public void GimmckAct()
    {
        m_State = STATE.KnockBack;
        m_lapseTimer = 0.0f;

        if (UseCurveJump) { m_JumpTimer = 0; }

        KnockBack();
    }
}
