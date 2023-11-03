using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpManager : MonoBehaviour
{
    [Header("�����Ԃ͕K���������֍��W���s���悤��")]

    [Header("�v���C���[�I�u�W�F�N�g")]
    [SerializeField] private PlayerController playerObj;

    [Header("���[�v�I�u�W�F�N�g(�K�������ɂȂ�悤��)")]
    [SerializeField] private GameObject[] warpObj;

    [Header("�t���[���̃I�u�W�F�N�g")]
    [SerializeField] private PlayerFrameMove frameObj;

    [Header("�t���[���o�����W")]
    [SerializeField] private GameObject[] frameObjPos;
    
    [SerializeField] private WallManager _wallManager;

    [Header("�v���C���[�������鏰�̃I�u�W�F�N�g")]
    [SerializeField] private GameObject deleteFloorObj;

    [Header("������v���C���[�������鏰�ւ̋���")]
    [SerializeField] private float deleteFloorSpace;

    //==================================================
    // private
    // �v���C���[�ړ�
    private List<Vector3> warpPosition = new List<Vector3>();
    private int nowIndex;
    private int exitIndex;
    private bool warped;

    // �t���[���I�u�W�F�N�g
    private List<Vector3> framePosition = new List<Vector3>();
    private PlayerFrameMove frameObjNow;

    private List<Vector3> lightPosition = new List<Vector3>();  // ���C�g

    private SCR_VCamManager scr_VM;     // �J����

    // Start is called before the first frame update
    void Start()
    {
        // ���[�v�I�u�W�F�N�g�̌��������łȂ���Ώ������Ȃ�
        if (warpObj.Length % 2 != 0) return;

        // ���[�v�I�u�W�F�N�g
        for (int i = 0; i < warpObj.Length; i++)
        {
            warpPosition.Add(warpObj[i].transform.position);

            // �A�N�V�����Z�b�g
            WarpCollitedPlayer temp = warpObj[i].GetComponent<WarpCollitedPlayer>();
            if (temp == null) continue;

            temp.SetWarp(Warp);
            temp.SetExitWarp(ExitWarp);
        }

        // �t���[���I�u�W�F�N�g
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

    // WarpCollitedPlayer�ŌĂяo���\��
    private void Warp()
    {
        // ���[�v�I�u�W�F�N�g�̌��������łȂ���Ώ������Ȃ�
        if (warpObj.Length % 2 != 0 || playerObj == null) return;

        for (int i = 0; i < warpPosition.Count; i++)
        {
            Vector3 playerPos = playerObj.transform.position;
            if (warpPosition[i].y + 2.0f < playerPos.y || warpPosition[i].y - 2.0f > playerPos.y) continue;

            if (warpPosition[i].x + 2.0f < playerPos.x || warpPosition[i].x - 2.0f > playerPos.x) continue;

            if (i % 2 == 0)
            {
                if (exitIndex == i + 1) return;     // ��x���[�v�n�_���痣��ă��[�v����悤�ɂ���

                Vector3 frontOfWarpPos = warpPosition[i + 1];
                frontOfWarpPos.z = frontOfWarpPos.z - 3.0f;

                // �v���C���[�ړ�
                playerObj.transform.position = frontOfWarpPos;
                nowIndex = i + 1;
                exitIndex = i;

                // �t���[���I�u�W�F�N�g�ړ�
                Destroy(frameObjNow);
                frameObjNow = Instantiate(frameObj);
                frameObjNow.wallManager = _wallManager;
                frameObjNow.transform.position = framePosition[i / 2 + 1];

                // �J�����ړ�
                scr_VM.SwitchVCam(i / 2 + 1 + 1);

                // �v���C���[�������鏰�ړ�
                Vector3 tempPos = new Vector3(deleteFloorObj.transform.position.x, warpPosition[i + 1].y, 
                                              deleteFloorObj.transform.position.z);
                tempPos.y -= deleteFloorSpace;
                deleteFloorObj.transform.position = tempPos;
            }
            else
            {
                if (exitIndex == i - 1) return;     // ��x���[�v�n�_���痣��ă��[�v����悤�ɂ���
                
                // �v���C���[�ړ�
                playerObj.transform.position = warpPosition[i - 1];
                nowIndex = i - 1;
                exitIndex = i;

                // �t���[���I�u�W�F�N�g�ړ�
                Destroy(frameObjNow);
                frameObjNow = Instantiate(frameObj);
                frameObjNow.wallManager = _wallManager;
                frameObjNow.transform.position = framePosition[i / 2];

                // �J�����ړ�
                scr_VM.SwitchVCam(i / 2 + 1);

                // �v���C���[�������鏰�ړ�
                Vector3 tempPos = deleteFloorObj.transform.position;
                tempPos.y += warpPosition[i - 1].y - deleteFloorSpace;
                deleteFloorObj.transform.position = tempPos;
            }
            playerObj.SetVectorZero();
            warped = true;

            return;
        }
    }

    // WarpCollitedPlayer�ŌĂяo���\��
    private void ExitWarp()
    {
        // ���[�v�I�u�W�F�N�g�̌��������łȂ���Ώ������Ȃ�
        if (warpObj.Length % 2 != 0 || playerObj == null) return;

        // ���ڂ̌Ăяo���͖���(���[�v������Ɍ����[�v�|�C���g��Exit�𖳎����邽��)
        if (warped)
        {
            warped = false;
            return;
        }

        // �v���C���[�̍��W���ړ�
        for (int i = 0; i < warpPosition.Count; i++)
        {
            Vector3 playerPos = playerObj.transform.position;
            if (warpPosition[i].y + 2.0f < playerPos.y || warpPosition[i].y - 2.0f > playerPos.y) continue;

            if (warpPosition[i].x + 2.0f < playerPos.x || warpPosition[i].x - 2.0f > playerPos.x) continue;

            exitIndex = i;
        }
    }
}
