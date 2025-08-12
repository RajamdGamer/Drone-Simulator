using UnityEngine;
using System.Collections.Generic;
using System;
public class ObjectPool : MonoBehaviour
{
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public Queue<GameObject> pool = new Queue<GameObject>();

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singleton
    public static ObjectPool Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion


    public List<Pool> pools;

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            return null;
        }
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        
        if (objectToSpawn != null)
        {
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
        }
        objectToSpawn.SetActive(true);
        poolDictionary[tag].Enqueue(objectToSpawn);
        return objectToSpawn;
    }

    public void ReturnToPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
}
