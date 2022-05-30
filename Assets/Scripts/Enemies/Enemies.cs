using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Threading.Tasks;
using DG.Tweening;

public class Enemies : FightingObject
{
    public int hp = 3;

    [SerializeField] private Animator anim;
    [SerializeField] private EnemyScriptables enemyType;
    [SerializeField] private GameObject enemyModel;
    [SerializeField] private GameObject puddleModel;
    [SerializeField] private float losingTargetTime;

    [Space] [Header("Wander")]
    [SerializeField] private float wanderRange;
    [SerializeField] private float timeBetweenWander;
    [SerializeField] private Vector3 spawnPosition;
    private float wanderTime;

    [Space] [Header("Turn when detect")]
    [SerializeField] private float turnDelay;
    private float turnVelocity;

    [Space] [Header("Panic")]
    [SerializeField] private float panicRange;
    [SerializeField] private float timeBetweenDirChange;
    [SerializeField] private GameObject sfxPanic;
    private float panicTime;
    private float panicDuration;

    [Space] [Header("Gather")]
    [SerializeField] private Transform[] gatheringPos;
    [SerializeField] private Enemies leader;
    [SerializeField] private float timeToGather;

    [Space] [Header("Fx")]
    [SerializeField] private GameObject puddleFx;
    [SerializeField] private GameObject RespawnFx;
    private Rigidbody rb;
    private NavMeshAgent navAgent;
    private Vector3 wanderPos;
    private Vector3 lastTargetPos;

    public enum FSM_Enemies { Idle, Wander, TargetAcquired, Chasing, Patroling, Attacking, TargetLost, Hit, Panic, Flee,Puddle}
    [SerializeField] public FSM_Enemies fsm;
    private FSM_Enemies fsmOld;

    public System.Action<GameObject> OnKill;

    #region ActionsDelegate
    public delegate void EnemyAction();

    public EnemyAction OnPlayerDetected;
    public EnemyAction OnAttack;
    public EnemyAction OnHit;
    #endregion

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
        
        if(enemyType != null)
        {
            SetUpEnemies(enemyType);
        }
        
        enemyModel.SetActive(true);
        puddleModel.SetActive(false);

        navAgent.speed = enemyType.GetSpeed();
        navAgent.angularSpeed = enemyType.GetTurningSpeed();

        anim = enemyModel.GetComponent<Animator>();

