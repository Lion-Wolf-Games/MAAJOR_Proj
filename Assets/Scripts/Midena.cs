using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class Midena : FightingObject
{
    public MidenaSate state,oldState;
    [SerializeField] private float travelSpeed;
    [SerializeField] private Animator anim;

    private Vector3 startPos;
    [SerializeField] private SphereCollider detectionTrigger;
    [SerializeField] private float focusTime = 1f;
    [SerializeField] private bool isFocusing;
    private float focusTimer;
    bool isAttacking;

    [SerializeField] private Transform visual;

    [SerializeField] private List<Enemies> enemiesInRange;

    [SerializeField] private AK.Wwise.Event onAttack;

    private NavMeshAgent agent;

    private void Start() {
        startPos = transform.position;
        enemiesInRange = new List<Enemies>();
        agent = GetComponent<NavMeshAgent>();
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
            break;
            case MidenaSate.Move:
            break;
            default:
            break;
        }

        if(isFocusing && !isAttacking)
        {
            focusTimer += Time.deltaTime;
            if(focusTimer >= focusTime)
            {
                isFocusing = false;
                focusTimer = 0;

                // GameObject enemy = GetClosestEnemy(visual.position);
                // AttackTarget(enemy);
                StartCoroutine(Attacking());
                isAttacking = true;

            }
        }

        if(agent != null)
        {
            float speedMag = agent.velocity.magnitude / agent.speed;
            float movDirX = Vector3.Dot(transform.right,agent.velocity.normalized);
            float movDirY = Vector3.Dot(transform.forward,agent.velocity.normalized);
            anim.SetFloat("DirX",movDirX);
            anim.SetFloat("DirY",movDirY);
            anim.SetFloat("Speed",speedMag);
        }
    }

    private GameObject GetClosestEnemy(Vector3 pos)
    {
        float minDist = float.MaxValue;
        GameObject closest = null;

        for (int i = 0; i < enemiesInRange.Count; i++)
        {
            float dist = Vector3.Distance(enemiesInRange[i].transform.position,pos);

            if(dist < minDist)
            {
                minDist = dist;
                closest = enemiesInRange[i].gameObject;
            }
        }

        return closest;
    }

    void AttackTarget(GameObject target)
    {
        Vector3 dir = (target.transform.position - transform.position) * 0.8f ;

        visual.DOMove(transform.position + dir,travelSpeed).OnComplete(() => {Attack();});
        visual.LookAt(target.transform );

        //_target.GetComponent<Enemies>().StopMovement();

        anim.SetTrigger("Attack");
        anim.SetInteger("AttackType",Random.Range(1 ,6));
    }

    public void Attack()
    {
        // if(_target != null)
        // {
        //     _target.GetComponent<Enemies>().Damage(transform.position);
        //     _target = null;
        // }

        //Get all enemies in range
        Collider[] cols = Physics.OverlapSphere(visual.position,attackRange,hostileLayer);

        //Damage all enemies
        foreach(var col in cols)
        {
            if(col.TryGetComponent<Enemies>(out Enemies enm))
            {
                enm.Damage(visual.position);

                //Remove them from list to not damage them twice
                if(enemiesInRange.Contains(enm))
                {
                    enemiesInRange.Remove(enm);
                }
            }
        }

    }

    private IEnumerator Attacking()
    {
        while(enemiesInRange.Count > 0)
        {
            GameObject closest = GetClosestEnemy(visual.position);
            AttackTarget(closest);
            yield return new WaitForSeconds(attackDuration);
        }

        isAttacking = false;

        visual.DOMove(transform.position,travelSpeed);
        visual.LookAt(transform);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(visual.position + visual.forward,attackRange);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.TryGetComponent<Enemies>(out Enemies en))
        {
            enemiesInRange.Add(en);
            if(!isFocusing)
            {
                isFocusing = true;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.TryGetComponent<Enemies>(out Enemies en))
        {
            if(enemiesInRange.Contains(en))
            {
                enemiesInRange.Remove(en);

                if (isFocusing && enemiesInRange.Count == 0)
                {
                    isFocusing = false;
                    focusTimer = 0;
                }
            }
        }
    }

    public void MoveToTarget(Transform target)
    {
        if (agent != null)
        {
            agent.SetDestination(target.position);
        }

        visual.forward = transform.forward;
    }
    
    public void LookAtTarget(Transform target)
    {
        Vector3 looktarget = new Vector3(target.position.x,transform.position.y,target.position.z);
        transform.DOLookAt(looktarget,0.5f).SetEase(Ease.InOutQuad);
    }

    private void OnValidate() {
        if(detectionTrigger != null)
        {
            detectionTrigger.radius = detectionRange;
        }
    }

}

public enum MidenaSate
{
    Idle,
    Move,
    Attack
}
