using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyBehaviour : MonoBehaviour
{

    // Use this for initialization

    public enum EnemyState { Idle, Patrol, Chase, Investigate, Attack, Stun, Dead}
    public EnemyState state;
    public Animator anim;
    private NavMeshAgent agent;
    public Transform targetTransform;

    [Header("Patch")]
    public Transform[] points;
    private int pathIndex = 0;
    [Header("Distance")]
    public float chaseRange;
    public float attackRange;
    private float distanceFromTarget = Mathf.Infinity;
    [Header("Sneak")]
    public Light spotLight;
    public float viewDistance;
    float viewAngle;
    public LayerMask viewMask;
    Color originalSpotLightColor;

    [Header("Timers")]
    public float idleTime = 1;
    public float stunTime = 1;
    private float timeCounter = 0;
    public float coolDownAttack = 0.5f;

    [Header("Stats")]
    [SerializeField] private bool canAttack = false;

    [Header("Properties")]
    public int hitDamage;
    public int life;

    void Start()
    {
        viewAngle = spotLight.spotAngle;
        originalSpotLightColor = spotLight.color;
        agent = GetComponent<NavMeshAgent>();
        targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
        SetIdle();

    }

    void Update()
    {
       
        distanceFromTarget = GetDistanceFromTarget();
        
        switch(state)
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
        if(timeCounter >= idleTime)
        {
            SetPatrol();
        }
        else timeCounter += Time.deltaTime;
    }
    void PatrolUpdate()
    {
		if(CanSeePlayer())
		{
			spotLight.color = Color.red;
			SetChase ();
			return;
		}
		else
		{
			spotLight.color = Color.green;
		}
		if (distanceFromTarget < chaseRange)
        {
            SetChase();
            return;
        }


        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            pathIndex++;
            if(pathIndex >= points.Length) pathIndex = 0;

            SetPatrol();
        }
    }
    void ChaseUpdate()
    {
        agent.SetDestination(targetTransform.position);
        /*if (distanceFromTarget > chaseRange)
        {
            SetPatrol();
            return;
        }*/
        if(distanceFromTarget < attackRange)
        {
            SetAttack();
            return;
        }
    }

    void AttackUpdate()
    {
        agent.SetDestination(targetTransform.position);

        if(canAttack)
        {
            agent.isStopped = true;
            targetTransform.GetComponent<EnergyBar>().ReceivedDamage(hitDamage);
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
        if(timeCounter >= stunTime)
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
        state = EnemyState.Stun;
    }
    void SetDead()
    {
        agent.isStopped = true;
        state = EnemyState.Dead;

        this.gameObject.SetActive(false);
        //Destroy(this.gameObject);
    }
    #endregion
    #region Public Functions
    public void SetDamage(int hit)
    {
        SetStun();
        life -= hit;

        if(life <= 0)
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
        if(other.tag == "Player")
        {
           
            canAttack = true;

        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            
            canAttack = false;
        }
    }

    bool CanSeePlayer()
    {
        if(Vector3.Distance(transform.position, targetTransform.position) < viewDistance)
        {
            Vector3 dirToPlayer = (targetTransform.position - transform.position).normalized;
            float angleBetweenEnemyAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            if(angleBetweenEnemyAndPlayer < viewAngle / 2f)
            {
                if(!Physics.Linecast(transform.position, targetTransform.position, viewMask))
                {
                    return true;
                }
            }
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
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
    }
    void FixedUpdate()
    {
        
    }
}