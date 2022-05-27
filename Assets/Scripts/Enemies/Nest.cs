using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nest : MonoBehaviour
{
    [SerializeField] private List<EnemyScriptables> enemiesToSpawn;
    [SerializeField] private GameObject enemiePrefab;
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private GameObject destroyFX;

    public Action OnNestDestroy;
    private SuckableObject suckableObject;

    private void Start() {

        suckableObject = GetComponent<SuckableObject>();

        if(enemies == null || enemies.Count == 0)
        {
            enemies = new List<GameObject>();
        }

        SpawnEnemies();   

        // foreach (var go in enemies)
        // {
        //     go.GetComponent<Enemies>().OnKill += OnEnemyKilled;
        // }

        suckableObject.OnSucked += DestroyNest;
    }

    private void SpawnEnemies()
    {
        foreach (var ennemieScriptable in enemiesToSpawn)
        {
            Enemies enemie = Instantiate(enemiePrefab,transform.position,Quaternion.identity).GetComponent<Enemies>();
            
            enemie.SetUpEnemies(ennemieScriptable);

            enemies.Add(enemie.gameObject);

        }
    }


    // private void OnEnemyKilled(GameObject ennemyKilled)
    // {
    //     if(enemies.Contains(ennemyKilled))
    //     {
    //         ennemyKilled.GetComponent<Enemies>().OnKill -= OnEnemyKilled;

    //         enemies.Remove(ennemyKilled);

    //         if(enemies.Count == 0)
    //         {
    //             DestroyNest();
    //         }
    //     }
    // }

    private void DestroyNest()
    {
        Debug.Log("Destroy Nest");
        OnNestDestroy?.Invoke();
        PoolManager.Instance.Spawn(destroyFX,true,transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos() {

        Gizmos.color = Color.black;

        if(!Application.isPlaying)
        {
            return;
        }

        foreach (var enemy in enemies)
        {
            Gizmos.DrawLine(transform.position + Vector3.up *0.5f,enemy.transform.position + Vector3.up * 0.5f);
        }
    }


}
