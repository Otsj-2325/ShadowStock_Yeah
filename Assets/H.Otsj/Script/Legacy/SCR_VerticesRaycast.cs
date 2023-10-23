using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_VerticesRaycast : MonoBehaviour
{
    MeshFilter m_mFilter;
    Mesh m_mesh;

    // Start is called before the first frame update
    void Start()
    {
        m_mFilter = GetComponent<MeshFilter>();//MeshFilterを取得しておいて
        m_mesh = m_mFilter.sharedMesh;//Meshを取得して

    }

    // Update is called once per frame
    void Update()
    {
        var vertices = m_mesh.vertices.Clone() as Vector3[];//取得したMeshの頂点情報を取ってきて
        List<Vector3> calcPoint = new List<Vector3>();

        var rot = transform.rotation;

        foreach (var point in vertices)
        {
            var p = point;

            /*ここで、　vertices　に　transform.rotation　を適用させたい*/
            /*ときは、頂点座標にtransform.rotationを掛けるだけ*/
            p = rot * p;

            //transform.Scaleの適用
            p.x *= transform.localScale.x;
            p.y *= transform.localScale.y;
            p.z *= transform.localScale.z;

            //transform.positionの適用
            p += transform.position;

            calcPoint.Add(p);
        }

        int i = 0;
        foreach (var point in calcPoint) {
            Debug.Log( i++ + " : " + point);

            Debug.DrawRay(point, transform.forward, Color.blue);
        }
    }
}
