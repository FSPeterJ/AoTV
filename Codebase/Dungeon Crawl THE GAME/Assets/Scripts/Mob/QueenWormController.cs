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
    //ParticleSystem particles;
    [SerializeField]
    uint pointValue = 1;
    GameObject weapon;
    IWeaponBehavior weaponScript;
    [SerializeField]
    GameObject Exit;

    [SerializeField]
    int attackRange = 5;

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
                    navigate.speed = 8f;
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
        //particles = GetComponent<ParticleSystem>();
        //particles.Stop();

        weapon = FindWeapon(transform).gameObject;
        weaponScript = weapon.GetComponent<IWeaponBehavior>();

    }

    // Update is called once per frame
    void Update()
    {
        PlayerDist = Vector3.Distance(PlayerPos, transform.position);

        switch (currentState)
        {
            case AI.Idle:
                if (idleTime > 1f)
                {
                    if (PlayerDist <= 5f)
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
                    else if (PlayerDist <= 8f)
                        currentState = AI.BiteAttack;
                    else if (PlayerDist <= 15f)
                    {
                        if (health <= 5)
                            currentState = AI.BreathAttackStart;
                        else
                            currentState = AI.CastSpell;
                    }
                }
                if (idleTime > 3f)
                    idleTime = 0;
                idleTime += Time.deltaTime;
                break;

            case AI.Move:
                navigate.SetDestination(PlayerPos);
                if (PlayerDist <= 4f)
                {
                    currentState = AI.BiteAttack;
                    anim.SetBool("Move", false);
                }
                else if (PlayerDist <= 10f)
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




    public int RemainingHealth()
    {
        return health;
    }

    public void ResetToIdle()
    {
        currentState = AI.Idle;
    }
}
