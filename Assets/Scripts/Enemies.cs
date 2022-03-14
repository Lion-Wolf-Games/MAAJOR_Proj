using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemies : MonoBehaviour
{
    public int hp = 3;

    private Rigidbody rb;
    private NavMeshAgent navAgent;
    [SerializeField] private Animator anim;
    
    [Space] [Header("Chase")]
    [SerializeField] private GameObject _target;
    [SerializeField] private float detectionRange;
    [SerializeField] private float timeBeforeLostTarget;

    [Space] [Header("Atttack")]
    [SerializeField] private float attackRange;
    [SerializeField] private float attackDuration;
    [SerializeField] private LayerMask hostileLayer;
    [SerializeField] private int damage;

    [Space] [Header("Wander")]
    [SerializeField] private float wanderRange;
    [SerializeField] private float timeBetweenWander;
    [SerializeField] private Vector3 spawnPosition;

    [Space] [Header("Panic")]
    [SerializeField] private float panicRange;
    [SerializeField] private float timeBetweenDirChange;
    [SerializeField] private GameObject sfxPanic;


    private Vector3 wanderPos;
    private float wanderTime;
    private float attackTime;
    private float panicTime;
    private float panicDuration;
    private Vector3 lastTargetPos;

    public enum FSM_Enemies { Idle, Wander, Chasing, Attacking, TargetLost, Hit, Panic}
    [SerializeField] private FSM_Enemies fsm;
    private FSM_Enemies fsmOld;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        navAgent = GetComponent<NavMeshAgent>();

        fsm = FSM_Enemies.Idle;
        fsmOld = FSM_Enemies.Idle;

        spawnPosition = transform.position;
        wanderPos = spawnPosition;
        sfxPanic.SetActive(false);
    }

    private void Update()
    {
        if (fsm != fsmOld)
        {
            fsmOld = fsm;

            switch (fsm)
            {
                case FSM_Enemies.Idle:
                    wanderTime = Time.time + timeBetweenWander;

                    if(anim != null)
                        anim.SetBool("IsMoving", false);
                    navAgent.SetDestination(transform.position);
                    break;
                case FSM_Enemies.Wander:

                    if (anim != null)
                        anim.SetBool("IsMoving",true);
                    break;
                case FSM_Enemies.Chasing:

                    if (anim != null)
                        anim.SetBool("IsMoving", true);
                    break;
                case FSM_Enemies.Attacking:
                    navAgent.SetDestination(transform.position);
                    attackTime = Time.time + attackDuration;

                    if (_target.TryGetComponent<IDamagable>(out IDamagable IdTarget))
                    {
                        IdTarget.ChangeHealth(-damage,transform.position);
                    }

                    if (anim != null)
                    {
                        anim.SetTrigger("Attack");
                        anim.SetBool("IsMoving", false);
                    }
                    break;
                case FSM_Enemies.TargetLost:
                    navAgent.SetDestination(lastTargetPos);
                    break;
                case FSM_Enemies.Hit:
                    break;
                case FSM_Enemies.Panic:
                    sfxPanic.SetActive(true);
                    break;
                default:
                    break;
            }
        }

        switch (fsm)
        {
            case FSM_Enemies.Idle:

                CheckForTarget();

                if (Time.time >= wanderTime)
                {
                    wanderPos = new Vector3(Random.Range(-wanderRange, wanderRange), 0, Random.Range(-wanderRange, wanderRange)) + spawnPosition;
                    navAgent.SetDestination(wanderPos);
                    fsm = FSM_Enemies.Wander;
                }

                break;
            case FSM_Enemies.Wander:

                CheckForTarget();

                float stoppingRange = 0.2f;
                if (navAgent.remainingDistance <= stoppingRange)
                {
                    fsm = FSM_Enemies.Idle;
                }

                break;
            case FSM_Enemies.Chasing:

                navAgent.SetDestination(_target.transform.position);

                if (Vector3.SqrMagnitude(transform.position - _target.transform.position) < attackRange)
                {
                    fsm = FSM_Enemies.Attacking;
                }

                if (Vector3.Distance(transform.position,_target.transform.position) > detectionRange + 2)
                {
                    lastTargetPos = _target.transform.position;
                    _target = null;
                    fsm = FSM_Enemies.TargetLost;
                }

                break;
            case FSM_Enemies.TargetLost:

                CheckForTarget();

                stoppingRange = 0.2f;

                if (navAgent.remainingDistance <= stoppingRange)
                {
                    fsm = FSM_Enemies.Idle;
                }

                break;
            case FSM_Enemies.Attacking:

                if (attackTime <= Time.time)
                {
                    fsm = FSM_Enemies.Chasing;
                }

                break;
            case FSM_Enemies.Hit:
                break;
            case FSM_Enemies.Panic:

                //Choose Random Dir if timer ended
                if (panicTime <= Time.time)
                {
                    //Go to random Dir
                    Vector3 dir = Random.onUnitSphere;
                    dir.y = 0;

                    navAgent.SetDestination(transform.position + dir * panicRange);

                    //startTimer
                    panicTime = Time.time + timeBetweenDirChange;
                    
                }
                
                //If panic over go back to idle
                if (Time.time >= panicDuration)
                {
                    fsm = FSM_Enemies.Idle;
                    sfxPanic.SetActive(false);
                }
                
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
            fsm = FSM_Enemies.Chasing;
        }
    }

    void Damage(Vector3 origin)
    {
        hp--;
        Vector3 dir = transform.position - origin;
        rb.velocity = dir.normalized * 5f + transform.up * 2.5f;

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Panic(float time)
    {
        panicDuration = Time.time + time;
        fsm = FSM_Enemies.Panic;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
