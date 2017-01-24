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
        TakeDamage,
        Death
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
                case TreantState.Walk:
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
                case TreantState.TakeDamage:
                    navAgent.speed = 0;
                    navAgent.enabled = false;
                    anim.SetBool("Take Damage", true);
                    break;
                case TreantState.Death:
                    navAgent.speed = 0;
                    navAgent.enabled = false;
                    GetComponent<BoxCollider>().enabled = false;
                    anim.SetBool("Die", true);
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
    //bool dead = false;

    NavMeshAgent navAgent;
    Collider attackRangeCol;

    float idleTime = 0;
    bool dead = false;

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
                    else if (targetDis < 4f)
                    {
                        currentState = TreantState.Shockwave;
                        anim.SetBool("Walk", false);
                    }
                    else if (targetDis < 7f)
                    {
                        currentState = TreantState.CastSpell;
                        anim.SetBool("Walk", false);
                    }
                    else if (targetDis < 10f)
                    {
                        currentState = TreantState.Projectile;
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
            case TreantState.Projectile:
                if (idleTime > 1f)
                {
                    currentState = TreantState.Idle;
                    anim.SetBool("Projectile Attack", false);
                }
                else
                    idleTime += Time.deltaTime;
                break;
            case TreantState.Shockwave:
                if (idleTime > 1f)
                {
                    currentState = TreantState.Idle;
                    anim.SetBool("Shockwave Attack", false);
                }
                else
                    idleTime += Time.deltaTime;
                break;
            case TreantState.CastSpell:
                if (idleTime > 1f)
                {
                    currentState = TreantState.Idle;
                    anim.SetBool("Cast Spell", false);
                }
                else
                    idleTime += Time.deltaTime;
                break;
            case TreantState.TakeDamage:
                break;
            case TreantState.Death:
                break;
            default:
                break;
        }
    }

    public void TakeDamage(int damage = 1)
    {
        if (!dead)
        {
            health -= damage;
            if (health < 1)
                Kill();
            else
                currentState = TreantState.TakeDamage;
        }
    }
    
    public int RemainingHealth()
    {
        return health;
    }

    public void Kill()
    {
        currentState = TreantState.Death;
    }
    void UpdateTargetPosition(Vector3 pos)
    {
        targetPos = pos;
    }
}
