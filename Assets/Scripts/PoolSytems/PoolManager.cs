using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    private Dictionary<GameObject, Pool> _pools;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        _pools = new Dictionary<GameObject, Pool>();

        foreach (var pool in gameObject.GetComponentsInChildren<Pool>())
        {
            _pools.Add(pool._prefab, pool);
        }
    }

    public GameObject Spawn(GameObject obj, bool activateOnSpawn)
    {
        if (_pools == null)
        {
            _pools = new Dictionary<GameObject, Pool>();
        }

        if (_pools.TryGetValue(obj,out Pool pool))
        {
            return pool.GetFromPool(activateOnSpawn);
        }
        else
        {
            Debug.Log("Pool Of this GameObject Not Found!\nCreating One");

            GameObject newPool = new GameObject();
            newPool.name = "Pool " + obj.name; 
            newPool.transform.SetParent(transform);
            pool = newPool.AddComponent<Pool>();
            pool._prefab = obj;
            _pools.Add(obj, pool);

            return pool.GetFromPool(activateOnSpawn);

        }
    }


    public GameObject Spawn(GameObject obj, bool activateOnSpawn, Vector3 pos,Quaternion rot)
    {
        var go = Spawn(obj, false);

        go.transform.position = pos;
        go.transform.rotation = rot;

        go.SetActive(activateOnSpawn);

        return go;
    }
}
