using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_RayCastMesh : MonoBehaviour
{
    HashSet<GameObject> m_ObjectList = new HashSet<GameObject>();
    List<Vector3> m_MeshVertices = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitdata;

        for (float rot = 0; rot < (2 * Mathf.PI) / 8; rot += Mathf.PI / 18.0f)
        {
            for (float rot2 = 0; rot2 < (2 * Mathf.PI) / 8; rot2 += Mathf.PI / 18.0f)
            {
                Vector3 dir;
                dir.x = Mathf.Cos(rot) * Mathf.Cos(rot2);
                dir.y = Mathf.Cos(rot) * Mathf.Sin(rot2);
                dir.z = Mathf.Sin(rot);

                if (Physics.Raycast(transform.position, dir * 3.0f, out hitdata))
                {
                    if(hitdata.collider != null){
                        Debug.DrawRay(transform.position, dir * 3.0f, Color.blue);
                        m_ObjectList.Add(hitdata.collider.gameObject);
                    }
                    else{
                        Debug.Log("Hit object : None"   );
                    }
                }

            }
        }

        foreach(GameObject obj in m_ObjectList){
            var mFilter = obj.GetComponent<MeshFilter>();
            var ms = mFilter.sharedMesh;
            var vertices = ms.vertices;

            int i = 0;
            foreach(var pos in vertices){
                Debug.Log(i++ + " : " + pos);
                var offsetPos = pos + obj.transform.position;
                var angles = obj.transform.eulerAngles;
                Ray objVerticesRay = new Ray(obj.transform.TransformPoint(offsetPos), obj.transform.forward);
                RaycastHit VRayHitData;

                if(Physics.Raycast(objVerticesRay, out VRayHitData)){
                    m_MeshVertices.Add(VRayHitData.collider.gameObject.transform.position);
                }

                Debug.DrawRay(offsetPos, obj.transform.forward, Color.red);
            }
        }

        //if (m_MeshVertices.Count > 3)
        //{
        //    Mesh createMesh = new Mesh();
        //    createMesh.SetVertices(m_MeshVertices);

        //    List<int> MeshIndices = new List<int>();
        //    for (int i = 0; i < m_MeshVertices.Count; i++)
        //    {
        //        MeshIndices.Add(i);
        //    }
        //    createMesh.SetIndices(MeshIndices, MeshTopology.Lines, 0);

        //    GameObject newMesh = new GameObject();
        //    newMesh.AddComponent<MeshRenderer>();
        //    MeshFilter mf = newMesh.AddComponent<MeshFilter>();
        //    mf.sharedMesh = createMesh;

        //    GameObject.Instantiate(newMesh);
        //}
    }
}
