using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Mesh new_mesh = new Mesh();
        List<int> right_index = new List<int>();
        List<int> left_index = new List<int>();
        Vector3[] point = new Vector3[37 * 2];
        point[0] = new Vector3(0.0f, 0.0f, 0.0f);

        point[1] = new Vector3(1.0f, 0.0f, 0.0f);

        for(int i = 0; i < 35; i++)
        {
            float rad = (i + 1) * 10 * Mathf.Deg2Rad;

            point[36 - i].x = point[1].x * Mathf.Cos(rad) - point[1].y * Mathf.Sin(rad);
            point[36 - i].y = -point[1].x * Mathf.Sin(rad) + point[1].y * Mathf.Cos(rad);

            //Debug.Log(point[i + 2].x);
            //Debug.Log(point[i + 2].y);
        }

        // 立体にする
        for (int i = 0; i < 37; i++)
        {
            point[i + 37] = new Vector3(point[i].x, point[i].y, -0.1f);
        }
        new_mesh.vertices = point;

        // 頂点インデックスを設定（立方体の場合）
        int[] triangles = new int[36 * 2 * 3 + 72 * 3];

        // 表面
        for(int i = 0; i < 35; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 2;
            triangles[i * 3 + 2] = i + 1;
        }
        triangles[35 * 3] = 0;
        triangles[35 * 3 + 1] = 1;
        triangles[35 * 3 + 2] = 36;

        // 裏面
        for (int i = 0; i < 35; i++)
        {
            triangles[(i + 36) * 3] = 37;
            triangles[(i + 36) * 3 + 1] = 37 + i + 1;
            triangles[(i + 36) * 3 + 2] = 37 + i + 2;
        }
        triangles[71 * 3] = 37;
        triangles[71 * 3 + 1] = 36 + 37;
        triangles[71 * 3 + 2] = 38;

        // 横面
        for(int i = 1; i < 36; i++)
        {
            int temp_num = 70 + i * 2;
            triangles[temp_num * 3] = i;
            triangles[temp_num * 3 + 1] = i + 1;
            triangles[temp_num * 3 + 2] = i + 38;

            triangles[(temp_num + 1) * 3] = i;
            triangles[(temp_num + 1) * 3 + 1] = i + 38;
            triangles[(temp_num + 1) * 3 + 2] = i + 37;
        }

        triangles[(70 + 36 * 2) * 3] = 36;
        triangles[(70 + 36 * 2) * 3 + 1] = 1;
        triangles[(70 + 36 * 2) * 3 + 2] = 38;

        triangles[(70 + 36 * 2 + 1) * 3] = 36;
        triangles[(70 + 36 * 2 + 1) * 3 + 1] = 38;
        triangles[(70 + 36 * 2 + 1) * 3 + 2] = 37 + 36;

        new_mesh.triangles = triangles;

        // 法線を計算して設定
        new_mesh.RecalculateNormals();

        GameObject newObject = new GameObject("NewMeshObject");

        //MeshFilterはshredMeshをMeshRendererに引き渡す。
        MeshFilter mfilter = newObject.AddComponent<MeshFilter>();
        mfilter.mesh = new_mesh;

        // MeshRendererを追加してマテリアルをアタッチ
        MeshRenderer meshRenderer = newObject.AddComponent<MeshRenderer>();

        newObject.transform.position = Vector3.zero; // 必要な位置に変更する

        newObject.transform.localScale = new Vector3(1.0f, 1.0f, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
