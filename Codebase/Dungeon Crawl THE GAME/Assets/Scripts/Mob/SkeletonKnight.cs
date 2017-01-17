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
    Collider AttackRegionCollider;
    Animator playerAnim;
    // Use this for initialization
	void Start ()
    {
        unitedStatePattern = GetComponent<StatePatternEnemy>();
        anim = GetComponent<Animator>();
        AttackRegionCollider = GetComponent<Collider>();
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

        if (unitedStatePattern.health_GetHealth() <= 0)
        {
            CancelCurrentAnimation();
            unitedStatePattern.enabled = false;
            anim.SetBool("Die", true);
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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") // && anim.GetCurrentAnimatorStateInfo(0).IsName("Double Attack"))
        {
            other.gameObject.GetComponent<Player>().TakeDamage(3);
            other.gameObject.GetComponent<Animator>().SetTrigger("Take Damage");
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
