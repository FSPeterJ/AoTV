using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonKnight : MonoBehaviour{

    public GameObject sword;
    StatePatternEnemy unitedStatePattern;
    IWeaponBehavior weaponBehavior;
    Animator anim;
    bool asleep = true;
    bool attacking = false;
    bool dead = false;

    // Use this for initialization
	void Start ()
    {
        unitedStatePattern = GetComponent<StatePatternEnemy>();
        anim = GetComponent<Animator>();
        //StartCoroutine(WakeMeUpInside());
        weaponBehavior = sword.GetComponent<IWeaponBehavior>();
        asleep = false;
    }

    // Update is called once per frame
    void Update ()
    {
        if (!asleep && unitedStatePattern.alive)
        {
            if (unitedStatePattern.currentState.ToString() == "PatrolState")
            {
                CancelCurrentAnimation();
                anim.SetBool("Walk", true);
            }
            if (unitedStatePattern.currentState.ToString() == "AlertState")
            {
                CancelCurrentAnimation();
                anim.SetBool("Defend", true);
            }
            if (unitedStatePattern.currentState.ToString() == "ChaseState") //&& unitedStatePattern.DistanceToPlayer > stopToAttackDistance)
            {
                CancelCurrentAnimation();
                if(unitedStatePattern.navMeshAgent.remainingDistance < unitedStatePattern.attackDistance)
                {
                    unitedStatePattern.navMeshAgent.Stop();
                    anim.SetBool("Run", false);
                    anim.SetTrigger("Double Attack");
                }
                else
                {
                    unitedStatePattern.navMeshAgent.Resume();
                    anim.ResetTrigger("Double Attack");
                    anim.SetBool("Run", true);
                }
            }
        }
        else
        {
            CancelCurrentAnimation();
        }
    }

    void BeginAttacking()
    {
        weaponBehavior.AttackStart();
    }

    void EndAttacking()
    {
        weaponBehavior.AttackEnd();
    }

    void CancelCurrentAnimation()
    {
        if (unitedStatePattern.currentState.ToString() != "PatrolState")
        {
            anim.SetBool("Walk", false);
        }
        if (unitedStatePattern.currentState.ToString() != "AlertState")
        {
            anim.SetBool("Defend", false);
        }
        if (unitedStatePattern.currentState.ToString() != "ChaseState") //|| unitedStatePattern.DistanceToPlayer < stopToAttackDistance)
        {
            anim.SetBool("Run", false);
        }
    }

    IEnumerator WakeMeUpInside()
    {
        while (asleep)
        {
            unitedStatePattern.navMeshAgent.Stop();
            anim.SetBool("Die", true);
            yield return new WaitForSeconds(5);
        }
    }

}
