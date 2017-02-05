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
