using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArcher : MonoBehaviour
{
    StatePatternEnemy unitedStatePattern;
    Animator anim;
    public GameObject arrow;
    public GameObject arrowSpawn;
    Vector3 arrowPos;
    Quaternion arrowQuat;
    bool asleep;
    //bool attacking = false;
    float attackDistance = 15;
    //float reloadTime = 2;
    public uint pointValue = 1;
    Rigidbody body;

    // Use this for initialization
    void Start()
    {
        unitedStatePattern = GetComponent<StatePatternEnemy>();
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
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
                body.rotation = Quaternion.Slerp(body.rotation, Quaternion.LookRotation(unitedStatePattern.chaseTarget.transform.position - body.position), 10 * Time.deltaTime);
                body.AddForce(gameObject.transform.up * -150);
                if (unitedStatePattern.navMeshAgent.remainingDistance < attackDistance)
                {
                    unitedStatePattern.navMeshAgent.Stop();
                    anim.SetBool("Run", false);
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
