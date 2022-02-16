using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    public Transform _poolParent;
    public GameObject _prefab;
    public int _initialPoolSize = 0;

    private List<GameObject> _objectPool;

    private void Start()
    {
        _objectPool = new List<GameObject>();

        if (_poolParent == null )
        {
            _poolParent = transform;
        }


        //Initialize Pool
        for (int i = 0; i < _initialPoolSize; i++)
        {
            SpawnObj();
        }
    }

    public GameObject GetFromPool(bool activate)
    {
        //Spawn Object if pool is empty
        if (_objectPool == null)
        {
            _objectPool = new List<GameObject>();
        }

        if (_objectPool.Count == 0)
        {
            SpawnObj();
        }

        //Get Object in pool
        GameObject go = _objectPool[0];
        _objectPool.Remove(go);
        go.SetActive(activate);

        //Set up BackToPool
        if (!go.TryGetComponent<BackToPool>(out var _backToPool))
        {
            _backToPool = go.AddComponent<BackToPool>();
        }
        _backToPool.SetPool(this);

        return go;
    }

    public void StoreInPool(GameObject obj)
    {
        _objectPool.Add(obj);
        obj.SetActive(false);
    }

    private void SpawnObj()
    {
        var go = Instantiate(_prefab, _poolParent);
        go.SetActive(false);
        _objectPool.Add(go);
    }

}
