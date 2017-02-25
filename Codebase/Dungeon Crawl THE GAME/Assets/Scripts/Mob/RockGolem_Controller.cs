using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RockGolem_Controller : MonoBehaviour, IEnemyBehavior
{

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
                    anim.SetBool("Fly Forward", true);
                    navAgent.enabled = true;
                    navAgent.speed = 3.5f;
                    _cs = value;
                    break;
                case AI.Walk:
                    anim.SetBool("Fly Forward", true);
                    navAgent.enabled = true;
                    navAgent.speed = 4f;
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
                    EventSystem.ScoreIncrease(pointValue);

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
    float targetdistance;
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
    public int pointValue = 1;




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
    private string monsterName;

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
                    }
                    else if (idleTime > 1f)
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

                    if (wanderTargetSet == false)
                    {
                        float x = originPos.x + (-10 + Random.Range(0, 20));
                        float z = originPos.z + (-10 + Random.Range(0, 20));
                        wanderTarget = new Vector3(x, transform.position.y, z);
                        anim.SetBool("Fly Forward", true);
                        wanderTargetSet = true;
                        navAgent.SetDestination(wanderTarget);
                    }
                    else if (navAgent.remainingDistance < 2)
                    {
                        anim.SetBool("Fly Forward", false);
                        currentState = AI.Idle;
                    }
                }
                break;
            case AI.Walk:
                {
                    navAgent.SetDestination(targetPos);
                    if (targetdistance > aggroRange)
                    {
                        currentState = AI.Idle;
                        anim.SetBool("Fly Forward", false);
                    }
                    else if (targetdistance < 1.9f)
                    {
                        anim.SetBool("Fly Forward", false);
                        currentState = AI.Attack;
                    }
                    break;
                }

            case AI.Jump:
                {
                    //Probably not
                }
                break;
            case AI.Run:
                {

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

                    if (targetdistance > 2f)
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
        AttackFinished();
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

    public string Name()
    {
        return monsterName;
    }

    public float HPOffsetHeight()
    {
        return GetComponent<Renderer>().bounds.size.y + 1;
    }
}
