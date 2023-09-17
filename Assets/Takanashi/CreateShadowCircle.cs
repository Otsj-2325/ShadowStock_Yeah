using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]

public class CreateShadowCircle : MonoBehaviour
{
    [Header("実体化する物")]
    [SerializeField] private GameObject circle;

    private MeshFilter mesh_filter;
    private Mesh mesh;

    private bool create = true;

    // Start is called before the first frame update
    void Start()
    {
        mesh_filter = circle.GetComponent<MeshFilter>();
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

        //頂点のY座標を取得
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices3.Add(vertices[i]);

            vertices2[i] = circle.gameObject.transform.position + vertices[i];
            if (!point_y.Contains(vertices2[i].y))
            {
                point_y.Add(vertices2[i].y);
            }
        }

        //===========================================
        // いらない座標を削除(裏面等)
        for (int i = 0; i < point_y.Count; i++)
        {
            List<int> right_max_index = new List<int>();  // 最も右側のベクトルのインデックス番号
            List<int> left_max_index = new List<int>();   // 最も左側のベクトルのインデックス番号

            float angle = 0.0f;
            float right_angle = -1.0f;
            float left_angle = -1.0f;
            for (int j = 0; j < vertices2.Length; j++)
            {
                angle = -1.0f;

                // 最も右側か左側の座標を求める(Y座標は同じ)
                if (vertices2[j].y == point_y[i])
                {
                    Vector3 temp_straight = circle.gameObject.transform.position - gameObject.transform.position;
                    Vector3 temp_this = vertices2[j] - gameObject.transform.position;
                    float temp_inner = Vector3.Dot(temp_straight, temp_this);
                    float angleRad = Mathf.Acos(temp_inner / (temp_straight.magnitude * temp_this.magnitude));
                    float angleDeg = angleRad * Mathf.Rad2Deg;
                    angle = angleDeg;

                    temp_straight.y = 0.0f;
                    temp_this.y = 0.0f;
                    Vector3 temp_outer = Vector3.Cross(temp_straight, temp_this);

                    // 点が右側にある
                    if (temp_outer.y > 0)
                    {
                        if (right_max_index.Count == 0)
                        {
                            right_angle = angle;
                            right_max_index.Add(j);
                        }

                        if (right_angle < angle)
                        {
                            right_angle = angle;
                            right_max_index.Clear();
                            right_max_index.Add(j);
                        }
                        else if (right_angle == angle)
                        {
                            right_max_index.Add(j);
                        }
                    }
                    // 点が左側にある
                    else
                    {
                        if (left_max_index.Count == 0)
                        {
                            left_angle = angle;
                            left_max_index.Add(j);
                        }

                        if (left_angle < angle)
                        {
                            left_angle = angle;
                            left_max_index.Clear();
                            left_max_index.Add(j);
                        }
                        else if (left_angle == angle)
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
        //===========================================


        HashSet<Vector3> vertex = new HashSet<Vector3>();
        List<float> vertex_x = new List<float>();

        //===========================================
        // レイを生成し壁に当て、影の座標を取得
        for (int i = 0; i < vertices3.Count; i++)
        {
            // レイの生成
            Vector3 dir = circle.gameObject.transform.position - gameObject.transform.position;
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
                    vertex_x.Add(hit.point.x);
                }
            }
        }

        // 影の中央座標を取得
        Vector3 center_point = new Vector3();    
        Vector3 dir_center = circle.gameObject.transform.position - gameObject.transform.position;
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
        //===========================================


        Mesh new_mesh = new Mesh();
        Vector3[] point = new Vector3[(vertex.Count + 1) * 2];

        //===========================================
        // X座標を右から見て、並べ替える(座標が時計回りになるように)
        int point_up_index = 1;
        int point_down_index = 0;
        bool point_center = false;
        vertex_x.Sort();
        for (int i = vertex_x.Count - 1; i > -1; i--)
        {
            // 前のX座標と同じ値であれば処理をしない
            if(i != vertex_x.Count - 1)
            {
                if (vertex_x[i] == vertex_x[i + 1])
                {
                    continue;
                }
            }

            foreach(Vector3 temp in vertex)
            {
                // 中央の点ははける
                if (temp == center_point)
                {
                    point_center = true;
                    continue;
                }

                // 右からのX座標と同じであれば座標を新しい変数に入れる
                if (vertex_x[i] == temp.x)
                {
                    // 上半分
                    if (center_point.y <= temp.y)
                    {
                        point[point_up_index++] = temp;
                    }
                    // 下半分
                    else
                    {
                        point[vertex.Count - point_down_index] = temp;
                        point_down_index++;
                    }
                }
            }
        }
        //===========================================

        point[0] = center_point;
        int vertex_num = 0;

        // 中央の座標が既に登録されていればカウントはそのまま
        if (point_center)
        {
            vertex_num = vertex.Count;
        }
        // 中央の座標分のカウントを入れる
        else
        {
            vertex_num = vertex.Count + 1;
        }

        // 立体にする
        for (int i = 0; i < vertex_num; i++)
        {
            point[i + vertex_num] = new Vector3(point[i].x, point[i].y, -0.1f);
        }
        new_mesh.vertices = point;


        //=======================================
        // 頂点インデックスの生成
        int[] triangles = new int[(vertex_num - 1) * 2 * 3 + (vertex_num - 1) * 2 * 3];

        // 表面
        for (int i = 0; i < (vertex_num - 2); i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 2;
            triangles[i * 3 + 2] = i + 1;
        }
        triangles[(vertex_num - 2) * 3] = 0;
        triangles[(vertex_num - 2) * 3 + 1] = 1;
        triangles[(vertex_num - 2) * 3 + 2] = vertex_num - 1;

        // 裏面
        for (int i = 0; i < (vertex_num - 2); i++)
        {
            triangles[(i + (vertex_num - 1)) * 3] = vertex_num;
            triangles[(i + (vertex_num - 1)) * 3 + 1] = vertex_num + i + 1;
            triangles[(i + (vertex_num - 1)) * 3 + 2] = vertex_num + i + 2;
        }
        triangles[((vertex_num - 1) * 2 - 1) * 3] = vertex_num;
        triangles[((vertex_num - 1) * 2 - 1) * 3 + 1] = vertex_num - 1 + vertex_num;
        triangles[((vertex_num - 1) * 2 - 1) * 3 + 2] = vertex_num + 1;

        // 横面
        for (int i = 1; i < (vertex_num - 1); i++)
        {
            int temp_num = ((vertex_num - 1) * 2 - 2) + i * 2;
            triangles[temp_num * 3] = i;
            triangles[temp_num * 3 + 1] = i + 1;
            triangles[temp_num * 3 + 2] = i + vertex_num + 1;

            triangles[(temp_num + 1) * 3] = i;
            triangles[(temp_num + 1) * 3 + 1] = i + vertex_num + 1;
            triangles[(temp_num + 1) * 3 + 2] = i + vertex_num;
        }

        triangles[(((vertex_num - 1) * 2 - 2) + (vertex_num - 1) * 2) * 3] = vertex_num - 1;
        triangles[(((vertex_num - 1) * 2 - 2) + (vertex_num - 1) * 2) * 3 + 1] = 1;
        triangles[(((vertex_num - 1) * 2 - 2) + (vertex_num - 1) * 2) * 3 + 2] = vertex_num + 1;

        triangles[(((vertex_num - 1) * 2 - 2) + (vertex_num - 1) * 2 + 1) * 3] = vertex_num - 1;
        triangles[(((vertex_num - 1) * 2 - 2) + (vertex_num - 1) * 2 + 1) * 3 + 1] = vertex_num + 1;
        triangles[(((vertex_num - 1) * 2 - 2) + (vertex_num - 1) * 2 + 1) * 3 + 2] = vertex_num + vertex_num - 1;

        new_mesh.triangles = triangles;
        //=======================================


        //===========================================
        // 上のMesh情報からの立体の設計
        // 法線を計算して設定
        new_mesh.RecalculateNormals();

        GameObject newObject = new GameObject("NewMeshObject");

        //MeshFilterはshredMeshをMeshRendererに引き渡す。
        MeshFilter mfilter = newObject.AddComponent<MeshFilter>();
        mfilter.mesh = new_mesh;

        // MeshRendererを追加してマテリアルをアタッチ
        MeshRenderer meshRenderer = newObject.AddComponent<MeshRenderer>();
        MeshCollider meshCollider = newObject.AddComponent<MeshCollider>();
        meshCollider.convex = true;

        newObject.transform.position = Vector3.zero; // 必要な位置に変更する

        newObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }
}
