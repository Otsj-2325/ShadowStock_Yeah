using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SCR_CreateMeshをアタッチするのに必須で、なければ自動追加してくれるアトリビュート（属性）
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]

public class SCR_CreateMesh : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var mesh = new Mesh();
        var vertex = new Vector3[3];
        
        //頂点座標の決定、この時の原点はtransform.position
        vertex[0] = new Vector3(-0.5f, -0.5f);
        vertex[1] = new Vector3(0.0f, 0.5f);
        vertex[2] = new Vector3(0.5f, -0.5f);

        //Meshインスタンスに頂点とインデックスを設定
        mesh.SetVertices(vertex);
        mesh.SetTriangles(new int[] { 0, 1, 2 }, 0);

        //MeshFilterはshredMeshをMeshRendererに引き渡す。
        var mfilter = this.GetComponent<MeshFilter>();
        mfilter.sharedMesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
