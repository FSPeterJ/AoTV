using UnityEngine;

public class DeathKnight : MonoBehaviour
{
    public GameObject sword;
    private StatePatternEnemy unitedStatePattern;
    private IWeaponBehavior weaponBehavior;
    private Animator anim;
    private bool asleep = true;
    public int pointValue = 1;

    // Use this for initialization
    private void Start()
    {
        unitedStatePattern = GetComponent<StatePatternEnemy>();
        anim = GetComponent<Animator>();
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
                anim.SetTrigger("Walk");
            }
            if (unitedStatePattern.currentState.ToString() == "AlertState")
            {
                CancelCurrentAnimation();
                anim.SetTrigger("Defend");
            }
            if (unitedStatePattern.currentState.ToString() == "ChaseState") //&& unitedStatePattern.DistanceToPlayer > stopToAttackDistance)
            {
                if (unitedStatePattern.navMeshAgent.remainingDistance < unitedStatePattern.attackDistance)
                {
                    anim.ResetTrigger("Run");
                    anim.SetTrigger("Double Attack");
                    unitedStatePattern.navMeshAgent.Stop();
                }
                else
                {
                    anim.ResetTrigger("Double Attack");
                    anim.SetTrigger("Run");
                    unitedStatePattern.navMeshAgent.Resume();
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
            anim.ResetTrigger("Walk");
        }
        if (unitedStatePattern.currentState.ToString() != "AlertState")
        {
            anim.ResetTrigger("Defend");
        }
        if (unitedStatePattern.currentState.ToString() != "ChaseState") //|| unitedStatePattern.DistanceToPlayer < stopToAttackDistance)
        {
            anim.ResetTrigger("Run");
        }
    }
}