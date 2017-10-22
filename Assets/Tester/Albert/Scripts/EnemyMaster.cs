using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaster : MonoBehaviour {

    public Transform myTarget;

    public delegate void GeneralEventHandler();
    public event GeneralEventHandler EventEnemyDie;
    public event GeneralEventHandler EventEnemyWalking;
    public event GeneralEventHandler EventEnemyReachedNavTarget;
    public event GeneralEventHandler EventEnemyAttack;
    public event GeneralEventHandler EventEnemyLostTarget;

    public delegate void HealthEventHandler(int Health);
    public event HealthEventHandler EventEnemyDeductHealth;

    public delegate void NavTargetEventHandler(Transform targetTransform);
    public event NavTargetEventHandler EventEnemySetNavTarget;

    public void CallEventEnemyDeductHealth(int health)
    {
        if (EventEnemyDeductHealth != null)
        {
            EventEnemyDeductHealth(health);
        }
    }

    public void CallEventEnemySetNavTarget(Transform targetTransform)
    {
        if(EventEnemyReachedNavTarget != null)
        {
            EventEnemySetNavTarget(targetTransform);
        }

        myTarget = targetTransform;
    }

    public void CallEventEnemyDie()
    {
        if(EventEnemyDie != null)
        {
            EventEnemyDie();
        }
    }

    public void CallEventEnemyWalking()
    {
        if (EventEnemyWalking != null)
        {
            EventEnemyWalking();
        }
    }

    public void CallEventEnemyReachedNavTarget()
    {
        if (EventEnemyReachedNavTarget != null)
        {
            EventEnemyReachedNavTarget();
        }
    }

    public void CallEventEnemyAttack()
    {
        if (EventEnemyAttack != null)
        {
            EventEnemyAttack();
        }
    }

    public void CallEventEnemyLostTarget()
    {
        if (EventEnemyLostTarget != null)
        {
            EventEnemyLostTarget();
        }

        myTarget = null;
    }
}
