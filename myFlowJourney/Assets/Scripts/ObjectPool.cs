using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
     [System.Serializable]
    public class Pool
    {
        public GameObject prefab;
        public int size;
    }

    public Pool[] pools;
    private Dictionary<GameObject, Queue<GameObject>> poolDictionary;

    void Start()
    {
        poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();

        foreach (var pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.prefab, objectPool);
        }
    }

    public GameObject GetObject(GameObject prefab)
    {
        if (poolDictionary.TryGetValue(prefab, out Queue<GameObject> objectPool))
        {
            if (objectPool.Count > 0)
            {
                GameObject obj = objectPool.Dequeue();
                obj.SetActive(true);
                return obj;
            }
            else
            {
                // Optionally expand the pool
                GameObject obj = Instantiate(prefab);
                return obj;
            }
        }
        Debug.LogError("Pool with prefab not found.");
        return null;
    }

    public void ReturnObject(GameObject prefab, GameObject obj)
    {
        if (poolDictionary.TryGetValue(prefab, out Queue<GameObject> objectPool))
        {
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }
        else
        {
            Debug.LogError("Pool with prefab not found.");
            Destroy(obj); // Optionally destroy if the pool is not found
        }
    }
}
