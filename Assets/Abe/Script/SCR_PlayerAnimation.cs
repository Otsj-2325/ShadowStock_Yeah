using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PlayerAnimation : MonoBehaviour
{
    [Header("AMC_Player")]
    [SerializeField]
    private Animator m_animator;

    void Start()
    {
        if (!m_animator) { Debug.Log("Not set AnimationController"); }
    }


    public void PlayWalkAnim()
    {
        m_animator.SetBool("Walk", true);
        m_animator.SetBool("Landing", false);
    }

    public void PlayeJumpAnim()
    {
        m_animator.SetBool("Jump", true);
        m_animator.SetBool("Walk", false);
    }

    public void PlayeLandingAnim()
    {
        m_animator.SetBool("Jump", false);
        m_animator.SetBool("Landing", true);
    }

    public void PlayerCutAnim()
    {
        m_animator.SetBool("Cut", false);
    }
}
