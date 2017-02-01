using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CactusMonster_Controller : MonoBehaviour, IEnemyBehavior
{

    public HUD huD;
    public AI _cs;
    AI currentState
    {
        get { return _cs; }
        set
        {
            switch (value)
            {
                case AI.Idle:
                    idleTime = 0;
                    navAgent.enabled = false;
                    wanderTargetSet = false;
                    //You can prevent a state assignment with a check here
                    _cs = value;
                    break;
                case AI.Wander:
                    anim.SetBool("Walk", true);
                    navAgent.enabled = true;
                    navAgent.speed = walkSpeed;
                    _cs = value;
                    break;
                case AI.Walk:
                    anim.SetBool("Walk", true);
                    navAgent.enabled = true;
                    navAgent.speed = walkSpeed;
                    _cs = value;
                    break;
                case AI.Run:
                    anim.SetBool("Run", true);
                    navAgent.enabled = true;
                    navAgent.speed = runSpeed;
                    _cs = value;
                    break;
                case AI.Attack:
                    if (attack == 1)
                    {
                        anim.SetBool("Right Punch Attack", true);
                    }
                    else
                    {
                        anim.SetBool("Left Punch Attack", true);
                    }
                    navAgent.enabled = false;
                    navAgent.speed = 0;
                    _cs = value;
                    break;
                case AI.TakeDamage:
                    anim.SetBool("Take Damage", true);
                    //_cs = value;
                    break;
                case AI.Die:
                    navAgent.enabled = false;
                    navAgent.speed = 0;
                    bCollider.enabled = false;
                    anim.SetBool("Die", true);
                    _cs = value;
                    break;
                default:
                    _cs = value;
                    break;
            }
        }
    }

    public enum AI
    {
        Idle, Wander, Walk, Jump, Run, CastSpell, Defend, TakeDamage, Die, Attack
    }


    //Variables
    Vector3 targetPos;
    public float targetdistance;
    bool dead = false;


    //Wandering variarables;
    Vector3 originPos;
    NavMeshHit navHitPos;
    Vector3 wanderTarget;
    bool wanderTargetSet = false;



    //Stat variables
    public int health;
    public float idleTime = 0;
    public float aggroRange = 20f;
    public float minrunRange = 10f;
    public float attackRange = 1.5f;
    public float walkSpeed = 3f;
    public float runSpeed = 4f;


    //Component References
    Animator anim;
    NavMeshAgent navAgent;
    BoxCollider bCollider;
    //This is a hack together way to get the weapon.
    public GameObject weaponR;
    public GameObject weaponL;
    IWeaponBehavior weaponScriptR;
    IWeaponBehavior weaponScriptL;

    int attack;

    void OnEnable()
    {
        EventSystem.onPlayerPositionUpdate += UpdateTargetPosition;
        EventSystem.onPlayerDeath += PlayerDied;
    }
    //unsubscribe from player movement
    void OnDisable()
    {
        EventSystem.onPlayerPositionUpdate -= UpdateTargetPosition;
        EventSystem.onPlayerDeath -= PlayerDied;

    }


    // Use this for initialization
    void Start()
    {
        bCollider = GetComponent<BoxCollider>();
        anim = GetComponent<Animator>();
        originPos = transform.position;
        navAgent = GetComponent<NavMeshAgent>();
        weaponScriptR = weaponR.GetComponent<IWeaponBehavior>();
        weaponScriptL = weaponL.GetComponent<IWeaponBehavior>();
        currentState = AI.Idle;
        navHitPos.hit = true;
        attack = Random.Range(0, 1);
    }




    // Update is called once per frame
    void Update()
    {

        //targetPos = //eventmanager passed pos
        targetdistance = Vector3.Distance(targetPos, transform.position);

        //StateMachine
        switch (currentState)
        {
            case AI.Idle:
                {
                    idleTime += Time.deltaTime;
                    if (targetdistance < 2f)
                    {
                        currentState = AI.Attack;
                        break;
                    }
                    if (idleTime > 1f)
                    {
                        if (targetdistance < aggroRange)
                        {
                            currentState = AI.Walk;
                            break;
                        }
                        else if (idleTime > 4f)
                        {
                            currentState = AI.Wander;
                            navAgent.enabled = true;
                            idleTime = 0;
                        }
                    }
                }
                break;
            case AI.Wander:
                {
                    if (targetdistance < aggroRange)
                    {
                        currentState = AI.Walk;
                    }
                    else if (wanderTargetSet == false)
                    {
                        float x = originPos.x + (-10 + Random.Range(0, 20));
                        float z = originPos.z + (-10 + Random.Range(0, 20));
                        wanderTarget = new Vector3(x, transform.position.y, z);
                        wanderTargetSet = true;
                        navAgent.SetDestination(wanderTarget);
                    }
                    else if (navAgent.remainingDistance < 2)
                    { 
                        currentState = AI.Idle;
                        anim.SetBool("Walk", false);
                    }
                }
                break;
            case AI.Walk:
                {

                    navAgent.SetDestination(targetPos);
                    if (targetdistance > aggroRange)
                    {
                        
                        currentState = AI.Idle;
                        anim.SetBool("Walk", false);
                    }
                    else if (targetdistance < attackRange)
                    {
                        currentState = AI.Attack;
                        anim.SetBool("Walk", false);

                    }
                    else if (targetdistance < aggroRange && targetdistance > minrunRange)
                    {
                        
                        currentState = AI.Run;
                        anim.SetBool("Walk", false);
                    }
                }
                break;
            case AI.Jump:
                {
                    //Probably not
                }
                break;
            case AI.Run:
                {
                    navAgent.SetDestination(targetPos);
                    if (targetdistance < minrunRange)
                    {
                        currentState = AI.Walk;
                        anim.SetBool("Run", false);

                    }
                    else if (targetdistance > aggroRange)
                    {

                        currentState = AI.Idle;
                        anim.SetBool("Run", false);
                    }
                }
                break;
            case AI.CastSpell:
                {
                    //Probably not
                }
                break;
            case AI.Defend:
                {

                }
                break;
            case AI.TakeDamage:
                {

                }
                break;
            case AI.Die:
                {

                }
                break;
            case AI.Attack:
                {
                    //Target Track
                    Vector3 lookPos = (transform.position - targetPos);
                    float angle = -(Mathf.Atan2(lookPos.z, lookPos.x) * Mathf.Rad2Deg) - 90;
                    transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, 1, 0));

                    if (targetdistance > 5f)
                    {
                        currentState = AI.Idle;
                    }
                }
                break;
            default:
                {

                }
                break;
        }
    }


    public void ResetToIdle()
    {
        currentState = AI.Idle;
    }

    void UpdateTargetPosition(Vector3 pos)
    {
        targetPos = pos;
    }

    public void TakeDamage(int damage = 1)
    {
        AttackFinished();
        if (!dead)
        {
            health -= damage;
            if (health < 1)
            {
                Kill();
                ScoreInc();
            }
            else
            {
                currentState = AI.TakeDamage;
            }
        }
    }

    public int RemainingHealth()
    {
        return health;
    }
    public void Kill()
    {
        currentState = AI.Die;

    }

    public void AttackFinished()
    {
        if (currentState == AI.Attack)
        {
            if (attack == 1)
            {
                anim.SetBool("Right Punch Attack", false);
                weaponScriptR.AttackEnd();
                attack = 0;
            }
            else
            {
                anim.SetBool("Left Punch Attack", false);
                weaponScriptL.AttackEnd();
                attack = 1;
            }
            currentState = AI.Idle;
        }
    }

    public void AttackStart()
    {
        if (attack == 1)
        {
            weaponScriptR.AttackStart();
        }
        else
        {
            weaponScriptL.AttackStart();
        }
    }


    void PlayerDied()
    {
        EventSystem.onPlayerPositionUpdate -= UpdateTargetPosition;
        targetPos = new Vector3(targetPos.x, 999999, targetPos.z);
    }

    void ScoreInc()
    {
        //Cannot call huD
        //huD.UpdateScore();
    }
}
