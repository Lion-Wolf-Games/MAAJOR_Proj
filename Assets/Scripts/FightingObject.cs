using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingObject : MonoBehaviour {
    
    [Header("Detection")]
    [SerializeField] protected float timeBeforeLostTarget;
    [SerializeField] protected GameObject _target;
    [SerializeField] protected float detectionRange;

    [Space] [Header("Atttack")]
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackDuration;
    [SerializeField] protected LayerMask hostileLayer;
    [SerializeField] protected int damage;

    
    protected float attackTime;
}