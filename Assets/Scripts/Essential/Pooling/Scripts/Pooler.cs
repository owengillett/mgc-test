using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BFS.Essential
{
    public class Pooler<t> where t : Component
    {
        int m_CurrentIndex = 0;
        t m_Object = null;
        List<t> m_Pool = new List<t>();

        public int Count => m_Pool.Count;

        t CreatPool()
        {
            t result = GameObject.Instantiate(m_Object, Vector3.zero, Quaternion.identity);
            m_Pool.Add(result);
            result.transform.name = m_Object.name + "_" + m_Pool.Count;
            result.gameObject.SetActive(false);
            return result;
        }

        public Pooler(int quantityAtStart, t objectToPool)
        {
            m_Object = objectToPool;

            for (int i = 0; i < quantityAtStart; i++)
                CreatPool();
        }

        public t Spawn(Vector3 position, Quaternion rotation)
        {
            t result;
            int round = 0;
            do
            {
                result = (round >= m_Pool.Count) ? CreatPool() : m_Pool[m_CurrentIndex];
                m_CurrentIndex = (m_CurrentIndex + 1) % m_Pool.Count;
                round++;
            } while (result.gameObject.activeSelf);

            Transform trans = result.transform;
            trans.position = position;
            trans.rotation = rotation;

            result.gameObject.SetActive(true);

            return result;
        }
    }
}