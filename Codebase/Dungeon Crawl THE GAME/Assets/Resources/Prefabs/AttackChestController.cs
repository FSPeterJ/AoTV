using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AttackChestController : MonoBehaviour, IEnemyBehavior
{
    //Use for executing commands on when first entering a state
    //Can also be used to prevent states from changing under certain conditions
    public AI _cs;
    public AI currentState
    {
        get { return _cs; }
        set
        {
            switch (value)
            {
                case AI.Idle:
                    navAgent.speed = 0;
                    navAgent.Stop();
                    //navAgent.
                    idleTime = 0;
                    //navAgent.enabled = false;
                    //You can prevent a state assignment with a check here
                    _cs = value;
                    break;
                case AI.Wander:
                    anim.SetBool("Walk", true);
                    navAgent.Resume();
                    //navAgent.enabled = true;
                    navAgent.speed = 3.5f;
                    _cs = value;
                    break;
                case AI.Jump:
                    _cs = value;
                    break;
                case AI.Run:
                    anim.SetBool("Run", true);
                    navAgent.Resume();
                    //navAgent.enabled = true;
                    navAgent.speed = 15f;
                    _cs = value;
                    break;
                case AI.Walk:
                    anim.SetBool("Walk", true);
                    navAgent.Resume();
                    navAgent.enabled = true;
                    navAgent.speed = 3.5f;
                    _cs = value;
                    break;
                case AI.BiteAttack:
                    anim.SetBool("Bite Attack", true);
                    navAgent.speed = 0;
                    navAgent.Stop();
                    //navAgent.enabled = false;
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
                    bCollider.enabled = false;
                    EventSystem.ScoreIncrease(pointValue);

                    _cs = value;
                    break;
                case AI.Rest:
                    navAgent.speed = 0;
                    navAgent.enabled = false;
                    anim.SetBool("Rest", true);
                    _cs = value;
                    break;
                default:

                    _cs = value;
                    break;
            }
        }

    }
    public enum AI
    {
        Idle, Walk, Jump, Run, BiteAttack, CastSpell, Defend, TakeDamage, Wander, Die, Rest
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
    [SerializeField]
    public int health=4;
    bool dead = false;
    [SerializeField]
    public uint pointValue = 1;


    //References
    NavMeshAgent navAgent;
    BoxCollider bCollider;
    IWeaponBehavior weaponScript;
    GameObject mouthGizmo;

    float idleTime = 0;


    void OnEnable()
    {
        EventSystem.onPlayerPositionUpdate += UpdateTargetPosition;
        EventSystem.onPlayerDeath += PlayerDied;
    }
    //unsubscribe from player movement
    void OnDisable()
    {
        EventSystem.onPlayerPositionUpdate -= UpdateTargetPosition;
        EventSystem.onPlayerDeath -= PlayerDied;
    }
    public void ResetToIdle()
    {
        currentState = AI.Idle;

    }
    public void TakeDamage(int damage = 1)
    {
        if(currentState == AI.Rest)
        {
            currentState = AI.Idle;
        }
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
    public int RemainingHealth()
    {
        return health;
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

    void UpdateTargetPosition(Vector3 pos)
    {
        targetPos = pos;
    }
    void PlayerDied()
    {
        EventSystem.onPlayerPositionUpdate -= UpdateTargetPosition;
        targetPos = new Vector3(targetPos.x, 999999, targetPos.z);
    }
    void Start()
    {
        bCollider = GetComponent<BoxCollider>();
        anim = GetComponent<Animator>();
        originPos = transform.position;
        navAgent = GetComponent<NavMeshAgent>();
        mouthGizmo = transform.Find("RigMouthTGizmo").gameObject;
        weaponScript = mouthGizmo.transform.Find("Attack Collider").gameObject.transform.GetComponent<IWeaponBehavior>();
        currentState = AI.Rest;
        navHitPos.hit = true;
    }
    void Update()
    {
        targetdistance = Vector3.Distance(targetPos, transform.position);
        idleTime += Time.deltaTime;
        switch (currentState)
        {
            case AI.Idle:
                currentState = AI.Rest;
                break;
            case AI.Walk:
                navAgent.SetDestination(targetPos);
                if (targetdistance < 4f)
                {
                    anim.SetBool("Walk", false);
                    idleTime = 0;
                    currentState = AI.BiteAttack;
                }
                break;
            case AI.Jump:
                break;
            case AI.Run:
                break;
            case AI.BiteAttack:
                if (idleTime > 1f)
                {
                    currentState = AI.Walk;
                }
                break;
            case AI.CastSpell:
                break;
            case AI.Defend:
                break;
            case AI.TakeDamage:
                break;
            case AI.Wander:
                break;
            case AI.Die:
                break;
            case AI.Rest:
                    if (health != 4)
                    {
                    anim.SetBool("Rest", false);
                    currentState = AI.Walk;
                        idleTime = 0;
                    }
                break;
            default:
                break;
        }
    }
    void OnTriggerStay(Collider Col)
    {
        if (Col.tag =="Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Triggered Brah!!!");
                currentState = AI.BiteAttack;
                anim.SetBool("Rest", false);
                //kill player here
            }
        }
    }
}
