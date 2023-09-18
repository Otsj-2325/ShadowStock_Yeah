using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFrameMove : MonoBehaviour
{
    [Header("�������X�s�[�h")]
    [SerializeField] private float moveSpeed;

    private Vector3 beforePosition;     // �O�t���[�����W
    private Vector2 wallRightUpPos = Vector2.zero;         // �ǂ̉E��[
    private Vector2 wallLeftDownPos = Vector2.zero;         // �ǂ̉E��[
    private List<Transform> frameObj = new List<Transform>();   // �q�I�u�W�F�N�g�̘g

    // Start is called before the first frame update
    void Start()
    {
        beforePosition = gameObject.transform.position;

        // ������ǂ�T�m
        float posZ = gameObject.transform.position.z;
        GameObject[] gameObjAll = GameObject.FindGameObjectsWithTag("Wall");
        foreach (GameObject temp in gameObjAll)
        {
            float tempZ = temp.gameObject.transform.position.z;

            // �ǂ�z���W�Ǝ�����z���W������Ă���Ώ������Ȃ�
            if (posZ - 1.0f > tempZ || posZ + 1.0f < tempZ) return;

            Vector3 tempScale = temp.transform.localScale / 2;

            // �[�_���擾
            wallRightUpPos.x = temp.transform.position.x + tempScale.x;
            wallRightUpPos.y = temp.transform.position.y + tempScale.y;
            wallLeftDownPos.x = temp.transform.position.x - tempScale.x;
            wallLeftDownPos.y = temp.transform.position.y - tempScale.y;
        }

        // �q�I�u�W�F�N�g���擾
        frameObj.Add(transform.GetChild(0));    // left
        frameObj.Add(transform.GetChild(1));    // right
        frameObj.Add(transform.GetChild(2));    // up
        frameObj.Add(transform.GetChild(3));    // down
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        // �Q�[���p�b�h���ڑ�����Ă��Ȃ���null�ɂȂ�
        if (Gamepad.current == null) return;

        beforePosition = gameObject.transform.position;

        Vector2 move = Vector3.zero;
        Vector2 rightStick = Gamepad.current.rightStick.ReadValue();

        // �E�X�e�B�b�N��
        if (rightStick.x > 0.1f || rightStick.x < -0.1f)
        {
            move.x += rightStick.x;
        }
        // �E�X�e�B�b�N�c
        if (rightStick.y > 0.1f || rightStick.y < -0.1f)
        {
            move.y += rightStick.y;
        }

        move = move.normalized;
        move *= moveSpeed;

        Vector3 tempPosition = new Vector3(move.x, move.y, 0.0f);
        gameObject.transform.position += tempPosition;

        // �ǂ̒��ɘg�����邩
        for(int i = 0; i < frameObj.Count; i++)
        {
            switch (i)
            {
                // Left
                case 0:
                    // �g���ɂ���Ώ������Ȃ�
                    if (frameObj[i].position.x > wallLeftDownPos.x) break;

                    gameObject.transform.position = beforePosition;
                    break;

                // Right
                case 1:
                    // �g���ɂ���Ώ������Ȃ�
                    if (frameObj[i].position.x < wallRightUpPos.x) break;

                    gameObject.transform.position = beforePosition;
                    break;

                // Up
                case 2:
                    if (frameObj[i].position.y < wallRightUpPos.y) break;

                    gameObject.transform.position = beforePosition;
                    break;

                // Down
                case 3:
                    if (frameObj[i].position.y > wallLeftDownPos.y) break;

                    gameObject.transform.position = beforePosition;
                    break;

                default:
                    break;
            }
        }
    }
}