        canAttack = true;
    }

    private void Update()
    {
        
        float stoppingRange = 0.2f;

        if (fsm != fsmOld)
        {
            fsmOld = fsm;

            switch (fsm)
            {
                case FSM_Enemies.Idle:
                    wanderTime = Time.time + Random.Range(0f, 10f);

                    if(anim != null){
                        anim.SetBool("IsMoving", false);
                    }
                    navAgent.SetDestination(transform.position);
                    break;
                case FSM_Enemies.Wander:

                    navAgent.speed = enemyType.GetSpeed() / 3;

                    if (anim != null)
                        anim.SetBool("IsMoving",true);
                    break;
                case FSM_Enemies.TargetAcquired:

                    StartCoroutine(TurnToTargetAndChangeState(0.75f, FSM_Enemies.Chasing));

                    break;
                case FSM_Enemies.Chasing:
                    if (anim != null)
                    {
                        anim.SetBool("IsMoving", true);
                    }

                    navAgent.speed = enemyType.GetSpeed();

                    break;

                case FSM_Enemies.Patroling:
                    navAgent.speed = enemyType.GetSpeed() / 3;
                    navAgent.SetDestination(_target.transform.position);

                    anim.SetBool("IsMoving", true);

                    break;
                    
                case FSM_Enemies.Attacking:
                    navAgent.SetDestination(transform.position);
                    attackTime = Time.time + attackDuration;

                    if (_target.TryGetComponent<IDamagable>(out IDamagable IdTarget) && canAttack)
                    {
                        IdTarget.ChangeHealth(-damage,transform.position);
                        OnAttack?.Invoke();
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
                case FSM_Enemies.Puddle:
                    enemyModel.SetActive(false);
                    puddleModel.SetActive(true);

                    navAgent.speed = enemyType.GetPuddleSpeed();
                    navAgent.angularSpeed = enemyType.GetTurningSpeed();

                    navAgent.SetDestination(spawnPosition);

                    PoolManager.Instance.Spawn(puddleFx,true,transform.position,Quaternion.identity);
                    break;
                default:
                    break;
            }

            if (anim != null)
            {
                if (navAgent.speed == enemyType.GetSpeed())
                {
                    anim.SetBool("IsRunning", true);
                }
                else
                {
                    anim.SetBool("IsRunning", false);
                }
            }
        }

        switch (fsm)
        {
            case FSM_Enemies.Idle:

                CheckForTarget();

                if (Time.time >= wanderTime)
                {
                    wanderPos = new Vector3(UnityEngine.Random.Range(-wanderRange, wanderRange), 0, UnityEngine.Random.Range(-wanderRange, wanderRange)) + spawnPosition;

                    Vector3 wanderPosDir = wanderPos - transform.position;
                    float dotProd = Vector3.Dot(transform.forward, wanderPosDir.normalized);

                    navAgent.SetDestination(wanderPos);

                    fsm = FSM_Enemies.Wander;

                    /*if (dotProd <= 0)
                    {
                        StartCoroutine(TurnTowardTarget(1f, wanderPos));
                        Debug.Log(dotProd);
                    }*/
                }

                break;
            case FSM_Enemies.Wander:

                CheckForTarget();

                if (navAgent.remainingDistance <= stoppingRange)
                {
                    fsm = FSM_Enemies.Idle;
                }

                break;
            case FSM_Enemies.TargetAcquired:

                break;
            case FSM_Enemies.Chasing:

                navAgent.SetDestination(_target.transform.position);

                if (Vector3.SqrMagnitude(transform.position - _target.transform.position) < attackRange && _target.GetComponent<IDamagable>() != null)
                {
                    fsm = FSM_Enemies.Attacking;
                }

                if (Vector3.Distance(transform.position,_target.transform.position) > detectionRange + 4)
                {
                    lastTargetPos = _target.transform.position;
                    _target = null;                 
                    fsm = FSM_Enemies.TargetLost;
                }

                if (navAgent.isStopped)
                {
                    anim.SetBool("IsMoving", false);
                }

                break;
            case FSM_Enemies.Patroling:
                CheckForTarget();
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
                    Vector3 dir = UnityEngine.Random.onUnitSphere;
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
            case FSM_Enemies.Puddle:
                if (Vector3.Distance(transform.position,spawnPosition) <= stoppingRange)
                {
                    StartCoroutine(TemporaryStopMovement(enemyType.GetRespawnTime()));

                    enemyModel.SetActive(true);
                    puddleModel.SetActive(false);

                    //Respawn animation
                    enemyModel.transform.localScale = new Vector3(1,0,1);
                    enemyModel.transform.DOScaleY(1,enemyType.GetRespawnTime());

                    if(RespawnFx != null)
                    {
                        PoolManager.Instance.Spawn(RespawnFx,true,transform.position,Quaternion.identity);
                    }

                    navAgent.speed = enemyType.GetSpeed();

                    fsm = FSM_Enemies.Idle;
                }
                break;
            default:
                break;
        }        
    }

    //Check for Player in detectionRange
    void CheckForTarget()
    {
        if (Physics.CheckSphere(transform.position, detectionRange, hostileLayer) && canAttack)
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, detectionRange, hostileLayer);
            _target = cols[0].gameObject;
            fsm = FSM_Enemies.TargetAcquired;

            OnPlayerDetected?.Invoke();
        }
    }

    public void Damage(Vector3 origin)
    {
        hp--;
        Vector3 dir = transform.position - origin;
        rb.velocity = dir.normalized * 5f + transform.up * 2.5f;

        if (hp <= 0)
        {
            fsm = FSM_Enemies.Puddle;
        }
    }

    public void Panic(float time)
    {
        panicDuration = Time.time + time;
        fsm = FSM_Enemies.Panic;
    }

    public void StopMovement()
    {
        canAttack = false;
        navAgent.speed = 0;
        navAgent.angularSpeed = 0;
        navAgent.SetDestination(transform.position);
        DG.Tweening.DOTween.Kill(transform);

        anim.SetBool("IsMoving", false);
    }

    public void ResumeMovement()
    {
        canAttack = true;
        navAgent.speed = enemyType.GetSpeed();
        navAgent.angularSpeed = enemyType.GetTurningSpeed();

    }

    public IEnumerator TemporaryStopMovement(float time)
    {
        StopMovement();
        yield return new WaitForSeconds(time);
        ResumeMovement();
    }

    public void Kill()
    {
        gameObject.SetActive(false);
        OnKill?.Invoke(gameObject);
        OnHit?.Invoke();
    }

    public void SetUpEnemies(EnemyScriptables scriptable)
    {
        enemyType = scriptable;

        if (enemyModel != null)
        {
            Destroy(enemyModel);
        }

        enemyModel = Instantiate(enemyType.GetModel(),transform);

    }

    private IEnumerator TurnTowardTarget(float duration, Vector3 targetPos)
    {
        StopMovement();

        float endTime = Time.time + duration;
        if (_target != null)
        {
            while (Time.time < endTime)
            {
                yield return new WaitForFixedUpdate();

                Vector3 lookAtTarget = new Vector3(targetPos.x, transform.position.y, targetPos.z);

                Vector3 targetDir = lookAtTarget - transform.position;
                float rotationToTarget = Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg;
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationToTarget, ref turnVelocity, turnDelay);
            }
        }

        ResumeMovement();
    }

    public IEnumerator TurnToTargetAndChangeState(float duration, FSM_Enemies stateAfterTurn)
    {
        float currentSpeed = navAgent.speed;
        yield return TurnTowardTarget(duration, _target.transform.position);
        navAgent.speed = currentSpeed;
        fsm = stateAfterTurn;
    }

    //Set target to current target for other enemies in detection range
    //private void Gather()
    //{
    //    if (Physics.CheckSphere(transform.position, detectionRange, 1 << 9))
    //    {
    //        Collider[] cols = Physics.OverlapSphere(transform.position, detectionRange, 1 << 9);

    //        for (int i = 0; i < cols.Length; i++)
    //        {
    //            Enemies enemy = cols[i].GetComponent<Enemies>();

    //            enemy.SetTarget(_target);
    //        }
    //    }
    //}

    //private void OnCollisionEnter(Collision other) {
    //    if(other.gameObject.TryGetComponent<IDamagable>(out IDamagable player))
    //    {
    //        player.ChangeHealth(-damage,transform.position);
    //        OnAttack?.Invoke();
    //    }
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
