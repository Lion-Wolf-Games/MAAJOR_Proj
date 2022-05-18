using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private enum State
    {
        Wander,
        Chase,
        Attack,
    }
    [SerializeField] private State currentState;
    [SerializeField] private EnemyScriptables enemyType;

    [SerializeField] private Vector3 initialPos;
    [SerializeField] private LayerMask targettedLayer;

    private void Start()
    {
        initialPos = transform.position;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Wander:
                break;
            case State.Chase:
                break;
            case State.Attack:
                break;
            default:
                break;
        }
    }

    private void TargetCheck()
    {

    }
}
