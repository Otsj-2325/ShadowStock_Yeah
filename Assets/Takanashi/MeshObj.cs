using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class MeshObj : MonoBehaviour
{

    public GameObject deleteFloorObj { private get; set; }

    public float speedController { private get; set; }
    public float speedKeyboard { private get; set; }
    
    private enum STATE
    {
        CREATE_PREPARE, // 生成場所決め中
        CREATE_CANT,    // 置けない場所に置こうとしたエラー表示中(振動中)
        CREATED         // 置いた
    }
    private STATE nowState;

    private bool inTrigger = false;

    private float shakeTimer = 0.0f;
    private float shakeTime = 1.0f;
    private float shakeMagnitude = 3.0f;

    private Rigidbody rigidBody = null;

    private Action actionCreatePlayer;
    private Action actionCreatePlayerShadow;

    public void SetActionCreate(Action action)
    {
        actionCreatePlayer = action;
    }

    public void SetActionCreatePlayerShadow(Action action)
    {
        actionCreatePlayerShadow = action;
    }

    private void CreateEndNewMeshObjFromShadow()
    {
        
    }

    private void Start()
    {
        nowState = STATE.CREATE_PREPARE;
        
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        rigidBody = gameObject.AddComponent<Rigidbody>();
        rigidBody.useGravity = false;

        MeshCollider meshColliderTrigger = gameObject.AddComponent<MeshCollider>();
        meshColliderTrigger.convex = true;
        meshColliderTrigger.isTrigger = true;        
    }

    private void FixedUpdate()
    {
        switch (nowState)
        {
            case STATE.CREATE_PREPARE:
                bool create = false;
                if (Input.GetKeyDown(KeyCode.R)) create = true;

                if (create)
                {
                    if (inTrigger)
                    {
                        shakeTimer = shakeTime;
                        nowState = STATE.CREATE_CANT;                                                
                        return;
                    }
                    MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
                    meshCollider.convex = true;
                    rigidBody.useGravity = true;
                    actionCreatePlayer?.Invoke();
                    actionCreatePlayerShadow?.Invoke();
                    nowState = STATE.CREATED;
                    return;
                }
                
                // コントローラー
                if (Gamepad.current != null)
                {
                    Vector3 velocity = Vector3.zero;

                    Vector2 leftStick = Gamepad.current.leftStick.ReadValue();
                    if (leftStick.x > 0.1f || leftStick.x < -0.1f)
                    {
                        velocity.x = speedController * leftStick.x;
                    }
                    if (leftStick.y > 0.1f || leftStick.y < -0.1f)
                    {
                        velocity.z = speedController * leftStick.y;
                    }

                    rigidBody.AddForce(velocity);
                }

                // キーボード
                // Wキー（前方移動）
                if (Input.GetKey(KeyCode.W))
                {
                    Vector3 temp = new Vector3(0.0f, 0.0f, 0.1f * speedKeyboard);
                    transform.position += temp;
                }
                // Sキー（後方移動）
                else if (Input.GetKey(KeyCode.S))
                {
                    //transform.position -= m_Speed * transform.forward * Time.deltaTime;
                    Vector3 temp = new Vector3(0.0f, 0.0f, -0.1f * speedKeyboard);
                    transform.position += temp;
                }
                // Dキー（右移動）
                if (Input.GetKey(KeyCode.D))
                {
                    Vector3 temp = new Vector3(0.1f * speedKeyboard, 0.0f, 0.0f);
                    transform.position += temp;
                }
                // Aキー（左移動）
                else if (Input.GetKey(KeyCode.A))
                {
                    Vector3 temp = new Vector3(-0.1f * speedKeyboard, 0.0f, 0.0f);
                    transform.position += temp;
                }
                break;
            case STATE.CREATE_CANT:
                shakeTimer -= Time.time;
                if(shakeTimer < 0.0f)
                {
                    nowState = STATE.CREATE_PREPARE;
                    return;
                }
                float x = transform.position.x + UnityEngine.Random.Range(-1.0f, 1.0f) * shakeMagnitude;
                float z = transform.position.z + UnityEngine.Random.Range(-1.0f, 1.0f) * shakeMagnitude;
                transform.position = new Vector3(x, transform.position.y, z);
                break;
            case STATE.CREATED:
                break;
            default:
                break;
        }    
    }

    private void OnTriggerEnter(Collider other)
    {
        // 置けない場所にあったら
        if (nowState == STATE.CREATE_PREPARE) inTrigger = true;

        // 削除する床に当たったら自分を消す
        if (nowState == STATE.CREATED && other.gameObject == deleteFloorObj) Destroy(this.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        inTrigger = false;
    }
}
