using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]

public class SCR_CreateNMesh : MonoBehaviour
{
    [SerializeField] float n;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var mesh = new Mesh();
        var vertex = new List<Vector3>();

        if(n < 1){
            n = 1;
        }

        //頂点座標の決定、この時の原点はtransform.position
        float rot = 2 * Mathf.PI / n;
        for (float angle = 0, k = 0; angle <= 2 * Mathf.PI; angle += rot, k += 1.0f){
            float x = Mathf.Cos(angle) * 2;
            float y = Mathf.Sin(angle) * 2;

            vertex.Add(new Vector3(x, y));
            Debug.Log(k + "：" + new Vector3(x, y));
        }

        var indices = new List<int>();
        for(int i = 0; i < vertex.Count; i++){
            indices.Add(i);

            if (i == vertex.Count - 1)
            {
                indices.Add(0);
            }
            else
            {
                indices.Add(i + 1);
                Debug.Log("index ：" + (i + 1));

            }
            Debug.Log("index ：" + i);
        }

        mesh.SetVertices(vertex);
        mesh.SetIndices(indices, MeshTopology.Lines, 0);

        //MeshFilterはshredMeshをMeshRendererに引き渡す。
        var mfilter = this.GetComponent<MeshFilter>();
        mfilter.sharedMesh = mesh;
        
    }
}
