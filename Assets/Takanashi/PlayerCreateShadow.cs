using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]

public class PlayerCreateShadow : MonoBehaviour
{
    [Header("実体化する物")]
    [SerializeField] private GameObject circle;
    [Header("実体化する左枠")]
    [SerializeField] private GameObject playerFlameLeft;
    [Header("実体化する右枠")]
    [SerializeField] private GameObject playerFlameRight;

    private Mesh mesh;

    private bool create = true;

    // 影と当たった枠の種類
    private enum CollitedPlayerFlame
    {
        Left,
        Down,
        Right,
        Up,
        None
    }

    // Start is called before the first frame update
    void Start()
    {
        mesh = circle.GetComponent<MeshFilter>().mesh;
    }

    // Update is called once per frame
    void Update()
    {
        // Eキーを押したら影の形からオブジェクトを生成
        //if (!Input.GetKeyDown(KeyCode.E)) return;

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
                // 衝突先が壁タグでなければ処理しない
                if (!hit.collider.CompareTag("Wall")) continue;

                vertex.Add(new Vector3(hit.point.x, hit.point.y, hit.point.z));
                vertex_x.Add(hit.point.x);
            }
        }

        // Eキーを押したら影の形からオブジェクトを生成
        if (!Input.GetKeyDown(KeyCode.E)) return;

        // 影の中央座標を取得
        Vector3 center_point = new Vector3();
        Vector3 dir_center = circle.gameObject.transform.position - gameObject.transform.position;
        Ray ray_center = new Ray(gameObject.transform.position, dir_center);
        RaycastHit[] hits_center = Physics.RaycastAll(ray_center);
        foreach (RaycastHit hit in hits_center)
        {
            // 衝突先が壁タグであれば座標を取得
            if (!hit.collider.CompareTag("Wall")) continue;

            center_point = hit.point;
        }
        //===========================================

        Mesh new_mesh = new Mesh();
        Vector3[] point = new Vector3[vertex.Count + 1];
        for(int i = 0; i < point.Length; i++)
        {
            point[i] = Vector3.zero;
        }

        //===========================================
        // X座標を右から見て、並べ替える(座標が時計回りになるように)
        int point_up_index = 1;
        int point_down_index = 0;
        vertex_x.Sort();
        for (int i = vertex_x.Count - 1; i > -1; i--)
        {
            // 前のX座標と同じ値であれば処理をしない
            if (i != vertex_x.Count - 1)
            {
                if (vertex_x[i] == vertex_x[i + 1])
                {
                    continue;
                }
            }

            foreach (Vector3 temp in vertex)
            {
                // 中央の点ははける
                if (temp == center_point) continue;

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

        // プレイヤーの枠内に入ってるかどうか
        List<int> inFlameIndex = new List<int>();   // 枠内に入ってる点のインデックス番号記録
        for(int i = 1; i < point.Length; i++)
        {
            // 初期状態であって、枠のゲームオブジェクトがなければ処理しない
            if (point[i] == Vector3.zero ||
                playerFlameLeft == null || playerFlameRight == null) continue;

            // 枠内にあるか
            float flameUp = playerFlameLeft.transform.position.y + playerFlameLeft.transform.localScale.y / 2;
            float flameDown = playerFlameLeft.transform.position.y - playerFlameLeft.transform.localScale.y / 2;
            if (playerFlameLeft.transform.position.x > point[i].x ||
                playerFlameRight.transform.position.x < point[i].x ||
                flameUp < point[i].y || flameDown > point[i].y)
                continue;

            inFlameIndex.Add(i);
        }

        // 枠内にない
        if (inFlameIndex.Count == 0) return;

        int upPointIndex = -1;          // 時計回りをして前の点が枠外で次の点が枠内のもの
        int upPointBeforeIndex = -1;    // 時計回りをして前の点が枠外で次の点が枠内の前の点の番号
        int downPointIndex = -1;        // 時計回りをして前の点が枠内で次の点が枠外のもの
        int downPointNextIndex = -1;    // 時計回りをして前の点が枠内で次の点が枠外の次の点の番号

        // 上の変数のそれぞれの衝突した枠の種類
        CollitedPlayerFlame upPointCollitedPlayerFlame = CollitedPlayerFlame.None;
        CollitedPlayerFlame downPointCollitedPlayerFlame = CollitedPlayerFlame.None;

        int pointNum = 0;       // 新しい点を保存する変数の数

        bool inFlameAll = false;

        // 全て枠内にあるなら
        if (inFlameIndex.Count == point.Length - 1)
        {
            pointNum = vertex.Count + 1;
            inFlameAll = true;
        }
        else
        {
            for (int i = 1; i < point.Length; i++)
            {
                if (inFlameIndex.IndexOf(i) == -1) continue;

                // 時計回りをして前の点が枠外で次の点が枠内のもの
                if (i == 1 && upPointIndex == -1)
                {
                    if (inFlameIndex.IndexOf(point.Length - 1) != -1) continue;

                    upPointIndex = i;
                    upPointBeforeIndex = point.Length - 1;
                    break;
                }
                else if(upPointIndex == -1)
                {
                    if (inFlameIndex.IndexOf(i - 1) != -1) continue;

                    upPointIndex = i;
                    upPointBeforeIndex = i - 1;
                    break;
                }
            }

            for (int i = 1; i < point.Length; i++)
            {
                if (inFlameIndex.IndexOf(i) == -1) continue;

                // 時計回りをして前の点が枠内で次の点が枠外のもの
                if (i == point.Length - 1 && downPointIndex == -1)
                {
                    if (inFlameIndex.IndexOf(1) != -1) continue;

                    downPointIndex = i;
                    downPointNextIndex = 1;
                    break;
                }
                else if (downPointIndex == -1)
                {
                    if (inFlameIndex.IndexOf(i + 1) != -1) continue;

                    downPointIndex = i;
                    downPointNextIndex = i + 1;
                    break;
                }

            }

            // レイを打つ
            // 時計回りをして前の点が枠外で次の点が枠内のもの
            Vector3 dir = point[upPointBeforeIndex] - point[upPointIndex];
            Ray ray = new Ray(point[upPointIndex], dir);
            Debug.DrawRay(ray.origin, ray.direction * 30, Color.red, 0.1f); // 長さ３０、赤色で５秒間可視化
            RaycastHit[] hits = Physics.RaycastAll(ray);
            foreach (RaycastHit hit in hits)
            {
                // 衝突した枠の種類を保存
                if (hit.collider.CompareTag("PlayerFlameLeft"))
                    upPointCollitedPlayerFlame = CollitedPlayerFlame.Left;
                else if (hit.collider.CompareTag("PlayerFlameRight"))
                    upPointCollitedPlayerFlame = CollitedPlayerFlame.Right;
                else if (hit.collider.CompareTag("PlayerFlameUp"))
                    upPointCollitedPlayerFlame = CollitedPlayerFlame.Up;
                else if (hit.collider.CompareTag("PlayerFlameDown"))
                    upPointCollitedPlayerFlame = CollitedPlayerFlame.Down;
                else
                    continue;    // 衝突先がプレイヤーの枠タグでなければ処理しない

                point[upPointBeforeIndex] = hit.point;
                inFlameIndex.Add(upPointBeforeIndex);
                break;
            }

            // 時計回りをして前の点が枠内で次の点が枠外のもの
            dir = point[downPointNextIndex] - point[downPointIndex];
            ray = new Ray(point[downPointIndex], dir);
            Debug.DrawRay(ray.origin, ray.direction * 30, Color.red, 0.1f); // 長さ３０、赤色で５秒間可視化
            hits = Physics.RaycastAll(ray);
            foreach (RaycastHit hit in hits)
            {
                // 衝突した枠の種類を保存
                if (hit.collider.CompareTag("PlayerFlameLeft"))
                    downPointCollitedPlayerFlame = CollitedPlayerFlame.Left;
                else if (hit.collider.CompareTag("PlayerFlameRight"))
                    downPointCollitedPlayerFlame = CollitedPlayerFlame.Right;
                else if (hit.collider.CompareTag("PlayerFlameUp"))
                    downPointCollitedPlayerFlame = CollitedPlayerFlame.Up;
                else if (hit.collider.CompareTag("PlayerFlameDown"))
                    downPointCollitedPlayerFlame = CollitedPlayerFlame.Down;
                else
                    continue;    // 衝突先がプレイヤーの枠タグでなければ処理しない

                point[downPointNextIndex] = hit.point;
                inFlameIndex.Add(downPointNextIndex);
                break;
            }

            //========================================================
            // 枠の角が新しい図形に入っていれば角を変数に入れる
            Vector3 cornerPos = Vector3.zero;   // 枠の角
            // 左下
            if (upPointCollitedPlayerFlame == CollitedPlayerFlame.Down &&
                downPointCollitedPlayerFlame == CollitedPlayerFlame.Left)
            {
                cornerPos = playerFlameLeft.transform.position;
                cornerPos.y = playerFlameLeft.transform.position.y - playerFlameLeft.transform.localScale.y / 2;
            }
            // 右下
            else if (upPointCollitedPlayerFlame == CollitedPlayerFlame.Right &&
                     downPointCollitedPlayerFlame == CollitedPlayerFlame.Down)
            {
                cornerPos = playerFlameRight.transform.position;
                cornerPos.y = playerFlameRight.transform.position.y - playerFlameRight.transform.localScale.y / 2;
            }
            // 右上
            else if (upPointCollitedPlayerFlame == CollitedPlayerFlame.Up &&
                     downPointCollitedPlayerFlame == CollitedPlayerFlame.Right)
            {
                cornerPos = playerFlameRight.transform.position;
                cornerPos.y = playerFlameRight.transform.position.y + playerFlameRight.transform.localScale.y / 2;
            }
            // 左上
            else if (upPointCollitedPlayerFlame == CollitedPlayerFlame.Left &&
                     downPointCollitedPlayerFlame == CollitedPlayerFlame.Up)
            {
                cornerPos = playerFlameLeft.transform.position;
                cornerPos.y = playerFlameLeft.transform.position.y + playerFlameLeft.transform.localScale.y / 2;
            }

            // 変数に入れる
            if (cornerPos != Vector3.zero)
            {
                // 前の点が外次の点が内の、前のインデックス番号を角の座標にする
                if(inFlameIndex.IndexOf(upPointBeforeIndex - 1) == -1)
                {
                    point[upPointBeforeIndex - 1] = cornerPos;
                    inFlameIndex.Add(upPointBeforeIndex - 1);
                }
            }
            //========================================================

            pointNum = inFlameIndex.Count + 1;
        }

        Vector3[] pointInFlame = new Vector3[pointNum * 2];
        int inFlameCount = 1;   // 新しい変数のインデックス番号のカウント

        // 新しい変数に切り替え
        if(inFlameAll)
        {
            for (int i = 1; i < point.Length; i++)
            {
                pointInFlame[i] = point[i];
            }
            pointInFlame[0] = center_point;     // 中心座標
        }
        else
        {
            for (int i = 1; i < point.Length; i++)
            {
                if (inFlameIndex.IndexOf(i) == -1) continue;

                pointInFlame[inFlameCount] = point[i];
                inFlameCount++;
            }

            // 新しい図形の中心
            // 新しい図形の各端を求め、その中心を図形の中心とする
            float mostLeftPos = pointInFlame[1].x;
            float mostRightPos = pointInFlame[1].x;
            float mostUpPos = pointInFlame[1].y;
            float mostDownPos = pointInFlame[1].y;
            for (int i = 2; i < point.Length; i++)
            {
                if (pointInFlame[i].x < mostLeftPos)
                    mostLeftPos = pointInFlame[i].x;
                else if (pointInFlame[i].x > mostRightPos)
                    mostRightPos = pointInFlame[i].x;

                if (pointInFlame[i].y < mostDownPos)
                    mostDownPos = pointInFlame[i].y;
                else if (pointInFlame[i].y > mostUpPos)
                    mostUpPos = pointInFlame[i].y;
            }
            Vector3 tempCenterPos = center_point;
            tempCenterPos.x = mostLeftPos + (mostRightPos - mostLeftPos) / 2;
            tempCenterPos.y = mostDownPos + (mostUpPos - mostDownPos) / 2;
            pointInFlame[0] = tempCenterPos;
        }

        int vertex_num = pointNum;

        // 立体にする
        for (int i = 0; i < vertex_num; i++)
        {
            pointInFlame[i + vertex_num] = new Vector3(pointInFlame[i].x, pointInFlame[i].y, -0.1f);
        }
        new_mesh.vertices = pointInFlame;


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
