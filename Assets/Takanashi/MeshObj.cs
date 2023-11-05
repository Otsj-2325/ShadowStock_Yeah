using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class MeshObj : MonoBehaviour
{
    public Material material { private get; set; }

    public GameObject[] wallObjects { private get; set; }

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

    private Vector3 beforePosition = Vector3.zero;
    private int frameCount = 0;
    private int beforeFrameNum = 10;
    private Vector3 originalPosition = Vector3.zero;

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
        if (material != null)
        {
            meshRenderer.material = material;
        }

        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);        

        rigidBody = gameObject.AddComponent<Rigidbody>();
        rigidBody.useGravity = false;

        MeshCollider meshColliderTrigger = gameObject.AddComponent<MeshCollider>();
        meshColliderTrigger.convex = true;
        meshColliderTrigger.isTrigger = true;

        originalPosition = transform.position;
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

                    rigidBody.velocity = velocity;
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

                // beforeFrameNum数のフレーム前の座標を取得
                if (++frameCount > beforeFrameNum)
                {
                    frameCount = 0;
                    beforePosition = transform.position;
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
        // 準備中
        if (nowState == STATE.CREATE_PREPARE)
        {
            // 壁に当たったら元の座標に戻す
            foreach(GameObject obj in wallObjects)
            {
                if (other.gameObject == obj)
                {
                    Debug.Log("hit");
                    transform.position = originalPosition;
                    rigidBody.velocity = Vector3.zero;
                    return;
                }
            }

            inTrigger = true;
        }

        // 削除する床に当たったら自分を消す
        if (nowState == STATE.CREATED && other.gameObject == deleteFloorObj) Destroy(this.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        inTrigger = false;
    }
}
