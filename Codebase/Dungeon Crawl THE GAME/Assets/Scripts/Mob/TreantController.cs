using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TreantController : MonoBehaviour, IEnemyBehavior
{
    enum AI
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
                    _cs = value;
                    break;
                case AI.Wander:
                    anim.SetBool("Walk", true);
                    navAgent.enabled = true;
                    navAgent.speed = 3.5f;
                    _cs = value;
                    break;
                case AI.Walk:
                    anim.SetBool("Walk", true);
                    navAgent.enabled = true;
                    navAgent.speed = 3.5f;
                    _cs = value;
                    break;
                case AI.Bite:
                    attackRangeCol.enabled = true;
                    anim.SetBool("Bite Attack", true);
                    navAgent.speed = 0;
                    navAgent.enabled = false;
                    idleTime = 0;
                    _cs = value;
                    break;
                case AI.Projectile:
                    attackRangeCol.enabled = true;
                    anim.SetBool("Projectile Attack", true);
                    navAgent.speed = 0;
                    navAgent.enabled = false;
                    idleTime = 0;
                    _cs = value;
                    break;
                case AI.Shockwave:
                    attackRangeCol.enabled = true;
                    anim.SetBool("Shockwave Attack", true);
                    navAgent.speed = 0;
                    navAgent.enabled = false;
                    idleTime = 0;
                    _cs = value;
                    break;
                case AI.CastSpell:
                    attackRangeCol.enabled = true;
                    anim.SetBool("Cast Spell", true);
                    navAgent.speed = 0;
                    navAgent.enabled = false;
                    idleTime = 0;
                    _cs = value;
                    break;
                case AI.TakeDamage:
                    navAgent.speed = 0;
                    navAgent.enabled = false;
                    anim.SetBool("Take Damage", true);
                    break;
                case AI.Death:
                    navAgent.speed = 0;
                    navAgent.enabled = false;
                    GetComponent<BoxCollider>().enabled = false;
                    anim.SetBool("Die", true);
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

    public GameObject weapon;
    IWeaponBehavior weaponScript;

    public int health;
    //bool dead = false;

    NavMeshAgent navAgent;
    Collider attackRangeCol;

    float idleTime = 0;
    bool dead = false;

    void OnEnable()
    {
        EventSystem.onPlayerPositionUpdate += UpdateTargetPosition;
        EventSystem.onPlayerDeath += PlayerDied;
    }

    void OnDisable()
    {
        EventSystem.onPlayerPositionUpdate -= UpdateTargetPosition;
        EventSystem.onPlayerDeath -= PlayerDied;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        origin = transform.position;
        navAgent = GetComponent<NavMeshAgent>();
        attackRangeCol = GetComponent<Collider>();
        currentState = AI.Idle;
        navHit.hit = true;
        weaponScript = weapon.GetComponent<IWeaponBehavior>();
    }

    void Update()
    {
        targetDis = Vector3.Distance(targetPos, transform.position);

        switch (currentState)
        {
            case AI.Idle:
                {
                    if (idleTime > 1f)
                        if (targetDis < 20f)
                            currentState = AI.Walk;
                    if (idleTime > 3f)
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
                        currentState = AI.Idle;
                    }
                    navAgent.SetDestination(navHit.position);
                }
                break;
            case AI.Walk:
                {
                    navAgent.SetDestination(targetPos);
                    if (targetDis < 1.8f)
                    {
                        currentState = AI.Bite;
                        anim.SetBool("Walk", false);
                    }
                    else if (targetDis < 4f)
                    {
                        currentState = AI.Shockwave;
                        anim.SetBool("Walk", false);
                    }
                    else if (targetDis < 7f)
                    {
                        currentState = AI.CastSpell;
                        anim.SetBool("Walk", false);
                    }
                    else if (targetDis < 10f)
                    {
                        currentState = AI.Projectile;
                        anim.SetBool("Walk", false);
                    }

                }
                break;
            case AI.Bite:
                {
                    if (idleTime > 1f)
                    {
                        currentState = AI.Idle;
                        anim.SetBool("Bite Attack", false);
                    }
                    else
                        idleTime += Time.deltaTime;
                }
                break;
            case AI.Projectile:
                if (idleTime > 1f)
                {
                    currentState = AI.Idle;
                    anim.SetBool("Projectile Attack", false);
                }
                else
                    idleTime += Time.deltaTime;
                break;
            case AI.Shockwave:
                if (idleTime > 1f)
                {
                    currentState = AI.Idle;
                    anim.SetBool("Shockwave Attack", false);
                }
                else
                    idleTime += Time.deltaTime;
                break;
            case AI.CastSpell:
                if (idleTime > 1f)
                {
                    currentState = AI.Idle;
                    anim.SetBool("Cast Spell", false);
                }
                else
                    idleTime += Time.deltaTime;
                break;
            case AI.TakeDamage:
                break;
            case AI.Death:
                break;
            default:
                break;
        }
    }

    public void ResetToIdle()
    {

    }

    public void TakeDamage(int damage = 1)
    {
        if (!dead)
        {
            health -= damage;
            if (health < 1)
                Kill();
            else
                currentState = AI.TakeDamage;
        }
    }

    public int RemainingHealth()
    {
        return health;
    }

    public void Kill()
    {
        currentState = AI.Death;
    }

    public void AttackFinished()
    {
        if (currentState == AI.Bite)
        {

            anim.SetBool("Bite Attack", false);
            weaponScript.AttackEnd();
            currentState = AI.Idle;
        }
    }

    public void AttackStart()
    {

        weaponScript.AttackStart();
    }
    void UpdateTargetPosition(Vector3 pos)
    {
        targetPos = pos;
    }

    void PlayerDied()
    {
        EventSystem.onPlayerPositionUpdate -= UpdateTargetPosition;
        targetPos = new Vector3(targetPos.x, 999999, targetPos.z);
    }
}
