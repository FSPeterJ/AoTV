using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TreantController : MonoBehaviour
{
    enum TreantState
    {
        Idle,
        Wander,
        Walk,
        Bite,
        CastSpell,
        Projectile,
        Shockwave,
        TakeDamage
    }

    TreantState _cs;
    TreantState currentState
    {
        get { return _cs; }
        set
        {
            switch (value)
            {
                case TreantState.Idle:
                    idleTime = 0;
                    navAgent.enabled = false;
                    _cs = value;
                    break;
                case TreantState.Wander:
                    anim.SetBool("Walk", true);
                    navAgent.enabled = true;
                    navAgent.speed = 3.5f;
                    _cs = value;
                    break;
                case TreantState.Bite:
                    attackRangeCol.enabled = true;
                    anim.SetBool("Bite Attack", true);
                    navAgent.speed = 0;
                    navAgent.enabled = false;
                    idleTime = 0;
                    _cs = value;
                    break;
                case TreantState.Projectile:
                    attackRangeCol.enabled = true;
                    anim.SetBool("Projectile Attack", true);
                    navAgent.speed = 0;
                    navAgent.enabled = false;
                    idleTime = 0;
                    _cs = value;
                    break;
                case TreantState.Shockwave:
                    attackRangeCol.enabled = true;
                    anim.SetBool("Shockwave Attack", true);
                    navAgent.speed = 0;
                    navAgent.enabled = false;
                    idleTime = 0;
                    _cs = value;
                    break;
                case TreantState.CastSpell:
                    attackRangeCol.enabled = true;
                    anim.SetBool("Cast Spell", true);
                    navAgent.speed = 0;
                    navAgent.enabled = false;
                    idleTime = 0;
                    _cs = value;
                    break;
                default:
                    _cs = value;
                    break;
            
            }
        }
    }
    

    Animator anim;
    Vector3 targetPos;
    float targetDis;

    Vector3 wanderingSphere;
    Vector3 origin;
    NavMeshHit navHit;

    int health;

    NavMeshAgent navAgent;
    Collider attackRangeCol;

    float idleTime = 0;


    void OnEnable()
    {
        EventSystem.onPlayerPositionUpdate += UpdateTargetPosition;
    }

    void OnDisable()
    {
        EventSystem.onPlayerPositionUpdate -= UpdateTargetPosition;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        origin = transform.position;
        navAgent = GetComponent<NavMeshAgent>();
        attackRangeCol = GetComponent<Collider>();
        currentState = TreantState.Idle;
        navHit.hit = true;        
    }
    
    void Update()
    {
        targetDis = Vector3.Distance(targetPos, transform.position);

        switch(currentState)
        {
            case TreantState.Idle:
                {
                    if(idleTime > 1f)
                        if (targetDis < 20f)
                            currentState = TreantState.Walk;
                    if(idleTime > 3f)
                    {
                        currentState = TreantState.Wander;
                        navAgent.enabled = true;
                        idleTime = 0;
                    }
                    idleTime += Time.deltaTime;
                }
                break;
            case TreantState.Wander:
                {
                    if (navHit.hit)
                    {
                        navHit.hit = false;
                        float x = origin.x + (-10 + Random.Range(0, 20));
                        float z = origin.z + (-10 + Random.Range(0, 20));
                        Vector3 randDirection = new Vector3(x, transform.position.y, z);
                        navHit.position = randDirection;
                        anim.SetBool("Walk", true);
                    }
                    else if (navAgent.remainingDistance < 2)
                    {
                        navHit.hit = true;
                        anim.SetBool("Walk", false);
                        currentState = TreantState.Idle;
                    }
                    navAgent.SetDestination(navHit.position);
                }
                break;
            case TreantState.Walk:
                {
                    navAgent.SetDestination(targetPos);
                    if (targetDis < 1.8f)
                    {
                        currentState = TreantState.Bite;
                        anim.SetBool("Walk", false);
                    }
                }
                break;
            case TreantState.Bite:
                {
                    if (idleTime > 1f)
                    {
                        currentState = TreantState.Idle;
                        anim.SetBool("Bite Attack", false);

                    }
                    else
                        idleTime += Time.deltaTime;
                }
                break;
            default:
                break;
        }
    }

    void UpdateTargetPosition(Vector3 pos)
    {
        targetPos = pos;
    }
}
