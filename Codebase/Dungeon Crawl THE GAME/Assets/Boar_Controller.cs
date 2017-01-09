using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boar_Controller : MonoBehaviour
{


    enum BoarState
    {
        Idle, Walk, Jump, Run, BiteAttack, TuskAttack, CastSpell, Defend, TakeDamage, Wander
    }


    //variables
    Animator anim;
    BoarState currentState;
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
        EventDelegates.onPlayerPositionUpdate += UpdateTargetPosition;
    }
    //unsubscribe from player movement
    void OnDisable()
    {
        EventDelegates.onPlayerPositionUpdate -= UpdateTargetPosition;
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
        if (targetdistance < 10 )
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
                    navAgent.enabled = false;
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
                    
                    navAgent.SetDestination(targetPos);
                    if (targetdistance > 20)
                    {
                        currentState = BoarState.Idle;
                        anim.SetBool("Walk", false);
                    }
                    break;
                }

            case BoarState.Jump:
                {

                }
                break;
            case BoarState.Run:
                {

                }
                break;
            case BoarState.BiteAttack:
                {

                }
                break;
            case BoarState.TuskAttack:
                {

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
