using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour {

    private EnemyMaster enemyMaster;
    private Animator myAnimator;

    void OnEnable()
    {
        SetInitialReferences();
        enemyMaster.EventEnemyDie += DisableAnimator;
        enemyMaster.EventEnemyWalking += SetAnimationToWalk;
        enemyMaster.EventEnemyWalking += SetAnimationToWalk;
        enemyMaster.EventEnemyReachedNavTarget += SetAnimationToIdle;
        enemyMaster.EventEnemyAttack += SetAnimationToAttack;
        enemyMaster.EventEnemyDeductHealth += SetAnimationToStruck;
    }

    void OnDisable()
    {
        enemyMaster.EventEnemyDie -= DisableAnimator;
        enemyMaster.EventEnemyWalking -= SetAnimationToWalk;
        enemyMaster.EventEnemyReachedNavTarget -= SetAnimationToIdle;
        enemyMaster.EventEnemyAttack -= SetAnimationToAttack;
        enemyMaster.EventEnemyDeductHealth -= SetAnimationToStruck;


    }
    void SetInitialReferences()
    {
        enemyMaster = GetComponent<EnemyMaster>();

        if(GetComponent<Animator>() != null)
        {
            myAnimator = GetComponent<Animator>();
        }
    }

    void SetAnimationToWalk()
    {
        if(myAnimator != null)
        {
            if (myAnimator.enabled)
            {
                myAnimator.SetBool("IsPursuing", true);
            }
        }
    }

    void SetAnimationToIdle()
    {
        if (myAnimator != null)
        {
            if (myAnimator.enabled)
            {
                myAnimator.SetBool("IsPursuing", false);
            }
        }
    }

    void SetAnimationToAttack()
    {
        if (myAnimator != null)
        {
            if (myAnimator.enabled)
            {
                myAnimator.SetTrigger("Attack");
            }
        }
    }

    void SetAnimationToStruck(int dummy)
    {
        if (myAnimator != null)
        {
            if (myAnimator.enabled)
            {
                myAnimator.SetTrigger("Struck");
            }
        }
    }

    void DisableAnimator()
    {
        if(myAnimator != null)
        {
            myAnimator.enabled = false;
        }
    }
}
