using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PushableStone : MonoBehaviour
{
    [SerializeField] private Vector3 pos1,pos2;

    private Transform visual;
    //[SerializeField] private GameObject visual;

    private Vector3 nextPos;
    private bool isMoving;

    private void Start() {
        visual = transform.GetChild(0);
        transform.position = pos1;
        nextPos = pos2;
    }

    public void Push(Vector3 sourcePos)
    {
        if(isMoving) return;

        if(Vector3.Dot(transform.position - sourcePos, nextPos - transform.position) > 0.75f)
        {
            isMoving = true;
            transform.DOMove(nextPos,1f).OnComplete(()=> {isMoving = false;});

            if(nextPos == pos1)
            {
                nextPos = pos2;
            }
            else
            {
                nextPos = pos1;
            }
        }
        
    }

    
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;

        Gizmos.DrawSphere(pos1,0.5f);
        Gizmos.DrawSphere(pos2,0.5f);

        Gizmos.DrawLine(pos1,pos2);
    }

}
