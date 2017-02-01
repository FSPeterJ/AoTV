using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boar_Controller : MonoBehaviour, IEnemyBehavior
{
    //Score variables
    int pScore;
    int sX = Screen.width - 7;
    int sY = 0;

    public HUD hud;

    //Use for executing commands on when first entering a state
    //Can also be used to prevent states from changing under certain conditions
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
                case AI.Jump:
                    _cs = value;
                    break;
                case AI.Run:
                    anim.SetBool("Run", true);
                    navAgent.enabled = true;
                    navAgent.speed = 15f;
                    _cs = value;
                    break;
                case AI.Walk:
                    anim.SetBool("Walk", true);
                    navAgent.enabled = true;
                    navAgent.speed = 3.5f;
                    _cs = value;
                    break;
                case AI.BiteAttack:
                    anim.SetBool("Bite Attack", true);
                    navAgent.speed = 0;
                    navAgent.enabled = false;
                    idleTime = 0;
                    _cs = value;
                    break;
                case AI.TuskAttack:
                    anim.SetBool("Tusk Attack", true);
                    navAgent.speed = 0;
                    navAgent.enabled = false;
                    idleTime = 0;
                    _cs = value;
                    break;
                case AI.TakeDamage:
                    anim.SetBool("Take Damage", true);

                    break;
                case AI.Die:
                    dead = true;
                    navAgent.speed = 0;
                    navAgent.enabled = false;
                    anim.SetBool("Die", true);

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
        Idle, Walk, Jump, Run, BiteAttack, TuskAttack, CastSpell, Defend, TakeDamage, Wander, Die
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
    public int health;
    bool dead = false;

    //References
    NavMeshAgent navAgent;
    IWeaponBehavior weaponScript;

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
        GameObject temp = transform.Find("RigMouthTGizmo").gameObject.transform.Find("Attack Collider").gameObject;
        Debug.Log(temp);
        weaponScript = temp.transform.GetComponent<IWeaponBehavior>();
        currentState = AI.Idle;
        navHitPos.hit = true;
    }

    private void Update()
    {
        targetdistance = Vector3.Distance(targetPos, transform.position);
        //StateMachine
        switch (currentState)
        {
            case AI.Idle:
                {
                    if (idleTime > 1f)
                    {
                        if (targetdistance < 50f && targetdistance > 10f)
                        {
                            currentState = AI.Run;
                        }
                        else if (targetdistance < 8f)
                        {
                            currentState = AI.Walk;
                        }
                    }
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

                    if (navHitPos.hit == true)
                    {
                        navHitPos.hit = false;
                        float x = originPos.x + (-10 + Random.Range(0, 20));
                        float z = originPos.z + (-10 + Random.Range(0, 20));
                        Vector3 randDirection = new Vector3(x, transform.position.y, z);
                        navHitPos.position = randDirection;
                        anim.SetBool("Walk", true);
                        navAgent.SetDestination(navHitPos.position);

                    }
                    else if (navAgent.remainingDistance < 2)
                    {
                        navHitPos.hit = true;
                        anim.SetBool("Walk", false);
                        currentState = AI.Idle;
                    }

                }
                break;
            case AI.Walk:
                {

                    navAgent.SetDestination(targetPos);
                    if (targetdistance < 2.8f)
                    {
                        currentState = AI.BiteAttack;
                        anim.SetBool("Walk", false);
                    }
                    else if (targetdistance < 50f && targetdistance > 10f)
                    {
                        currentState = AI.Run;
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
                    navAgent.SetDestination(targetPos);
                    if (targetdistance < 2.8f)
                    {
                        currentState = AI.TuskAttack;
                        anim.SetBool("Run", false);
                    }
                }
                break;
            case AI.BiteAttack:
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
            case AI.TuskAttack:
                {

                    if (idleTime > 1f)
                    {
                        currentState = AI.Idle;
                        anim.SetBool("Tusk Attack", false);

                    }
                    else
                        idleTime += Time.deltaTime;
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
                idleTime = 0f;
                while (true)
                {
                    if (idleTime > 500f)
                    {
                        break;

                    }
                    else
                        idleTime += Time.deltaTime;
                }
                // Destroy(gameObject);
                break;
        }
    }

    public void ResetToIdle()
    {
        currentState = AI.Idle;

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
                Scoreinc();
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

    void UpdateTargetPosition(Vector3 pos)
    {
        targetPos = pos;
    }

    void PlayerDied()
    {
        EventSystem.onPlayerPositionUpdate -= UpdateTargetPosition;
        targetPos = new Vector3(targetPos.x, 999999, targetPos.z);
    }

    void Scoreinc()
    {
        hud.UpdateScore();
    }
}
