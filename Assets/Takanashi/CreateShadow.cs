using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]

public class CreateShadow : MonoBehaviour
{
    [Header("キューブ")]
    [SerializeField] private GameObject cube;

    private MeshFilter mesh_filter;
    private Mesh mesh;

    private bool create = true;

    // Start is called before the first frame update
    void Start()
    {
        mesh_filter = cube.GetComponent<MeshFilter>();
        mesh = mesh_filter.mesh;
    }

    // Update is called once per frame
    void Update()
    {
        // Eキーを押したら影の形からオブジェクトを生成
        if (!Input.GetKeyDown(KeyCode.E))
        {
            return;
        }

        Vector3[] vertices = mesh.vertices;
        List<Vector3> vertices3 = new List<Vector3>();
        Vector3[] vertices2 = mesh.vertices;        // キューブの場所と各頂点の座標を加算したもの
        List<float> point_y = new List<float>();    // Y座標を取得

        // 頂点のY座標を取得
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices3.Add(vertices[i]);

            vertices2[i] = cube.gameObject.transform.position + vertices[i];
            if (!point_y.Contains(vertices2[i].y))
            {
                point_y.Add(vertices2[i].y);
            }
        }

        // いらない座標を削除(裏面等)
        for (int i = 0; i < point_y.Count; i++)
        {
            List<int> right_max_index = new List<int>();  // 最も右側のベクトルのインデックス番号
            List<int> left_max_index = new List<int>();   // 最も左側のベクトルのインデックス番号

            float[] angle = new float[vertices2.Length];
            float right_angle = -1.0f;
            float left_angle = -1.0f;
            for (int j = 0; j < vertices2.Length; j++)
            {
                angle[j] = -1.0f;

                // 最も右側か左側の座標を求める(Y座標は同じ)
                if (vertices2[j].y == point_y[i])
                {
                    Vector3 temp_straight = cube.gameObject.transform.position - gameObject.transform.position;
                    Vector3 temp_this = vertices2[j] - gameObject.transform.position;
                    float temp_inner = Vector3.Dot(temp_straight, temp_this);
                    float angleRad = Mathf.Acos(temp_inner / (temp_straight.magnitude * temp_this.magnitude));
                    float angleDeg = angleRad * Mathf.Rad2Deg;
                    angle[j] = angleDeg;

                    temp_straight.y = 0.0f;
                    temp_this.y = 0.0f;
                    Vector3 temp_outer = Vector3.Cross(temp_straight, temp_this);

                    // 点が右側にある
                    if (temp_outer.y > 0)
                    {
                        if (right_max_index.Count == 0)
                        {
                            right_angle = angle[j];
                            right_max_index.Add(j);
                        }

                        if (right_angle < angle[j])
                        {
                            right_angle = angle[j];
                            right_max_index.Clear();
                            right_max_index.Add(j);
                        }
                        else if (right_angle == angle[j])
                        {
                            right_max_index.Add(j);
                        }
                    }
                    // 点が左側にある
                    else
                    {
                        if (left_max_index.Count == 0)
                        {
                            left_angle = angle[j];
                            left_max_index.Add(j);
                        }

                        if (left_angle < angle[j])
                        {
                            left_angle = angle[j];
                            left_max_index.Clear();
                            left_max_index.Add(j);
                        }
                        else if (left_angle == angle[j])
                        {
                            left_max_index.Add(j);
                        }
                    }
                }
            }

            // 同じY座標で最も右か左でなければ配列から削除する(その頂点は処理しない)
            for (int j = 0; j < vertices2.Length; j++)
            {
                if (vertices2[j].y == point_y[i])
                {
                    if (!right_max_index.Contains(j) && !left_max_index.Contains(j))
                    {
                        vertices3.Remove(vertices[j]);
                    }
                }
            }
        }

        Mesh new_mesh = new Mesh();
        var vertex = new List<Vector3>();

        // レイを生成し壁に当て、1影の座標を取得
        for (int i = 0; i < vertices3.Count; i++)
        {
            // レイの生成
            Vector3 dir = cube.gameObject.transform.position - gameObject.transform.position;
            dir += vertices3[i];
            Ray ray = new Ray(gameObject.transform.position, dir);
            Debug.DrawRay(ray.origin, ray.direction * 30, Color.red, 0.1f); // 長さ３０、赤色で５秒間可視化

            RaycastHit[] hits = Physics.RaycastAll(ray);
            foreach (RaycastHit hit in hits)
            {
                // 衝突先が壁タグであれば座標を取得
                if (hit.collider.CompareTag("Wall"))
                {
                    vertex.Add(new Vector3(hit.point.x, hit.point.y, hit.point.z));
                }
            }
        }

        Vector3 center_point = new Vector3();    // 影の中央座標
        Vector3 dir_center = cube.gameObject.transform.position - gameObject.transform.position;
        Ray ray_center = new Ray(gameObject.transform.position, dir_center);
        RaycastHit[] hits_center = Physics.RaycastAll(ray_center);
        foreach (RaycastHit hit in hits_center)
        {
            // 衝突先が壁タグであれば座標を取得
            if (hit.collider.CompareTag("Wall"))
            {
                center_point = hit.point;
            }
        }

        List<int> right_index = new List<int>();
        List<int> left_index = new List<int>();
        Vector3[] point = new Vector3[8];
        for (int i = 0; i < vertex.Count; i++)
        {
            // 左
            if (vertex[i].x < center_point.x)
            {
                // 下
                if (vertex[i].y < center_point.y)
                {
                    point[0] = vertex[i];
                }
                // 上
                else
                {
                    point[2] = vertex[i];
                }
            }
            // 右
            else
            {
                // 下
                if (vertex[i].y < center_point.y)
                {
                    point[1] = vertex[i];
                }
                // 上
                else
                {
                    point[3] = vertex[i];
                }
            }
        }

        // 立体にする
        for (int i = 0; i < 4; i++)
        {
            point[i + 4] = new Vector3(point[i].x, point[i].y, -0.1f);
        }
        new_mesh.vertices = point;

        //// インデックスを設定
        //var indices = new List<int>();
        //for (int i = 0; i < vertex.Count; i++)
        //{
        //    indices.Add(i);

        //    if (i == vertex.Count - 1)
        //    {
        //        indices.Add(0);
        //    }
        //    else
        //    {
        //        indices.Add(i + 1);
        //        //Debug.Log("index ：" + (i + 1));

        //    }
        //    //Debug.Log("index ：" + i);
        //}

        // 頂点インデックスを設定（立方体の場合）
        int[] triangles = new int[]
        {
            // 前面
            0, 1, 2, // 1つ目の三角形
            1, 3, 2, // 2つ目の三角形

            // 背面
            5, 4, 6, // 1つ目の三角形
            5, 6, 7, // 2つ目の三角形

            // 上面
            2, 3, 6, // 1つ目の三角形
            3, 7, 6, // 2つ目の三角形

            // 底面
            0, 4, 1, // 1つ目の三角形
            1, 4, 5, // 2つ目の三角形

            // 左面
            4, 0, 6, // 1つ目の三角形
            6, 0, 2, // 2つ目の三角形

            // 右面
            1, 5, 3, // 1つ目の三角形
            5, 7, 3, // 2つ目の三角形
        };
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

        BoxCollider boxCol = newObject.AddComponent<BoxCollider>();    // 当たり判定追加

        Rigidbody rb = newObject.AddComponent<Rigidbody>();     // 重力を加える
        rb.useGravity = true;

        newObject.transform.localScale = new Vector3(1.0f, 1.0f, 0.5f);

        create = false;
    }
}
