using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonKnight : MonoBehaviour{

    Animator anim;
    bool asleep = true;
    StatePatternEnemy unitedStatePattern;
    bool attacking = false;
    float attackDistance = 1.5f;
    int health = 10;
    // Use this for initialization
	void Start ()
    {
        unitedStatePattern = GetComponent<StatePatternEnemy>();
        anim = GetComponent<Animator>();
        //StartCoroutine(WakeMeUpInside());

        asleep = false;
    }

    // Update is called once per frame
    void Update ()
    {
        if (!asleep)
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
                if(unitedStatePattern.navMeshAgent.remainingDistance < attackDistance)
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            health -= 5;
            CancelCurrentAnimation();
            //anim.SetBool("TakeDamage", true);
        }

        if (health <= 0)
        {
            CancelCurrentAnimation();
            unitedStatePattern.enabled = false;
            anim.SetTrigger("Die");
        }
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
