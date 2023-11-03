using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpManager : MonoBehaviour
{
    [Header("※順番は必ず下から上へ座標が行くように")]

    [Header("プレイヤーオブジェクト")]
    [SerializeField] private PlayerController playerObj;

    [Header("ワープオブジェクト(必ず偶数個になるように)")]
    [SerializeField] private GameObject[] warpObj;

    [Header("フレームのオブジェクト")]
    [SerializeField] private PlayerFrameMove frameObj;

    [Header("フレーム出現座標")]
    [SerializeField] private GameObject[] frameObjPos;
    
    [SerializeField] private WallManager _wallManager;

    [Header("プレイヤーが消える床のオブジェクト")]
    [SerializeField] private GameObject deleteFloorObj;

    [Header("床からプレイヤーが消える床への距離")]
    [SerializeField] private float deleteFloorSpace;

    //==================================================
    // private
    // プレイヤー移動
    private List<Vector3> warpPosition = new List<Vector3>();
    private int nowIndex;
    private int exitIndex;
    private bool warped;

    // フレームオブジェクト
    private List<Vector3> framePosition = new List<Vector3>();
    private PlayerFrameMove frameObjNow;

    private List<Vector3> lightPosition = new List<Vector3>();  // ライト

    private SCR_VCamManager scr_VM;     // カメラ

    // Start is called before the first frame update
    void Start()
    {
        // ワープオブジェクトの個数が偶数でなければ処理しない
        if (warpObj.Length % 2 != 0) return;

        // ワープオブジェクト
        for (int i = 0; i < warpObj.Length; i++)
        {
            warpPosition.Add(warpObj[i].transform.position);

            // アクションセット
            WarpCollitedPlayer temp = warpObj[i].GetComponent<WarpCollitedPlayer>();
            if (temp == null) continue;

            temp.SetWarp(Warp);
            temp.SetExitWarp(ExitWarp);
        }

        // フレームオブジェクト
        if (warpObj.Length / 2 + 1 != frameObjPos.Length) return;
        for(int i = 0; i < frameObjPos.Length; i++)
        {
            framePosition.Add(frameObjPos[i].transform.position);
        }
        frameObjNow = Instantiate(frameObj);
        frameObjNow.wallManager = _wallManager;
        frameObjNow.transform.position = framePosition[0];

        nowIndex = 0;
        exitIndex = -1;
        warped = false;

        scr_VM = FindObjectOfType<SCR_VCamManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // WarpCollitedPlayerで呼び出し予定
    private void Warp()
    {
        // ワープオブジェクトの個数が偶数でなければ処理しない
        if (warpObj.Length % 2 != 0 || playerObj == null) return;

        for (int i = 0; i < warpPosition.Count; i++)
        {
            Vector3 playerPos = playerObj.transform.position;
            if (warpPosition[i].y + 2.0f < playerPos.y || warpPosition[i].y - 2.0f > playerPos.y) continue;

            if (warpPosition[i].x + 2.0f < playerPos.x || warpPosition[i].x - 2.0f > playerPos.x) continue;

            if (i % 2 == 0)
            {
                if (exitIndex == i + 1) return;     // 一度ワープ地点から離れてワープするようにする

                Vector3 frontOfWarpPos = warpPosition[i + 1];
                frontOfWarpPos.z = frontOfWarpPos.z - 3.0f;

                // プレイヤー移動
                playerObj.transform.position = frontOfWarpPos;
                nowIndex = i + 1;
                exitIndex = i;

                // フレームオブジェクト移動
                Destroy(frameObjNow);
                frameObjNow = Instantiate(frameObj);
                frameObjNow.wallManager = _wallManager;
                frameObjNow.transform.position = framePosition[i / 2 + 1];

                // カメラ移動
                scr_VM.SwitchVCam(i / 2 + 1 + 1);

                // プレイヤーが消える床移動
                Vector3 tempPos = new Vector3(deleteFloorObj.transform.position.x, warpPosition[i + 1].y, 
                                              deleteFloorObj.transform.position.z);
                tempPos.y -= deleteFloorSpace;
                deleteFloorObj.transform.position = tempPos;
            }
            else
            {
                if (exitIndex == i - 1) return;     // 一度ワープ地点から離れてワープするようにする
                
                // プレイヤー移動
                playerObj.transform.position = warpPosition[i - 1];
                nowIndex = i - 1;
                exitIndex = i;

                // フレームオブジェクト移動
                Destroy(frameObjNow);
                frameObjNow = Instantiate(frameObj);
                frameObjNow.wallManager = _wallManager;
                frameObjNow.transform.position = framePosition[i / 2];

                // カメラ移動
                scr_VM.SwitchVCam(i / 2 + 1);

                // プレイヤーが消える床移動
                Vector3 tempPos = deleteFloorObj.transform.position;
                tempPos.y += warpPosition[i - 1].y - deleteFloorSpace;
                deleteFloorObj.transform.position = tempPos;
            }
            playerObj.SetVectorZero();
            warped = true;

            return;
        }
    }

    // WarpCollitedPlayerで呼び出し予定
    private void ExitWarp()
    {
        // ワープオブジェクトの個数が偶数でなければ処理しない
        if (warpObj.Length % 2 != 0 || playerObj == null) return;

        // 一回目の呼び出しは無視(ワープした後に元ワープポイントのExitを無視するため)
        if (warped)
        {
            warped = false;
            return;
        }

        // プレイヤーの座標を移動
        for (int i = 0; i < warpPosition.Count; i++)
        {
            Vector3 playerPos = playerObj.transform.position;
            if (warpPosition[i].y + 2.0f < playerPos.y || warpPosition[i].y - 2.0f > playerPos.y) continue;

            if (warpPosition[i].x + 2.0f < playerPos.x || warpPosition[i].x - 2.0f > playerPos.x) continue;

            exitIndex = i;
        }
    }
}
