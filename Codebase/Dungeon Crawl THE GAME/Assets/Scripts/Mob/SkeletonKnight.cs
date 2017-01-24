using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonKnight : MonoBehaviour, IEnemyBehavior{

    Animator anim;
    bool asleep = true;
    StatePatternEnemy unitedStatePattern;
    bool attacking = false;
    [SerializeField]
    int health = 10;
    Animator playerAnim;
    bool dead = false;
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

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player") // && anim.GetCurrentAnimatorStateInfo(0).IsName("Double Attack"))
        {
            other.gameObject.GetComponent<Player>().TakeDamage(2);
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

    public void TakeDamage(int damage = 1)
    {
        if (!dead)
        {
            health -= damage;
            if (RemainingHealth() < 1)
            {
                Kill();
            }
            else
            {
                CancelCurrentAnimation();
                anim.SetTrigger("Take Damage");
            }
        }
    }

    public int RemainingHealth()
    {
        return health;
    }

    public void Kill()
    {
        anim.SetBool("Die", true);
    }

    public void ResetToIdle()
    {
        //targetPos = pos;
    }
}
