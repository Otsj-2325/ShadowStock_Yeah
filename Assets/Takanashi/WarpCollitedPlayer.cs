using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WarpCollitedPlayer : MonoBehaviour
{
    private Action warp;
    private Action exitWarp;

    // WarpManagerから参照
    public void SetWarp(Action action)
    {
        warp = action;
    }

    // WarpManagerから参照
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
        
        // プレイヤーと当たっていなければ処理しない
        if (!other.gameObject.CompareTag("Player")) return;

        // アクションがセットされいなければ処理しない
        if (warp == null) return;

        warp?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        // プレイヤーでなければ処理しない
        if (!other.gameObject.CompareTag("Player")) return;

        // アクションがセットされいなければ処理しない
        if (exitWarp == null) return;

        exitWarp?.Invoke();
    }
}
