using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMage : MonoBehaviour {
    SpawnManager spawn;
    Animator anim;
    StatePatternEnemy unitedStatePattern;
    float timer = 5;
    bool RaisedDead = false;
    bool deadHaveBeenRaised = false;
	// Use this for initialization
	void Start ()
    {
        spawn = GetComponent<SpawnManager>();
        unitedStatePattern = GetComponent<StatePatternEnemy>();
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        timer -= Time.deltaTime;
        if (unitedStatePattern.currentState.ToString() == "PatrolState" && !RaisedDead)
        {
            CancelCurrentAnimation();
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetTrigger("Raise Dead");
        }
        if (unitedStatePattern.currentState.ToString() == "AlertState")
        {
            CancelCurrentAnimation();
            anim.SetBool("Chanting", true);
        }
        if (unitedStatePattern.currentState.ToString() == "ChaseState") //&& unitedStatePattern.DistanceToPlayer > stopToAttackDistance)
        {
            CancelCurrentAnimation();
            if (unitedStatePattern.navMeshAgent.remainingDistance < unitedStatePattern.attackDistance)
            {
                unitedStatePattern.navMeshAgent.Stop();
                anim.SetBool("Run", false);
                anim.SetTrigger("Projectile Attack");
            }
            else
            {
                unitedStatePattern.navMeshAgent.Resume();
                anim.ResetTrigger("Projectile Attack");
                anim.SetBool("Run", true);
            }
        }
        if (spawn.EnemiesHaveSpawned && !RaisedDead && !deadHaveBeenRaised)
        {
            RaisedDead = true;
            unitedStatePattern.navMeshAgent.speed = 0;
            CancelCurrentAnimation();
            anim.SetTrigger("Raise Dead");
            //transform.LookAt(unitedStatePattern.chaseTarget.transform.position);
        }

        if (timer <= 0)
        {
            spawn.EnemiesHaveSpawned = false;
            timer = 5;
        }
	}

    void CancelCurrentAnimation()
    {
        //if (unitedStatePattern.currentState.ToString() != "PatrolState")
        //{
            anim.SetBool("Walk", false);
        //}
        if (unitedStatePattern.currentState.ToString() != "AlertState")
        {
            anim.SetBool("Chanting", false);
        }
        if (unitedStatePattern.currentState.ToString() != "ChaseState") //|| unitedStatePattern.DistanceToPlayer < stopToAttackDistance)
        {
            anim.SetBool("Run", false);
        }
    }


}
