using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boar_Controller : MonoBehaviour
{

    //Use for executing commands on when first entering a state
    //Can also be used to prevent states from changing under certain conditions
    BoarState _cs;
    BoarState currentState
    {
        get { return _cs; }
        set
        {
            switch (value)
            {
                case BoarState.Idle:
                    _cs = value;
                    break;
                case BoarState.Wander:
                    _cs = value;
                    break;
                case BoarState.Jump:
                    _cs = value;
                    break;
                case BoarState.Run:
                    _cs = value;
                    break;
                default:
                    _cs = value;
                    break;
            }
        }

    }
    enum BoarState
    {
        Idle, Walk, Jump, Run, BiteAttack, TuskAttack, CastSpell, Defend, TakeDamage, Wander
    }


    //variables
    Animator anim;
    Vector3 targetPos;
    float targetdistance;


    //wandering variarables;
    Vector3 wanderingSphere;
    Vector3 originPos;
    NavMeshHit navHitPos;



    //Stat variables
    int health;

    //References
    NavMeshAgent navAgent;

    float idleTime = 0;


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
        currentState = BoarState.Idle;
        navHitPos.hit = true;
    }




    // Update is called once per frame
    void Update()
    {
        //targetPos = //eventmanager passed pos
        targetdistance = Vector3.Distance(targetPos, transform.position);
        if (targetdistance < 20f && currentState== BoarState.Idle && targetdistance>10f)
        {
            currentState = BoarState.Run;
            anim.SetBool("Run", true);
            navAgent.enabled = true;
        }
        if (targetdistance < 8f && currentState == BoarState.Idle)
        {
            currentState = BoarState.Walk;
            anim.SetBool("Walk", true);
            navAgent.enabled = true;
        }
            //StateMachine
            switch (currentState)
        {
            case BoarState.Idle:
                {
                    navAgent.speed = 0;
                    navAgent.enabled = true;
                    navAgent.enabled = false;
                    navAgent.speed = 3.5f;
                    if (idleTime > 4)
                    {
                        
                        currentState = BoarState.Wander;
                        navAgent.enabled = true;
                        idleTime = 0;
                    }
                    idleTime += Time.deltaTime;
                }
                break;
            case BoarState.Wander:
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
                    else if(navAgent.remainingDistance < 2)
                    {
                        navHitPos.hit = true;
                        anim.SetBool("Walk", false);
                        currentState = BoarState.Idle;
                    }
                    navAgent.SetDestination(navHitPos.position);

                }
                break;
            case BoarState.Walk:
                {
                    navAgent.speed = 3.5f;
                    navAgent.SetDestination(targetPos);
                    if (targetdistance < 1.8f)
                    {
                        navAgent.speed = 0;
                        navAgent.enabled = false;
                        currentState = BoarState.BiteAttack;
                        anim.SetBool("Walk", false);
                        idleTime = 0;
                    }
                        break;

                }

            case BoarState.Jump:
                {

                }
                break;
            case BoarState.Run:
                {
                    navAgent.speed = 10;
                    navAgent.SetDestination(targetPos);
                    if (targetdistance < 1.8f)
                    {
                        
                        navAgent.speed = 0;
                        navAgent.enabled = false;
                        currentState = BoarState.TuskAttack;
                        anim.SetBool("Run", false);
                        idleTime = 0;
                    }
                }
                break;
            case BoarState.BiteAttack:
                {
                    anim.SetBool("Bite Attack", true);
                    GetComponent<Collider>().enabled = true;
                    if (idleTime > 1f)
                    {
                        currentState = BoarState.Idle;
                        anim.SetBool("Bite Attack", false);
                        GetComponent<Collider>().enabled = false;
                    }
                    else
                        idleTime += Time.deltaTime;
                }
                break;
            case BoarState.TuskAttack:
                {
                   
                    anim.SetBool("Tusk Attack", true);
                    GetComponent<Collider>().enabled =true;
                    if (idleTime > 1f)
                    {
                        currentState = BoarState.Idle;
                        anim.SetBool("Tusk Attack", false);
                        GetComponent<Collider>().enabled = false;
                    }
                    else
                    idleTime += Time.deltaTime;
                }
                break;
            case BoarState.CastSpell:
                {

                }
                break;
            case BoarState.Defend:
                {

                }
                break;
            case BoarState.TakeDamage:
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
