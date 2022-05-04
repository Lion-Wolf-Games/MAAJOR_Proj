using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoints : MonoBehaviour
{
    [SerializeField] private Enemies linkedEnemy;

    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private int targetedPatrolPoint;

    private void Start()
    {
        linkedEnemy.transform.position = patrolPoints[0].position;
        linkedEnemy.SetTarget(patrolPoints[0].gameObject);
    }

    private void Update()
    {
        if (Vector3.Distance(linkedEnemy.transform.position, patrolPoints[targetedPatrolPoint].position) < 0.3f)
        {
            if (targetedPatrolPoint < patrolPoints.Length - 1)
            {
                targetedPatrolPoint++;
            }
            else
            {
                targetedPatrolPoint = 0;
            }

            linkedEnemy.SetTarget(patrolPoints[targetedPatrolPoint].gameObject);
            linkedEnemy.fsm = Enemies.FSM_Enemies.Idle;
            StartCoroutine(linkedEnemy.TurnToTargetAndChangeState(1f, Enemies.FSM_Enemies.Patroling));
        }
        if (linkedEnemy.fsm == Enemies.FSM_Enemies.Wander)
        {
            linkedEnemy.SetTarget(patrolPoints[targetedPatrolPoint].gameObject);
            linkedEnemy.fsm = Enemies.FSM_Enemies.Patroling;
        }
    }

    public void SetLinkedEnemy(Enemies enemy)
    {
        linkedEnemy = enemy;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if (patrolPoints.Length > 0)
        {
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                if (i < patrolPoints.Length - 1)
                {
                    Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i + 1].position);
                }
                else
                {
                    Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[0].position);
                }
            }
        }
    }
}
