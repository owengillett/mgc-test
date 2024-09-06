using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BFS.Essential;

public class DemoPooler : MonoBehaviour
{
    public Transform _Sphere;
    Pooler<Transform> m_PoolSphere;
    public Pooler<Transform> PoolSphere
    {
        get
        {
            if (m_PoolSphere == null)
                m_PoolSphere = new Pooler<Transform>(3, _Sphere);

            return m_PoolSphere;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < 10; i++)
                PoolSphere.Spawn(Vector3.right * i, Quaternion.identity);

            Debug.Log("PoolCount: " + PoolSphere.Count);
        }
    }

}
