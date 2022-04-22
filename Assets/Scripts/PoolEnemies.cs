using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolEnemies : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    public static PoolEnemies Instance;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    #region Singleton
    void Awake()
    {
        Instance = this;
    }
    #endregion
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
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
            Debug.LogWarning("pool with tag" + tag + "doesn't exist.");
            return null;
        }
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
    public void ReturnToPool(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("pool with tag" + tag + "doesn't exist.");
            return;
        }
        foreach (GameObject obj in poolDictionary[tag])
        {
            if (obj.activeInHierarchy)
            {
                obj.SetActive(false);
            }

        }

    }
}