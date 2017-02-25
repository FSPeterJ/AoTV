using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class QueenWormController : MonoBehaviour, IEnemyBehavior
{

    enum AI
    {
        Idle,
        Move,
        ClawAttack,
        BiteAttack,
        CastSpell,
        BreathAttackStart,
        BreathAttackLoop,
        BreathAttackEnd,
        Summon,
        Defend,
        TakeDamage,
        Die
    }

    [SerializeField]
    int maxHealth;

    int health;
    bool dead = false;

    Animator anim;
    float PlayerDist;
    Vector3 PlayerPos;
    bool defendTime;
    float idleTime;
    NavMeshAgent navigate;
    [SerializeField]
    int pointValue = 1;
    GameObject weapon;
    IWeaponBehavior weaponScript;

    [SerializeField]
    GameObject weaponBreath;
    BreathAttack Breath;
    [SerializeField]
    GameObject Exit;

    [SerializeField]
    float BreathAttackTime = 888;
    float BreathAttackCD = 15;

    [SerializeField]
    int attackRange = 5;

    [SerializeField]
    AI cs;
    AI currentState
    {
        get { return cs; }
        set
        {
            switch (value)
            {
                case AI.Idle:
                    navigate.speed = 0;
                    navigate.Stop();
                    idleTime = 0;
                    cs = value;
                    break;
                case AI.Move:
                    anim.SetBool("Move", true);
                    navigate.Resume();
                    navigate.speed = 10f;
                    cs = value;
                    break;
                case AI.ClawAttack:
                    anim.SetBool("Claw Attack", true);
                    navigate.speed = 0;
                    navigate.Stop();
                    idleTime = 0;
                    cs = value;
                    break;
                case AI.BiteAttack:
                    anim.SetBool("Bite Attack", true);
                    navigate.speed = 0;
                    navigate.Stop();
                    idleTime = 0;
                    cs = value;
                    break;
                case AI.CastSpell:
                    anim.SetBool("Cast Spell", true);
                    navigate.speed = 0;
                    navigate.Stop();
                    idleTime = 0;
                    cs = value;
                    break;
                case AI.BreathAttackStart:
                    BreathAttackTime = 0;
                    anim.SetBool("Breath Attack", true);
                    navigate.speed = 0;
                    navigate.Stop();
                    idleTime = 0;
                    cs = value;
                    break;
                case AI.BreathAttackLoop:
                    cs = value;
                    break;
                case AI.BreathAttackEnd:
                    anim.SetBool("Bite Attack", false);
                    cs = value;
                    break;
                case AI.Summon:
                    anim.SetBool("Summon", true);
                    navigate.speed = 0;
                    navigate.Stop();
                    idleTime = 0;
                    cs = value;
                    break;
                case AI.Defend:
                    anim.SetBool("Defend", true);
                    navigate.speed = 0;
                    navigate.Stop();
                    idleTime = 0;
                    cs = value;
                    break;
                case AI.TakeDamage:
                    anim.SetBool("Take Damage", true);
                    break;
                case AI.Die:
                    EventSystem.ScoreIncrease(pointValue);

                    Exit.SetActive(true);
                    navigate.speed = 0f;
                    navigate.enabled = false;
                    anim.SetBool("Die", true);
                    dead = true;
                    cs = value;
                    break;
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        navigate = GetComponent<NavMeshAgent>();
        currentState = AI.Idle;
        health = maxHealth;

        weapon = FindWeapon(transform).gameObject;
        weaponScript = weapon.GetComponent<IWeaponBehavior>();

        Breath = weaponBreath.GetComponent<BreathAttack>();

    }

    // Update is called once per frame
    void Update()
    {
        BreathAttackTime += Time.deltaTime;

        PlayerDist = Vector3.Distance(PlayerPos, transform.position);

        switch (currentState)
        {
            case AI.Idle:
                if (idleTime > 1f)
                {
                    if (PlayerDist <= 5f && AttackRangeCheck(5))
                    {
                        if (defendTime)
                        {
                            currentState = AI.Defend;
                            defendTime = false;
                        }
                        else
                        {
                            currentState = AI.ClawAttack;
                            defendTime = true;
                        }
                    }
                    else if (PlayerDist <= 8f && AttackRangeCheck(9))
                        currentState = AI.BiteAttack;
                    else if (PlayerDist <= 30f)
                    {
                        if (health <= 10 && AttackRangeCheck(9) && BreathAttackTime > BreathAttackCD )
                        {
                            currentState = AI.BreathAttackStart;
                        }
                        else
                        {
                            currentState = AI.Move;
                        }
                    }
                }
                if (idleTime > 3f)
                    idleTime = 0;
                idleTime += Time.deltaTime;
                break;

            case AI.Move:
                navigate.SetDestination(PlayerPos);
                if (PlayerDist <= 12f )
                {
                    if (health <= 10 && AttackRangeCheck(11) && BreathAttackTime > BreathAttackCD)
                    {
                        currentState = AI.BreathAttackStart;
                        anim.SetBool("Move", false);
                    }
                    else if (AttackRangeCheck(5))
                    {
                        currentState = AI.BiteAttack;
                        anim.SetBool("Move", false);
                    }
                        
                }
                else if (PlayerDist <= 5f && AttackRangeCheck(5))
                {
                    currentState = AI.ClawAttack;
                    anim.SetBool("Move", false);
                }
                break;

            case AI.ClawAttack:

                break;
            case AI.BiteAttack:

                break;
            case AI.CastSpell:

                break;
            case AI.BreathAttackStart:

                break;
            case AI.BreathAttackLoop:

                break;
            case AI.BreathAttackEnd:

                break;
            case AI.Summon:
                break;
            case AI.Defend:

                break;
            case AI.TakeDamage:


                break;
            case AI.Die:
                break;
        }
    }

    public void Kill()
    {
        AttackFinished();
        currentState = AI.Die;
    }

    public void AttackFinished()
    {
        weaponScript.AttackEnd();
        currentState = AI.Idle;
    }

    public void AttackStart()
    {
        weaponScript.AttackStart();
    }

    public void BreathStart()
    {
        Breath.StartAttack();
    }

    public void BreathFinished()
    {
        Breath.EndAttack();
        currentState = AI.Idle;
        anim.SetBool("Breath Attack", false);
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
    void OnEnable()
    {
        EventSystem.onPlayerPositionUpdate += UpdateTargetPosistion;
    }

    void OnDisable()
    {
        EventSystem.onPlayerPositionUpdate -= UpdateTargetPosistion;
    }

    void UpdateTargetPosistion(Vector3 pos)
    {
        PlayerPos = pos;
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
    Ray rangeForward;
    Ray rangeLeft;
    Ray rangeRight;
    private string monsterName;

    bool AttackRangeCheck(float range = 10f)
    {
        Debug.DrawRay(transform.position + Vector3.up, transform.forward * range, Color.red);
        Debug.DrawRay(transform.position + Vector3.up, (transform.forward + transform.right * 0.25f) * range, Color.red);
        Debug.DrawRay(transform.position + Vector3.up, (transform.forward - transform.right * 0.25f) * range, Color.red);
        rangeForward = new Ray(transform.position + Vector3.up, transform.forward * range);
        rangeRight = new Ray(transform.position + Vector3.up, (transform.forward - transform.right * 0.25f) * range);
        rangeLeft = new Ray(transform.position + Vector3.up, (transform.forward + transform.right * 0.25f) * range);
        RaycastHit forwardHit;
        RaycastHit leftHit;
        RaycastHit rightHit;

        if (Physics.Raycast(rangeForward, out forwardHit, range) && forwardHit.transform.tag == "Player" || Physics.Raycast(rangeLeft, out leftHit, range) && leftHit.transform.tag == "Player" || Physics.Raycast(rangeRight, out rightHit, range) && rightHit.transform.tag == "Player")
        {
            return true;
        }
        return false;
    }

    public int RemainingHealth()
    {
        return health;
    }

    public void ResetToIdle()
    {
        currentState = AI.Idle;
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
