using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArcher : MonoBehaviour {

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

    // Use this for initialization
    void Start()
    {
        unitedStatePattern = GetComponent<StatePatternEnemy>();
        anim = GetComponent<Animator>();
        asleep = false;
        arrowQuat = new Quaternion(90, transform.rotation.y, gameObject.transform.rotation.z, transform.rotation.w);
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
                    unitedStatePattern.transform.Rotate(unitedStatePattern.chaseTarget.transform.position, 0f);
                    anim.SetBool("Run", false);
                    anim.SetTrigger("Arrow Attack");
                    if (reloadTime <= .025)
                    {
                        StartCoroutine(ShootArrow());
                    }
                }
                else
                {
                    unitedStatePattern.navMeshAgent.Resume();
                    anim.ResetTrigger("Arrow Attack");
                    anim.SetBool("Run", true);
                }
            }
            reloadTime -= Time.deltaTime;
            if (reloadTime < 0)
            {
                reloadTime = 2;
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
    IEnumerator ShootArrow()
    {
        Vector3 towardsPlayer = unitedStatePattern.chaseTarget.position - gameObject.transform.position;


        //AudioSource.PlayClipAtPoint(shootSound, transform.position, PlayerPrefs.GetFloat("SFXVolume"));
        GameObject tempBullet = Instantiate(arrow, arrowSpawn.transform.position, arrowQuat);
        tempBullet.GetComponent<Rigidbody>().velocity = towardsPlayer;
        tempBullet.GetComponent<Rigidbody>().AddForce(towardsPlayer, ForceMode.Acceleration);
        yield return new WaitForSeconds(0.4f);

    }
}
