using UnityEngine;

public class Dragon_Mike : MonoBehaviour
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
        gameObject.transform.parent = null;
        unitedStatePattern = GetComponent<StatePatternEnemy>();
        anim = GetComponent<Animator>();
        //StartCoroutine(WakeMeUpInside());
        weaponBehavior = sword.GetComponent<IWeaponBehavior>();
        asleep = false;
        anim.SetBool("Fly Idle", true);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!asleep && unitedStatePattern.alive)
        {
            if (unitedStatePattern.currentState.ToString() == "PatrolState")
            {
                //CancelCurrentAnimation();
                anim.SetBool("Fly Forward", true);
            }
            if (unitedStatePattern.currentState.ToString() == "AlertState")
            {
                //CancelCurrentAnimation();
                anim.SetTrigger("Fly Fire Breath Attack");
            }
            if (unitedStatePattern.currentState.ToString() == "ChaseState") //&& unitedStatePattern.DistanceToPlayer > stopToAttackDistance)
            {
                //CancelCurrentAnimation();
                anim.SetLookAtPosition(unitedStatePattern.chaseTarget.position);
                if (unitedStatePattern.navMeshAgent.remainingDistance < unitedStatePattern.attackDistance)
                {
                    unitedStatePattern.navMeshAgent.Stop();
                    anim.SetBool("Fly Forward", false);
                    anim.SetTrigger("Fly Bite Attack");
                }
                else
                {
                    unitedStatePattern.navMeshAgent.Resume();
                    anim.ResetTrigger("Fly Bite Attack");
                    anim.SetBool("Fly Forward", true);
                }
            }
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
}