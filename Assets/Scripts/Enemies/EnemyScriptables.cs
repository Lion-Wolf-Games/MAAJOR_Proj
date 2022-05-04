using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Enemy", menuName = "MAAJOR_Proj/Enemy", order = 0)]
public class EnemyScriptables : ScriptableObject {

    [SerializeField] private int health;
    [SerializeField] private float speed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float puddleSpeed;
    [SerializeField] private float detectionRange;
    [SerializeField] private float fleeRange;
    [SerializeField] private int attackDamage;
    [SerializeField] private float wanderRange;
    [SerializeField] private float timeBetweenWander;
    [SerializeField] private float respawnTime;
    [Space]
    [SerializeField] private GameObject model;

    public float GetSpeed()
    {
        return speed;
    }
    
    public float GetTurningSpeed()
    {
        return turnSpeed;
    }

    public float GetPuddleSpeed()
    {
        return puddleSpeed;
    }

    public GameObject GetModel()
    {
        return model;
    }

    public float GetRespawnTime()
    {
        return respawnTime;
    }
}
