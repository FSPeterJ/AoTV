using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon_Mike : MonoBehaviour {

    public GameObject sword;
    StatePatternEnemy unitedStatePattern;
    IWeaponBehavior weaponBehavior;
    Animator anim;
    bool asleep = true;
    public uint pointValue = 1;

    // Use this for initialization
    void Start()
    {
        gameObject.transform.parent = null;
        unitedStatePattern = GetComponent<StatePatternEnemy>();
        anim = GetComponent<Animator>();
        //StartCoroutine(WakeMeUpInside());
        weaponBehavior = sword.GetComponent<IWeaponBehavior>();
        asleep = false;
        anim.SetBool("Fly Idle", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!asleep && unitedStatePattern.alive)
        {
            if (unitedStatePattern.currentState.ToString() == "PatrolState")
            {
                //CancelCurrentAnimation();
                anim.SetBool("Fly Forward", true);
            }
            if (unitedStatePattern.currentState.ToString() == "AlertState")
            {
                //CancelCurrentAnimation();
                anim.SetTrigger("Fly Fire Breath Attack");
            }
            if (unitedStatePattern.currentState.ToString() == "ChaseState") //&& unitedStatePattern.DistanceToPlayer > stopToAttackDistance)
            {
                //CancelCurrentAnimation();
                anim.SetLookAtPosition(unitedStatePattern.chaseTarget.position);
                if (unitedStatePattern.navMeshAgent.remainingDistance < unitedStatePattern.attackDistance)
                {
                    unitedStatePattern.navMeshAgent.Stop();
                    anim.SetBool("Fly Forward", false);
                    anim.SetTrigger("Fly Bite Attack");
                }
                else
                {
                    unitedStatePattern.navMeshAgent.Resume();
                    anim.ResetTrigger("Fly Bite Attack");
                    anim.SetBool("Fly Forward", true);
                }
            }
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
}
