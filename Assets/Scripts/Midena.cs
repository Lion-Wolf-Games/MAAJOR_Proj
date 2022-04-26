using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Midena : FightingObject
{
    public MidenaSate state,oldState;
    [SerializeField] private float travelSpeed;
    [SerializeField] private Animator anim;

    private Vector3 startPos;


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
            
            AttackTarget();
        }
    }

    void AttackTarget()
    {
        Vector3 dir = (_target.transform.position - transform.position) * 0.8f ;

        transform.DOMove(transform.position + dir,travelSpeed).OnComplete(() => {state = MidenaSate.Attack;});
        transform.LookAt(_target.transform);
        
        _target.GetComponent<Enemies>().StopMovement();
        
        anim.SetTrigger("Attack");
        anim.SetInteger("AttackType",Random.Range(1 ,6));
    }

    public void Attack()
    {
        if(_target != null)
        {
            _target.GetComponent<Enemies>().Damage(transform.position);
            _target = null;
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
