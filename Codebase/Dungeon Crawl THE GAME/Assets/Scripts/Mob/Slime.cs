using UnityEngine;

public class Slime : MonoBehaviour
{
    public GameObject sword;
    private StatePatternEnemy unitedStatePattern;
    private IWeaponBehavior weaponBehavior;
    private Animator anim;
    private bool asleep = true;
    public int pointValue = 1;
    public GameObject[] Slimes;
    public Transform[] Spawns;

    // Use this for initialization
    private void Start()
    {
        gameObject.transform.parent = null;
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
                //CancelCurrentAnimation();
                anim.SetBool("Move Forward", true);
            }
            if (unitedStatePattern.currentState.ToString() == "AlertState")
            {
                //CancelCurrentAnimation();
                anim.SetBool("Defend", true);
            }
            if (unitedStatePattern.currentState.ToString() == "ChaseState") //&& unitedStatePattern.DistanceToPlayer > stopToAttackDistance)
            {
                //CancelCurrentAnimation();
                anim.SetLookAtPosition(unitedStatePattern.chaseTarget.position);
                if (unitedStatePattern.navMeshAgent.remainingDistance < unitedStatePattern.attackDistance)
                {
                    unitedStatePattern.navMeshAgent.Stop();
                    anim.SetBool("Move Forward", false);
                    anim.SetTrigger("Melee Attack");
                }
                else
                {
                    unitedStatePattern.navMeshAgent.Resume();
                    anim.ResetTrigger("Melee Attack");
                    anim.SetBool("Move Forward", true);
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
        //if (unitedStatePattern.currentState.ToString() != "PatrolState")
        //{
        //    anim.SetBool("Move Forward", false);
        //}
        //if (unitedStatePattern.currentState.ToString() != "AlertState")
        //{
        //    anim.SetBool("Defend", false);
        //}
        //if (unitedStatePattern.currentState.ToString() != "ChaseState") //|| unitedStatePattern.DistanceToPlayer < stopToAttackDistance)
        //{
        //    anim.SetBool("Run", false);
        //}
    }

    public void Spawn()
    {
        for (int i = 0; i < Slimes.Length; i++)
        {
            Instantiate(Slimes[i], Spawns[i], true);
        }
    }
}