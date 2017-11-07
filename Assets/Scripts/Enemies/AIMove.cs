using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMove : MonoBehaviour {

    private enum EnemyState { Idle, Patrol, Chase, Attack, Stun, Dead }
    [SerializeField]
    private EnemyState state;

    private NavMeshAgent agent;
    [SerializeField]
    private Transform targetTransform;

    [Header("Path")]
    public Transform[] points;
    private int pathIndex = 0;

    [Header("Distances")]
    public float chaseRange;
    public float attackRange;
    [SerializeField]
    private float distanceFromTarget = Mathf.Infinity;

    [Header("Stats")]
    private bool canAttack = false;

    [Header("Properties")]
    public int hitDamage;
    public int life;

    [Header("Timers")]
    public float idleTime = 1.0f;
    [SerializeField]
    private float timeCounter = 0;
    public float coolDownAttack = 0.5f;
    public float stunTime = 1.0f;

    [Header("Sounds")]
    public Animator anim;

    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        targetTransform = GameObject.FindGameObjectWithTag("Player").transform;

        SetIdle();
    }

    // Update is called once per frame
    void Update()
    {
        //agent.SetDestination(targetTransform.position);

        distanceFromTarget = GetDistanceFromTarget();

        switch (state)
        {
            case EnemyState.Idle:
                IdleUpdate();
                break;
            case EnemyState.Patrol:
                PatrolUpdate();
                break;
            case EnemyState.Chase:
                ChaseUpdate();
                break;
            case EnemyState.Attack:
                AttackUpdate();
                break;
            case EnemyState.Stun:
                StunUpdate();
                break;
            case EnemyState.Dead:
                DeadUpdate();
                break;
            default:
                break;
        }
    }

    #region Updates
    void IdleUpdate()
    {
        anim.SetBool("Patrol", false);

        if (timeCounter >= idleTime)
        {
            SetPatrol();
        }
        else timeCounter += Time.deltaTime;
    }
    void PatrolUpdate()
    {
        anim.SetBool("Patrol", true);

        if (distanceFromTarget < chaseRange)
        {
            SetChase();
            return;
        }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            pathIndex++;
            if (pathIndex >= points.Length) pathIndex = 0;

            SetPatrol();
        }
    }
    void ChaseUpdate()
    {
        agent.SetDestination(targetTransform.position);

        if (distanceFromTarget > chaseRange)
        {
            SetPatrol();
            return;
        }
        if (distanceFromTarget < attackRange)
        {
            SetAttack();
            return;
        }
    }
    void AttackUpdate()
    {
        agent.SetDestination(targetTransform.position);
        anim.SetBool("Patrol", false);
        if (canAttack)
        {
            agent.isStopped = true;
            //agent.isStopped = true; // VERSIÓN 5.6
            targetTransform.GetComponent<EnergyBar>().ReceivedDamage(hitDamage);
            if (targetTransform.GetComponent<PlayerBehaviour>().energy <= 0)
            {
                canAttack = false;
                return;
            }
            idleTime = coolDownAttack;
            SetIdle();
            return;
        }

        if (distanceFromTarget > attackRange)
        {
            SetChase();
            return;
        }
    }
    void StunUpdate()
    {
        if (timeCounter >= stunTime)
        {
            idleTime = 0;
            SetIdle();
        }
        else timeCounter += Time.deltaTime;
    }
    void DeadUpdate()
    {
        timeCounter += Time.deltaTime;
        if (timeCounter >= 3.3f)
        {
            this.gameObject.SetActive(false);
        }
    }
    #endregion
    #region Sets
    void SetIdle()
    {
        timeCounter = 0;
        //anim.SetBool("Patrol", false);
        state = EnemyState.Idle;
    }
    void SetPatrol()
    {
        //agent.SetDestination();
        agent.isStopped = false;
        agent.SetDestination(points[pathIndex].position);
        //anim.SetBool("Patrol", true);
        state = EnemyState.Patrol;
    }
    void SetChase()
    {
        // Feedback de que empieza a perseguirnos
        state = EnemyState.Chase;
    }
    void SetAttack()
    {
        anim.SetTrigger("Attack");
        state = EnemyState.Attack;
    }
    void SetStun()
    {
        agent.isStopped = true;
        anim.SetTrigger("HitEne");
        state = EnemyState.Stun;
    }
    void SetDead()
    {
        agent.isStopped = true;
        anim.SetTrigger("Dead");
        state = EnemyState.Dead;
        //this.gameObject.SetActive(false);
    }
    #endregion

    #region public Functions
    public void SetDamage(int hit)
    {
        life -= hit;

        if (life <= 0)
        {
            SetDead();
            return; // lo q viene a continuacuin no se ejecuta
        }
        SetStun();
    }
    #endregion

    float GetDistanceFromTarget()
    {
        return Vector3.Distance(targetTransform.position, transform.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            canAttack = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            canAttack = false;
        }
    }

    void OnDrawGizmos()
    {
        Color newColor = Color.yellow;
        newColor.a = 0.2f;
        Gizmos.color = newColor;
        Gizmos.DrawSphere(transform.position, chaseRange);
        newColor = Color.red;
        newColor.a = 0.15f;
        Gizmos.color = newColor;
        Gizmos.DrawSphere(transform.position, attackRange);
    }
}
