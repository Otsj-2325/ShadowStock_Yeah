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
        // �ǂ̃I�u�W�F�N�g���L�^
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
