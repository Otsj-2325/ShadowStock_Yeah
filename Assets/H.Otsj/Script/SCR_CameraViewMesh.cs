using UnityEngine;

public class SCR_CameraViewMesh : MonoBehaviour
{
    // 生成するポリゴンのマテリアル
    public Material polygonMaterial;

    // カメラの参照
    private Camera mainCamera;

    void Start()
    {
        // カメラへの参照を取得
        mainCamera = Camera.main;
    }

    void Update()
    {
        // カメラのビューポートの中心を取得
        Vector3 viewportCenter = new Vector3(0.5f, 0.5f, mainCamera.nearClipPlane);

        // カメラのビューポートの中心にRayを飛ばす
        Ray ray = mainCamera.ViewportPointToRay(viewportCenter);

        // Rayが衝突する位置を求める
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // ヒットした位置を取得
            Vector3 hitPoint = hit.point;

            // カメラからヒット位置に向かうベクトルを求める
            Vector3 toHitPoint = hitPoint - mainCamera.transform.position;

            // ヒット位置から少し奥に移動させる距離（適宜調整してください）
            float offsetDistance = 10.0f;

            // ヒット位置から少し奥に移動させる
            Vector3 newPosition = hitPoint + toHitPoint.normalized * offsetDistance;

            // 新しいポリゴンを生成
            GameObject newPolygon = new GameObject("GeneratedPolygon");
            newPolygon.transform.position = newPosition;
            newPolygon.transform.rotation = mainCamera.transform.rotation;

            // 新しいポリゴンにMeshFilterとMeshRendererを追加
            MeshFilter meshFilter = newPolygon.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = newPolygon.AddComponent<MeshRenderer>();

            // 新しいポリゴンのメッシュを作成
            Mesh mesh = new Mesh();

            // メッシュの頂点情報を設定（適宜頂点情報を設定してください）
            Vector3[] vertices = new Vector3[]
            {
                new Vector3(-0.5f, -0.5f, 0f),
                new Vector3(0.5f, -0.5f, 0f),
                new Vector3(0.5f, 0.5f, 0f),
                new Vector3(-0.5f, 0.5f, 0f)
            };
            mesh.vertices = vertices;

            // メッシュのポリゴンを設定（適宜ポリゴン情報を設定してください）
            int[] triangles = new int[]
            {
                0, 1, 2,
                2, 3, 0
            };
            mesh.triangles = triangles;

            // メッシュを設定
            meshFilter.mesh = mesh;

            // マテリアルを設定
            meshRenderer.material = polygonMaterial;
        }
    }
}
