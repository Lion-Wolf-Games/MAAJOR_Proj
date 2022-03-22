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
    [Space]
    [SerializeField] private GameObject model;
    
}
