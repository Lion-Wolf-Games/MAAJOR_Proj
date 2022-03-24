using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Midena : MonoBehaviour
{
    public MidenaSate state,oldState;
    [SerializeField] private GameObject _target;
    [SerializeField] private float detectionRange;
    [SerializeField] private float attackDuration;
    [SerializeField] private LayerMask hostileLayer;
    [SerializeField] private int damage;
    [SerializeField] private float travelSpeed;
    [SerializeField] private Animator anim;

    private Vector3 startPos;
    private float attackTime;


    [SerializeField] private AK.Wwise.Event onAttack;

    private void Start() {
        startPos = transform.position;
    }

    private void Update() {
        
        if (oldState != state)
        {
            oldState = state;

            switch (state)
            {
                case MidenaSate.Attack:
                
                Destroy(_target);

                onAttack.Post(gameObject);
                attackTime = Time.time + attackDuration;
                break;
                case MidenaSate.Idle:
                break;
                case MidenaSate.Move:
                break;
                default:
                break;
            }
        }
        
        switch (state)
        {
            case MidenaSate.Attack:
            if(attackTime <= Time.time)
            {
                transform.DOMove(startPos,travelSpeed).OnComplete(()=>{state = MidenaSate.Idle;});
                state = MidenaSate.Move;
            }
            break;
            case MidenaSate.Idle:
            CheckForTarget();
            break;
            case MidenaSate.Move:
            break;
            default:
            break;
        }
    }

    void CheckForTarget()
    {
        if (Physics.CheckSphere(transform.position, detectionRange, hostileLayer))
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, detectionRange, hostileLayer);
            _target = cols[0].gameObject;
            state = MidenaSate.Move;
            transform.DOMove(_target.transform.position,travelSpeed).OnComplete(() => {state = MidenaSate.Attack;});
            transform.LookAt(_target.transform);

            
            anim.SetTrigger("Attack");
            anim.SetInteger("AttackType",Random.Range(0,6));
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position,detectionRange);
    }

}

public enum MidenaSate
{
    Idle,
    Move,
    Attack
}
