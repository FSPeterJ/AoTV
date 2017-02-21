using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour {

    public GameObject sword;
    StatePatternEnemy unitedStatePattern;
    IWeaponBehavior weaponBehavior;
    Animator anim;
    bool asleep = true;
    public int pointValue = 1;
    public GameObject[] Slimes;
    public Transform[] Spawns;
    // Use this for initialization
    void Start()
    {
        gameObject.transform.parent = null;
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
                //CancelCurrentAnimation();
                anim.SetBool("Move Forward", true);
            }
            if (unitedStatePattern.currentState.ToString() == "AlertState")
            {
                //CancelCurrentAnimation();
                anim.SetBool("Defend", true);
            }
            if (unitedStatePattern.currentState.ToString() == "ChaseState") //&& unitedStatePattern.DistanceToPlayer > stopToAttackDistance)
            {
                //CancelCurrentAnimation();
                anim.SetLookAtPosition(unitedStatePattern.chaseTarget.position);
                if (unitedStatePattern.navMeshAgent.remainingDistance < unitedStatePattern.attackDistance)
                {
                    unitedStatePattern.navMeshAgent.Stop();
                    anim.SetBool("Move Forward", false);
                    anim.SetTrigger("Melee Attack");
                }
                else
                {
                    unitedStatePattern.navMeshAgent.Resume();
                    anim.ResetTrigger("Melee Attack");
                    anim.SetBool("Move Forward", true);
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
        //if (unitedStatePattern.currentState.ToString() != "PatrolState")
        //{
        //    anim.SetBool("Move Forward", false);
        //}
        //if (unitedStatePattern.currentState.ToString() != "AlertState")
        //{
        //    anim.SetBool("Defend", false);
        //}
        //if (unitedStatePattern.currentState.ToString() != "ChaseState") //|| unitedStatePattern.DistanceToPlayer < stopToAttackDistance)
        //{
        //    anim.SetBool("Run", false);
        //}
    }

    public void Spawn()
    {
        for (int i = 0; i < Slimes.Length; i++)
        {
            Instantiate(Slimes[i], Spawns[i], true);
        }
    }

}
