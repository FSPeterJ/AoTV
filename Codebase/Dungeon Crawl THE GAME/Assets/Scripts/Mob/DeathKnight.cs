using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathKnight : MonoBehaviour {

    public GameObject sword;
    StatePatternEnemy unitedStatePattern;
    IWeaponBehavior weaponBehavior;
    Animator anim;
    bool asleep = true;
    //bool attacking = false;
    //bool dead = false;
    public uint pointValue = 1;


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
    void Update()
    {
        if (!asleep && unitedStatePattern.alive)
        {
            if (unitedStatePattern.currentState.ToString() == "PatrolState")
            {
                CancelCurrentAnimation();
                anim.SetTrigger("Walk");
            }
            if (unitedStatePattern.currentState.ToString() == "AlertState")
            {
                CancelCurrentAnimation();
                anim.SetTrigger("Defend");
            }
            if (unitedStatePattern.currentState.ToString() == "ChaseState") //&& unitedStatePattern.DistanceToPlayer > stopToAttackDistance)
            {
                if (unitedStatePattern.navMeshAgent.remainingDistance < unitedStatePattern.attackDistance)
                {
                    anim.ResetTrigger("Run");
                    anim.SetTrigger("Double Attack");
                    unitedStatePattern.navMeshAgent.Stop();

                }
                else
                {
                    anim.ResetTrigger("Double Attack");
                    anim.SetTrigger("Run");
                    unitedStatePattern.navMeshAgent.Resume();

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
            anim.ResetTrigger("Walk");
        }
        if (unitedStatePattern.currentState.ToString() != "AlertState")
        {
            anim.ResetTrigger("Defend");
        }
        if (unitedStatePattern.currentState.ToString() != "ChaseState") //|| unitedStatePattern.DistanceToPlayer < stopToAttackDistance)
        {
            anim.ResetTrigger("Run");
        }
    }
}
