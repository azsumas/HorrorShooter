using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FasterEnemyBehaviour : MonoBehaviour
{

    // Use this for initialization

    public enum EnemyState { Idle, Patrol, Chase, Investigate, Attack, Stun, Dead }
    public EnemyState state;
    //public EnemiesBar energyBar;
    public Animator anim;
    private NavMeshAgent agent;
    public Transform targetTransform;

    [Header("Patch")]
    public Transform[] points;
    private int pathIndex = 0;
    [Header("Distance")]
    public float chaseRange;
    public float maxChaseRange;
    public float minChaseRange;
    public float attackRange;
    private float distanceFromTarget = Mathf.Infinity;

    [Header("Timers")]
    public float idleTime = 1;
    public float stunTime = 1;
    private float timeCounter = 0;
    public float coolDownAttack = 1;

    [Header("Stats")]
    [SerializeField]
    private bool canAttack = false;

    [Header("Properties")]
    public int hitDamage;
    public float energy;
    public float maxEnergy;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
        SetIdle();

    }

    void Update()
    {
        distanceFromTarget = GetDistanceFromTarget();
        if (distanceFromTarget < attackRange)
        {
            transform.LookAt(targetTransform);
        }
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
        if (timeCounter >= idleTime)
        {
            SetPatrol();
        }
        else timeCounter += Time.deltaTime;

    }
    void PatrolUpdate()
    {
        agent.speed = 1f;
        chaseRange = chaseRange - minChaseRange;
        if (chaseRange <= minChaseRange)
        {
            chaseRange = minChaseRange;
        }
        if (CanListenPlayer())
        {
            Debug.Log("te oigo");
            SetChase();
            return;
        }
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
        agent.speed = 10f;
        agent.SetDestination(targetTransform.position);

        chaseRange = chaseRange + maxChaseRange;
        if (chaseRange >= maxChaseRange)
        {
            Debug.Log("Te he visto");
            chaseRange = maxChaseRange;
        }
       
        if (distanceFromTarget > chaseRange)
        {
            SetPatrol();
            return;
        }
        else if (distanceFromTarget < attackRange)
        {
            SetAttack();
            return;
        }
        else if (canAttack)
        {
            SetAttack();
            return;
        }
    }

    void AttackUpdate()
    {
        agent.speed = 1f;
        agent.SetDestination(targetTransform.position);

        Debug.Log("ATTACKRANGE");
        chaseRange = chaseRange - minChaseRange;
        if (chaseRange <= minChaseRange)
        {
            chaseRange = minChaseRange;
        }
        if (canAttack)
        {
            agent.isStopped = true;
            targetTransform.GetComponent<PlayerBehaviour>().ReceivedDamage(hitDamage);
            idleTime = coolDownAttack;
            SetIdle();
            Debug.Log("EnemyHitting");

        }

        if (distanceFromTarget > attackRange)
        {
            agent.isStopped = false;
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
        Destroy(this.gameObject, 0.5f);
    }
    #endregion
    #region Sets
    void SetIdle()
    {
        timeCounter = 0;
        state = EnemyState.Idle;
    }
    void SetPatrol()
    {
        agent.isStopped = false;
        agent.SetDestination(points[pathIndex].position);
        state = EnemyState.Patrol;
    }
    void SetChase()
    {
        //Feedback de lo que empieza a perseguirnos :D
        state = EnemyState.Chase;
        anim.SetTrigger("Chase");

    }
    void SetAttack()
    {
        anim.SetTrigger("Attack");
        state = EnemyState.Attack;
    }
    void SetStun()
    {
        agent.isStopped = true;

        //Feedback animations, sound...
        anim.SetTrigger("Stun");
        state = EnemyState.Stun;

    }
    void SetDead()
    {
        agent.isStopped = true;
        this.gameObject.SetActive(false);
        state = EnemyState.Dead;
        //Destroy(this.gameObject);
    }
    #endregion
    #region Public Functions
    public void SetDamage(int hit)
    {
        SetStun();
        energy -= hit;
        //energyBar.UpdateEnergyUI();

        if (energy <= 0)
        {
            SetDead();
            return;
        }

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
    bool CanListenPlayer()
    {
        PlayerBehaviour player = targetTransform.GetComponent<PlayerBehaviour>();
        if(player.speed > 3)
        {
            return true;
        }
        return false;
    }

    void OnDrawGizmos()
    {
        Color newColor = Color.yellow;
        newColor.a = 0.15f;
        Gizmos.color = newColor;
        Gizmos.DrawSphere(transform.position, chaseRange);
        newColor = Color.red;
        newColor.a = 0.15f;
        Gizmos.color = newColor;
        Gizmos.DrawSphere(transform.position, attackRange);


        Gizmos.color = Color.red;
    }
    void FixedUpdate()
    {

    }
}