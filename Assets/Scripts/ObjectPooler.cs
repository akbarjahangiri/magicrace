using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectPooler : MonoBehaviour
{
    public List<Pool> objectPools;
    public Dictionary<string, Queue<GameObject>> PoolDictionary;
    private GameObject _objectToSpawn;
    [System.NonSerialized] public GameObject _lastPoolObject = null;
    
    [System.Serializable]
    public class Pool
    {
        //this pool is limited type of pool, we must declare prefab of multiple kind of gameObject;
        // public GameObject prefab;
        public List<GameObject> prefabs;
        public string type;
    }
    
    private void Start()
    {
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (var pool in objectPools)
        {
            Queue<GameObject> innerPool = new Queue<GameObject>();
            for (int i = 0; i < pool.prefabs.Count; i++)
            {
                GameObject obj = Instantiate(pool.prefabs[Random.Range(0, pool.prefabs.Count)]);
                obj.SetActive(false);
                innerPool.Enqueue(obj);
            }

            PoolDictionary.Add(pool.type, innerPool);
        }
    }

    public GameObject SpawnFromPool(string type, Vector3 position, Quaternion rotation)
    {
        if (!PoolDictionary.ContainsKey(type))
        {
            return null;
        }
        _objectToSpawn = PoolDictionary[type].Dequeue();
        _objectToSpawn.transform.position = position;
        _objectToSpawn.transform.rotation = rotation;
        _objectToSpawn.SetActive(true);
        if (type == "ground")
        {
            _lastPoolObject = _objectToSpawn.gameObject;
        }

        PoolDictionary[type].Enqueue(_objectToSpawn);
        return _objectToSpawn;
    }
}