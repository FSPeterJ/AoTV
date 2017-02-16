using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TreantController : MonoBehaviour, IEnemyBehavior
{



    Animator anim;
    Vector3 targetPos;
    float targetDis;

    Vector3 wanderingSphere;
    Vector3 origin;

    GameObject weapon;
    IWeaponBehavior weaponScript;

    [SerializeField]
    int health;
    [SerializeField]
    uint pointValue = 1;
    float shockwaveTime = 0;
    [SerializeField]
    float shockwaveCooldown = 9;
    GameObject Proj;

    NavMeshAgent navAgent;
    Collider attackRangeCol;
    bool navPos;
    Vector3 targetDestination;

    [SerializeField]
    float attackRange = 20f;

    float idleTime = 0;
    bool dead = false;

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
    [SerializeField]
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
                    navAgent.angularSpeed = 120;
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
                    navAgent.speed = 0.1f;
                    idleTime = 0;
                    navAgent.angularSpeed = 1420;
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
                    EventSystem.ScoreIncrease(pointValue);
                    _cs = value;
                    break;
                default:
                    _cs = value;
                    break;
            }
        }
    }

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
        Proj = (GameObject)Resources.Load("Prefabs/Projectiles/Worm Projectile");
        anim = GetComponent<Animator>();
        origin = transform.position;
        navAgent = GetComponent<NavMeshAgent>();
        attackRangeCol = GetComponent<Collider>();
        currentState = AI.Idle;
        weapon = FindWeapon(transform);
        weaponScript = weapon.GetComponent<IWeaponBehavior>();
    }

    void Update()
    {
        shockwaveTime += Time.deltaTime;
        targetDis = Vector3.Distance(targetPos, transform.position);

        switch (currentState)
        {
            case AI.Idle:
                {
                    if (idleTime > 1f)
                        if (targetDis < attackRange)
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
                    if (navPos == true)
                    {
                        navPos = false;
                        float x = origin.x + (-10 + Random.Range(0, 20));
                        float z = origin.z + (-10 + Random.Range(0, 20));
                        Vector3 randDirection = new Vector3(x, transform.position.y, z);
                        targetDestination = randDirection;
                        anim.SetBool("Walk", true);
                        navAgent.SetDestination(targetDestination);
                    }
                    else if (navAgent.remainingDistance < 2)
                    {
                        navPos = true;
                        anim.SetBool("Walk", false);
                        currentState = AI.Idle;
                    }

                }
                break;
            case AI.Walk:
                {
                    navAgent.SetDestination(targetPos);
                    if (targetDis < 2.9f)
                    {

                        currentState = AI.Bite;
                        anim.SetBool("Walk", false);
                    }
                    else if (targetDis < 8f && shockwaveTime > shockwaveCooldown)
                    {
                        currentState = AI.Shockwave;
                        anim.SetBool("Walk", false);
                    }
                    else if (targetDis < 14f)
                    {
                        currentState = AI.CastSpell;
                        anim.SetBool("Walk", false);
                    }
                    else if (targetDis < attackRange)
                    {
                        currentState = AI.Projectile;
                        anim.SetBool("Walk", false);
                    }
                }
                break;
            case AI.Bite:
                break;
            case AI.Projectile:
                navAgent.SetDestination(targetPos);
                
                if (AttackRangeCheck())
                {
                    anim.SetBool("Projectile Attack", true);
                    currentState = AI.Idle;
                }
                break;
            case AI.Shockwave:
                break;
            case AI.CastSpell:
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
        currentState = AI.Idle;
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

    public void CreateProjectile()
    {
        Instantiate(Proj, weapon.transform.position, weapon.transform.rotation * Quaternion.Euler(-15, 0, 0));
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

    GameObject FindWeapon(Transform obj)
    {
        foreach (Transform tr in obj)
        {
            if (tr.tag == "Weapon")
            {
                return tr.gameObject;
            }
            if (tr.childCount > 0)
            {
                GameObject temp = FindWeapon(tr);
                if (temp)
                {
                    return temp;
                }
            }
        }
        return null;
    }

    bool AttackRangeCheck()
    {
        Debug.DrawRay(transform.position + Vector3.up, transform.forward * attackRange, Color.red);
        Ray attackRangeForward = new Ray(transform.position + Vector3.up, transform.forward * attackRange);
        RaycastHit[] forwardHit = Physics.RaycastAll(attackRangeForward, attackRange);

        for (int i = 0; i < forwardHit.Length; i++)
        {
            if (forwardHit[i].collider.gameObject.tag == "Player")
            {
                return true;
            }

        }
        return false;
    }
}
