using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MidenaAttack : MonoBehaviour
{
    
    public Animator animator;
    
    public LayerMask enemyLayer;
    public float detectionRange;
    public float attackRange;

    public Transform targetedEnemy;
    public bool hasATarget;
    public bool isMoving;

    public float wanderingCooldown;
    public float attackCooldown;
    public float movingSpeed;
    public int nbOfattack;

    private float attackTime;
    private float wanderingTime;
    [SerializeField] private Vector3 wanderingDir;

    private int attackCount;

    public enum FSM_Midena { Idle,Wandering,Running, Atacking}

    public FSM_Midena fsm;
    private FSM_Midena fsmOld;

    // Start is called before the first frame update
    void Start()
    {
        fsm = FSM_Midena.Idle;
        wanderingDir = Vector3.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (fsm != fsmOld)
        {
            fsmOld = fsm;

            switch (fsm)
            {
                case FSM_Midena.Idle:
                    animator.SetBool("IsMoving", false);
                    break;
                case FSM_Midena.Wandering:
                    animator.SetBool("IsMoving", true);
                    break;
                case FSM_Midena.Running:
                    animator.SetBool("IsMoving", true);
                    break;
                case FSM_Midena.Atacking:
                    animator.SetBool("IsMoving", false);
                    break;
                default:
                    break;
            }
        }

        switch (fsm)
        {
            case FSM_Midena.Idle:
                //check for new target
                GetNewTarget();
                break;
            case FSM_Midena.Wandering:
                //check for new target
                GetNewTarget();
                //wander around
                if (Time.time >= wanderingTime)
                {
                    wanderingDir.x = Random.Range(-1, 1f);
                    wanderingDir.z = Random.Range(-1, 1f);

                    wanderingDir.Normalize();
                    wanderingTime = Time.time + wanderingCooldown + Random.Range(0, 2f);
                }

                transform.position += wanderingDir.normalized * movingSpeed * Time.deltaTime;
                transform.LookAt(transform.position + wanderingDir);

                break;
            case FSM_Midena.Running:

                if (targetedEnemy == null)
                {
                    fsm = FSM_Midena.Idle;
                    break;
                }

                //Run from point A to B
                Vector3 moveDir = targetedEnemy.position - transform.position;
                moveDir.y = 0;

                transform.LookAt(transform.position + moveDir, transform.up);

                if (moveDir.sqrMagnitude > attackRange * attackRange)
                {
                    transform.position += moveDir.normalized * movingSpeed * Time.deltaTime;
                }
                else if (Time.time >= attackTime)
                {
                    fsm = FSM_Midena.Atacking;
                }
                break;
            case FSM_Midena.Atacking:
                
                //Check if enemy is in range
                if ((transform.position - targetedEnemy.position).sqrMagnitude <= attackRange * attackRange)
                {
                    //Attack enemy
                    if (Time.time >= attackTime)
                    {
                        animator.SetTrigger("Attack");
                        attackTime = Time.time + attackCooldown; 
                    }
                }
                break;
            default:
                break;
        }

    //    if (hasATarget && Time.time >= attackTime)
    //    {
    //        Vector3 pos = targetedEnemy.transform.position;
    //        pos.y = transform.position.y;


    //        if (Vector3.Distance(transform.position,targetedEnemy.transform.position) > attackRange)
    //        {
    //            if (!isMoving)
    //            {
    //                isMoving = true;
    //                Vector3 targetpos = targetedEnemy.transform.position;
    //                targetpos.y = 0;
    //                Vector3 FinalPos = Vector3.Lerp(transform.position,targetpos , 0.9f);

    //                float speed = Vector3.Distance(FinalPos, transform.position) / (movingSpeed > 0 ? movingSpeed : 1);

    //                transform.DOMove(FinalPos, speed).SetEase(Ease.Linear).onComplete = () => { isMoving = false; };
    //            }
    //        }
    //        else
    //        {
                
                
    //            animator.SetTrigger("Attack");

    //        }
    //    }
    //    else
    //    {
    //        GetNewTarget();
    //    }

    //    animator.SetBool("IsMoving", isMoving);
    }

    public void Contact()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position + transform.forward, 1.5f, enemyLayer);


        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].SendMessage("Damage", transform.position, SendMessageOptions.DontRequireReceiver);
        }

        //GetNewTarget();

    }

    void GetNewTarget()
    {
        
        if (Physics.CheckSphere(transform.position,detectionRange,enemyLayer))
        {
            hasATarget = true;

            Collider[] cols = Physics.OverlapSphere(transform.position, detectionRange, enemyLayer);

            float minDistance = detectionRange;
            int closestId = 0;

            for (int i = 0; i < cols.Length; i++)
            {
                float dist = Vector3.Distance(cols[i].transform.position, transform.position);
                if ( dist <= minDistance)
                {
                    minDistance = dist;
                    closestId = i;
                }
            }

            targetedEnemy = cols[closestId].transform;
            fsm = FSM_Midena.Running;
        }
        else
        {
            hasATarget = false;
            targetedEnemy = null;
            fsm = FSM_Midena.Wandering;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
