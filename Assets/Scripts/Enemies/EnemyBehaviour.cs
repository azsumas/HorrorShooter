﻿using System.Collections;
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
    public GameObject target;
    [Header("Patch")]
    public Transform[] points;
    private int pathIndex = 0;
    [Header("Distance")]
    public float chaseRange;
    public float attackRange;
    private float distanceFromTarget = Mathf.Infinity;
    [Header("Sneak")]
    private Vector3 investigateSpot;
    private float timer = 0;
    public float investigateWait = 10;
    public float heightMultiplier;
    public float sightDist = 10;

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
        agent = GetComponent<NavMeshAgent>();
        targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
        SetIdle();

        heightMultiplier = 1.36f;
    }

    void Update()
    {
        //agent.SetDestination(targetTransform.position);
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
            case EnemyState.Investigate:
                InvestigateUpdate();
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
        if (distanceFromTarget > chaseRange)
        {
            SetPatrol();
            return;
        }
        if(distanceFromTarget < attackRange)
        {
            SetAttack();
            return;
        }
    }
    void InvestigateUpdate()
    {
        timer += Time.deltaTime;
        agent.SetDestination(this.transform.position);
        transform.LookAt(investigateSpot);
        if (timer >= investigateWait)
        {
            SetPatrol();
            timer = 0;
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
    }
    void FixedUpdate()
    {
        RaycastHit rayHit;
        Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, transform.forward * sightDist, Color.green);
        Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward + transform.right).normalized * sightDist, Color.green);
        Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward - transform.right).normalized * sightDist, Color.green);
        if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, transform.forward, out rayHit, sightDist))
        {
            if (rayHit.collider.gameObject.tag == "Player")
            {
                SetChase();
                target = rayHit.collider.gameObject;
            }
        }
        if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, (transform.forward + transform.right).normalized, out rayHit, sightDist))
        {
            if (rayHit.collider.gameObject.tag == "Player")
            {
                SetChase();
                target = rayHit.collider.gameObject;
            }
        }
        if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, (transform.forward - transform.right).normalized, out rayHit, sightDist))
        {
            if (rayHit.collider.gameObject.tag == "Player")
            {
                SetChase();
                target = rayHit.collider.gameObject;
            }
        }
    }
}