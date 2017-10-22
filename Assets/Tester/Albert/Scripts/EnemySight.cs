using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;  //Libreria de unity para Random

public class EnemySight : MonoBehaviour {

    public EnemyMaster enemyMaster;  //Gestor de eventos
    public Transform myTransform;   //Este transform
    public Transform head;  //cabeza objeto
    public LayerMask playerLayer;       //LocalizarPlayer
    public LayerMask sightLayer;        
    public float checkRate; //Ratio de buscar, tiempo.
    public float nextCheck; // prox chequeo.
    public float detectRadius = 10; //Area de detección
    private RaycastHit hit; //Vision

    void OnEnable()
    {
        SetInitialReferences();
        enemyMaster.EventEnemyDie += DisableThis;
    }

    void OnDisable()
    {
        enemyMaster.EventEnemyDie -= DisableThis;
    }

    void Update()
    {
        CarryOutDetection();
    }

    void SetInitialReferences()
    {
        enemyMaster = GetComponent<EnemyMaster>();
        myTransform = transform;
        if(head == null)
        {
            head = myTransform;
        }


        checkRate = Random.Range(0.8f, 1.2f);
    }

    private void CarryOutDetection()
    {
        if(Time.time > nextCheck)
        {
            nextCheck = Time.time + checkRate;
            Collider[] colliders = Physics.OverlapSphere(myTransform.position, detectRadius, playerLayer);

            if(colliders.Length > 0)
            {
                foreach(Collider potentialTargetCollider in colliders)
                {
                    if(potentialTargetCollider.CompareTag("Player"))
                    {
                        if (CanPotentialTargetBeSeen(potentialTargetCollider.transform))
                        {
                            break;
                        }
                    }
                }
            }

            else
            {
                enemyMaster.CallEventEnemyLostTarget();
            }
        }
    }

    bool CanPotentialTargetBeSeen(Transform potentialTarget)
    {
        if(Physics.Linecast(head.position, potentialTarget.position, out hit, sightLayer))
        {
            if(hit.transform == potentialTarget)
            {
                enemyMaster.CallEventEnemySetNavTarget(potentialTarget);
                return true;
            }
            else
            {
                enemyMaster.CallEventEnemyLostTarget();
                return false;
            }
        }
        else
        {
            enemyMaster.CallEventEnemyLostTarget();
            return true;
        }
    }
    void DisableThis()
    {
        this.enabled = false;
    }
}
