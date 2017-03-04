using System.Collections;
using UnityEngine;

public class SkeletonKnight : MonoBehaviour
{
    public GameObject sword;
    private StatePatternEnemy unitedStatePattern;
    private IWeaponBehavior weaponBehavior;
    private Animator anim;
    private bool asleep = true;
    public int pointValue = 1;
    private Rigidbody body;

    // Use this for initialization
    private void Start()
    {
        unitedStatePattern = GetComponent<StatePatternEnemy>();
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
        //StartCoroutine(WakeMeUpInside());
        weaponBehavior = sword.GetComponent<IWeaponBehavior>();
        asleep = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!asleep && unitedStatePattern.alive)
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
                if (unitedStatePattern.navMeshAgent.remainingDistance < unitedStatePattern.attackDistance)
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
        else
        {
            CancelCurrentAnimation();
        }
    }

    private void BeginAttacking()
    {
        weaponBehavior.AttackStart();
    }

    private void EndAttacking()
    {
        weaponBehavior.AttackEnd();
    }

    private void CancelCurrentAnimation()
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

    private IEnumerator WakeMeUpInside()
    {
        while (asleep)
        {
            unitedStatePattern.navMeshAgent.Stop();
            anim.SetBool("Die", true);
            yield return new WaitForSeconds(5);
        }
    }
}