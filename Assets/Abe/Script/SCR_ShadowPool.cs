using UnityEngine;
using UnityEngine.Pool;

public class SCR_ShadowPool : MonoBehaviour
{

    ObjectPool<GameObject> m_Pool;

    public GameObject Prefab { get; private set; }

    void Awake()
    {
        m_Pool = new ObjectPool<GameObject>(OnCreatePooledObject, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject);
    }

    GameObject OnCreatePooledObject()
    {
        return Instantiate(Prefab);
    }

    void OnGetFromPool(GameObject obj)
    {
        obj.SetActive(true);
    }

    void OnReleaseToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    void OnDestroyPooledObject(GameObject obj)
    {
        Destroy(obj);
    }

    public GameObject GetGameObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        Prefab = prefab;
        GameObject obj = m_Pool.Get();
        Transform tf = obj.transform;
        tf.position = position;
        tf.rotation = rotation;

        return obj;
    }

    public void ReleaseGameObject(GameObject obj)
    {
        m_Pool.Release(obj);
    }

}