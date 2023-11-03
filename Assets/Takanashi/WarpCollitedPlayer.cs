using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WarpCollitedPlayer : MonoBehaviour
{
    private Action warp;
    private Action exitWarp;

    // WarpManager����Q��
    public void SetWarp(Action action)
    {
        warp = action;
    }

    // WarpManager����Q��
    public void SetExitWarp(Action action)
    {
        exitWarp = action;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning("Anything enter");
        
        // �v���C���[�Ɠ������Ă��Ȃ���Ώ������Ȃ�
        if (!other.gameObject.CompareTag("Player")) return;

        // �A�N�V�������Z�b�g���ꂢ�Ȃ���Ώ������Ȃ�
        if (warp == null) return;

        warp?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        // �v���C���[�łȂ���Ώ������Ȃ�
        if (!other.gameObject.CompareTag("Player")) return;

        // �A�N�V�������Z�b�g���ꂢ�Ȃ���Ώ������Ȃ�
        if (exitWarp == null) return;

        exitWarp?.Invoke();
    }
}
