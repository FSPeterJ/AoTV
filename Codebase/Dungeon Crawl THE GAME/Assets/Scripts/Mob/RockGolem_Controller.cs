using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RockGolem_Controller : MonoBehaviour
{

    AI _cs;
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
                    //You can prevent a state assignment with a check here
                    _cs = value;
                    break;
                case AI.Wander:
                    anim.SetBool("Walk", true);
                    navAgent.enabled = true;
                    navAgent.speed = 3.5f;
                    _cs = value;
                    break;
                case AI.Walk:
                    anim.SetBool("FlyForward", true);
                    navAgent.enabled = true;
                    navAgent.speed = 4f;
                    _cs = value;
                    break;
                case AI.RightPunch:
                    anim.SetBool("RightPunch", true);
                    navAgent.enabled = true;
                    navAgent.speed = 4f;
                    _cs = value;
                    break;
                default:
                    _cs = value;
                    break;
            }
        }

    }

    enum AI
    {
        Idle, Wander, Walk, Jump, Run, CastSpell, Defend, TakeDamage, Die, RightPunch
    }


    //Variables
    Vector3 targetPos;
    float targetdistance;


    //Wandering variarables;
    Vector3 originPos;
    NavMeshHit navHitPos;



    //Stat variables
    public int health;
    public float idleTime = 0;
    public float aggroRange = 20f;


    //Component References
    Animator anim;
    NavMeshAgent navAgent;



    void OnEnable()
    {
        EventSystem.onPlayerPositionUpdate += UpdateTargetPosition;
    }
    //unsubscribe from player movement
    void OnDisable()
    {
        EventSystem.onPlayerPositionUpdate -= UpdateTargetPosition;
    }


    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        originPos = transform.position;
        navAgent = GetComponent<NavMeshAgent>();
        currentState = AI.Idle;
        navHitPos.hit = true;
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
                    if (idleTime > 1f)
                    {
                        if (targetdistance < aggroRange)
                        {
                            currentState = AI.Walk;
                        }
                    }
                    else if (idleTime > 4f)
                    {

                        currentState = AI.Wander;
                        navAgent.enabled = true;
                        idleTime = 0;
                    }
                    idleTime += Time.deltaTime;
                }
                break;
            case AI.Wander:
                {

                    if (navHitPos.hit == true)
                    {
                        navHitPos.hit = false;
                        float x = originPos.x + (-10 + Random.Range(0, 20));
                        float z = originPos.z + (-10 + Random.Range(0, 20));
                        Vector3 randDirection = new Vector3(x, transform.position.y, z);
                        navHitPos.position = randDirection;
                        anim.SetBool("Walk", true);
                    }
                    else if (navAgent.remainingDistance < 2)
                    {
                        navHitPos.hit = true;
                        anim.SetBool("Walk", false);
                        currentState = AI.Idle;
                    }
                    navAgent.SetDestination(navHitPos.position);

                }
                break;
            case AI.Walk:
                {

                    navAgent.SetDestination(targetPos);
                    if (targetdistance > 20f)
                    {
                        currentState = AI.Idle;
                        anim.SetBool("Walk", false);
                    }
                    else if (targetdistance < 1.5f)
                    {
                        currentState = AI.TakeDamage;
                        anim.SetBool("Walk", false);
                    }
                    break;
                }

            case AI.Jump:
                {

                }
                break;
            case AI.Run:
                {

                }
                break;
            case AI.CastSpell:
                {

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
            case AI.RightPunch:
                {

                }
                break;
        }

    }

    void UpdateTargetPosition(Vector3 pos)
    {
        targetPos = pos;
    }

}
