using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArcher : MonoBehaviour
{

    Animator anim;
    bool asleep = true;
    StatePatternEnemy unitedStatePattern;
    bool attacking = false;
    float attackDistance = 15;
    float reloadTime = 2;
    Animator playerAnim;
    public GameObject arrow;
    public GameObject arrowSpawn;
    Vector3 arrowPos;
    Quaternion arrowQuat;
    public uint pointValue = 1;


    // Use this for initialization
    void Start()
    {
        unitedStatePattern = GetComponent<StatePatternEnemy>();
        anim = GetComponent<Animator>();
        asleep = false;
    }

    // Update is called once per frame
    void Update()
    {
        arrow.transform.rotation = arrowQuat;
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
                if (unitedStatePattern.navMeshAgent.remainingDistance < attackDistance)
                {

                    unitedStatePattern.navMeshAgent.Stop();
                    anim.SetBool("Run", false);
                    anim.SetLookAtPosition(unitedStatePattern.chaseTarget.transform.position);
                    anim.SetTrigger("Arrow Attack");
                }
                else
                {
                    unitedStatePattern.navMeshAgent.Resume();
                    anim.ResetTrigger("Arrow Attack");
                    anim.SetBool("Run", true);
                }
            }
        }

        //if (unitedStatePattern.health_GetHealth() <= 0)
        //{
        //    CancelCurrentAnimation();
        //    unitedStatePattern.enabled = false;
        //    anim.SetBool("Die", true);
        //}
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
    public void ShootArrow()
    {
        Vector3 towardsPlayer = unitedStatePattern.chaseTarget.position - gameObject.transform.position;
        arrowQuat = new Quaternion(-3.14f / 2, transform.rotation.y, gameObject.transform.rotation.z, transform.rotation.w);


        //AudioSource.PlayClipAtPoint(shootSound, transform.position, PlayerPrefs.GetFloat("SFXVolume"));
        GameObject tempBullet = Instantiate(arrow, arrowSpawn.transform.position, arrowQuat);
        tempBullet.GetComponent<Rigidbody>().velocity = towardsPlayer;
        tempBullet.GetComponent<Rigidbody>().AddForce(towardsPlayer, ForceMode.Acceleration);

    }
}
