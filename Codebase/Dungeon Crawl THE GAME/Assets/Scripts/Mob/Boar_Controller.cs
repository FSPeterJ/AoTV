using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boar_Controller : MonoBehaviour, IEnemyBehavior
{
    //Score variables
    int pScore;
    //int sX = Screen.width - 7;
    //int sY = 0;


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
                    navAgent.speed = 25f;
                    _cs = value;
                    break;
                case AI.Walk:
                    anim.SetBool("Walk", true);
                    navAgent.Resume();
                    //navAgent.enabled = true;
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
                case AI.TuskAttack:
                    anim.SetBool("Tusk Attack", true);
                    navAgent.speed = 0;
                    navAgent.Stop();
                    weaponScript.ImpactAttack(true);
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
                default:

                    _cs = value;
                    break;
            }
        }

    }
    public enum AI
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
    bool navPos;
    Vector3 targetDestination;



    //Stat variables
    [SerializeField]
    public int health;
    bool dead = false;
    [SerializeField]
    public int pointValue = 1;


    //References
    NavMeshAgent navAgent;
    BoxCollider bCollider;
    IWeaponBehavior weaponScript;
    GameObject mouthGizmo;
    public AudioClip deathSFX;

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


    // Use this for initialization
    void Start()
    {
        bCollider = GetComponent<BoxCollider>();
        anim = GetComponent<Animator>();
        originPos = transform.position;
        navAgent = GetComponent<NavMeshAgent>();
        mouthGizmo = transform.Find("RigMouthTGizmo").gameObject;
        weaponScript = mouthGizmo.transform.Find("Attack Collider").gameObject.transform.GetComponent<IWeaponBehavior>();
        currentState = AI.Idle;
        navPos = true;
    }

    private void Update()
    {
        targetdistance = Vector3.Distance(targetPos, transform.position);


        //StateMachine
        switch (currentState)
        {
            case AI.Idle:
                {
                    idleTime += Time.deltaTime;
                    //Debug.DrawRay(mouthGizmo.transform.position + Vector3.up, (mouthGizmo.transform.r + mouthGizmo.transform.right).normalized * 4f, Color.red);
                    //Debug.DrawRay(mouthGizmo.transform.position + Vector3.up, (mouthGizmo.transform.forward - mouthGizmo.transform.right).normalized * 4f, Color.red);
                    if (AttackRangeCheck())
                    {
                        currentState = AI.BiteAttack;

                    }
                    else if (idleTime > 1f)
                    {
                        if (targetdistance < 50f && targetdistance > 10f)
                        {
                            currentState = AI.Run;
                        }
                        else if (targetdistance < 8f)
                        {
                            currentState = AI.Walk;
                        }
                        else if (idleTime > 3f)
                        {
                            currentState = AI.Wander;
                            navAgent.enabled = true;
                            idleTime = 0;
                            
                        }
                    }
                }
                break;
            case AI.Wander:
                {

                    if (navPos == true)
                    {
                        navPos = false;
                        float x = originPos.x + (-10 + Random.Range(0, 20));
                        float z = originPos.z + (-10 + Random.Range(0, 20));
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
                    if (AttackRangeCheck())
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
                    if (AttackRangeCheck())
                    {
                        currentState = AI.TuskAttack;
                        anim.SetBool("Run", false);
                    }
                }
                break;
            case AI.BiteAttack:
                {


                }
                break;
            case AI.TuskAttack:
                {

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
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SFX Volume");
        AttackFinished();
        if (!dead)
        {
            health -= damage;
            if (health < 1)
            {
                GetComponent<AudioSource>().PlayOneShot(deathSFX);
                Kill();
                Scoreinc();
            }
            else
            {
                GetComponent<AudioSource>().Play();
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
        targetPos = new Vector3(targetPos.x, 999999, targetPos.z);
    }

    void Scoreinc()
    {
        //Event goes here
    }



    Ray attackRangeForward;
    Ray attackRangeLeft;
    Ray attackRangeRight;
    float attackRange = 5.5f;
    private string monsterName;

    bool AttackRangeCheck()
    {
        //what the hell is this?
        Debug.DrawRay(mouthGizmo.transform.position + Vector3.up, -mouthGizmo.transform.right * attackRange, Color.red);
        Debug.DrawRay(mouthGizmo.transform.position + Vector3.up, (-mouthGizmo.transform.right + mouthGizmo.transform.forward * 0.25f) * attackRange, Color.red);
        Debug.DrawRay(mouthGizmo.transform.position + Vector3.up, (-mouthGizmo.transform.right - mouthGizmo.transform.forward * 0.25f) * attackRange, Color.red);
        attackRangeForward = new Ray(mouthGizmo.transform.position + Vector3.up, -mouthGizmo.transform.right * attackRange);
        attackRangeRight = new Ray(mouthGizmo.transform.position + Vector3.up, (-mouthGizmo.transform.right - mouthGizmo.transform.forward * 0.25f) * attackRange);
        attackRangeLeft = new Ray(mouthGizmo.transform.position + Vector3.up, (-mouthGizmo.transform.right + mouthGizmo.transform.forward * 0.25f) * attackRange);
        RaycastHit forwardHit;
        RaycastHit leftHit;
        RaycastHit rightHit;

        if (Physics.Raycast(attackRangeForward, out forwardHit, attackRange) && forwardHit.transform.tag == "Player" || Physics.Raycast(attackRangeLeft, out leftHit, attackRange) && leftHit.transform.tag == "Player" || Physics.Raycast(attackRangeRight, out rightHit, attackRange) && rightHit.transform.tag == "Player")
        {
            return true;
        }
        return false;
    }


    public string Name()
    {
        return monsterName;
    }

    public float HPOffsetHeight()
    {
        return GetComponentInChildren<Renderer>().bounds.size.y + 1;
    }
}
