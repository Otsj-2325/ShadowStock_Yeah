using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpManager : MonoBehaviour
{
    [Header("���[�v�I�u�W�F�N�g(�K�������ɂȂ�悤��)")]
    [SerializeField] private GameObject[] warpObj;

    [Header("�t���[���o�����W")]
    [SerializeField] private GameObject[] frameObj;

    private List<Vector3> warpPosition = new List<Vector3>();
    private GameObject playerObj;
    private int nowIndex;
    private int exitIndex;
    private bool warped;

    private List<Vector3> framePosition = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
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
        for(int i = 0; i < frameObj.Length; i++)
        {
            framePosition.Add(frameObj[i].transform.position);
        }

        playerObj = GameObject.FindGameObjectWithTag("Player");
        nowIndex = 0;
        exitIndex = -1;
        warped = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // WarpCollitedPlayer�ŌĂяo���\��
    private void Warp()
    {
        // �v���C���[�̍��W���ړ�
        for (int i = 0; i < warpPosition.Count; i++)
        {
            Vector3 playerPos = playerObj.transform.position;
            if (warpPosition[i].y + 2.0f < playerPos.y || warpPosition[i].y - 2.0f > playerPos.y) continue;

            if (warpPosition[i].x + 2.0f < playerPos.x || warpPosition[i].x - 2.0f > playerPos.x) continue;

            if (i % 2 == 0)
            {
                if (exitIndex == i + 1) return;     // ��x���[�v�n�_���痣��ă��[�v����悤�ɂ���
                
                playerObj.transform.position = warpPosition[i + 1];
                nowIndex = i + 1;
                exitIndex = i;


            }
            else
            {
                if (exitIndex == i - 1) return;     // ��x���[�v�n�_���痣��ă��[�v����悤�ɂ���
                
                playerObj.transform.position = warpPosition[i - 1];
                nowIndex = i - 1;
                exitIndex = i;
            }
            warped = true;

            return;
        }
    }

    // WarpCollitedPlayer�ŌĂяo���\��
    private void ExitWarp()
    {
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
