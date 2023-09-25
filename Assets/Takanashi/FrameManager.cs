using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameManager : MonoBehaviour
{
    [SerializeField] private WarpWithFrame warpWithFrame;

    private GameObject[] wallObj;

    // Start is called before the first frame update
    void Start()
    {
        // 壁のオブジェクトを記録
        wallObj = GameObject.FindGameObjectsWithTag("Wall");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }
}
