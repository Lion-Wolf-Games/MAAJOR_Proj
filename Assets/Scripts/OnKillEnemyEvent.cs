using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnKillEnemyEvent : MonoBehaviour
{
    [SerializeField] List<Enemies> enemies;

    public UnityEvent events;
    int enemyCount;

    private void Start() {

        if(enemies != null)
        {
            foreach (var en in enemies)
            {
                en.OnKill += OnKilled;
            }

            enemyCount = enemies.Count;

        }
    }

    private void OnKilled(GameObject _)
    {
        //events?.Invoke();

        enemyCount--;

        if(enemyCount <= 0)
        {
            events?.Invoke();
        }
    }

    

}
