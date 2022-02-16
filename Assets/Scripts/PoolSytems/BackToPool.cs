using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToPool : MonoBehaviour
{
    private Pool _pool;

    public void SetPool(Pool pool)
    {
        _pool = pool;
    }

    private void OnDisable()
    {
        _pool.StoreInPool(gameObject);
    }
}
