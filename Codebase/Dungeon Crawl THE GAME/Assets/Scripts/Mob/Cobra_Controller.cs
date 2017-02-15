using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cobra_Controller : MonoBehaviour, IEnemyBehavior {

    

    enum AI
    {
        Idle, Slither, BiteAttack, ProjectileAttack, BreathAttackStart, BreathAttackEnd,BreathAttackLoop, CastSpell, TakeDamage, Die, Wander
            
    }

    AI _cs;
    AI currentState
    {
        get { return _cs; }
        set
        {
            switch(value)
            {
                case AI.Idle:
                    idleTime = 0;
                    navAgent.enabled = false;
                    //You can prevent a state assignment with a check here
                    _cs = value;
                    break;
                case AI.Wander:
                    anim.SetBool("Slither", true);
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
                case AI.BreathAttackEnd:
                   
                    _cs = value;
                    break;
                case AI.BreathAttackLoop:
                    _cs = value;
                    break;
                case AI.BreathAttackStart:
                    idleTime = 0;
                    anim.SetBool("Breath Attack", true);
                    navAgent.speed = 0;
                    navAgent.Stop();
                    //navAgent.enabled = false;
                    idleTime = 0;
                    _cs = value;
                    break;
                case AI.CastSpell:
                    _cs = value;
                    break;
                case AI.Die:
                    dead = true;
                    navAgent.speed = 0;
                    navAgent.enabled = false;
                    anim.SetBool("Die", true);
                    bCollider.enabled = false;
                    EventSystem.ScoreIncrease(pointValue);

                    dead = true;
                    navAgent.speed = 0;
                    navAgent.enabled = false;
                    anim.SetBool("Die", true);
                    bCollider.enabled = false;
                    _cs = value;
                    break;
                case AI.ProjectileAttack:
                    anim.SetBool("Projectile Attack", true);
                    navAgent.speed = 0;
                    navAgent.Stop();
                    _cs = value;
                    break;
                case AI.Slither:
                    idleTime = 0;
                    anim.SetBool("Slither", true);
                    navAgent.enabled = true;
                    navAgent.speed = 3.5f;
                    _cs = value;
                    break;
                case AI.TakeDamage:
                    _cs = value;
                    break;
            }
        }

    }
    //variables
    Animator anim;
    Vector3 targetPos;
    float targetdistance;
    bool poisonBreathCreated = false;

    //wandering variarables;
    Vector3 wanderingSphere;
    Vector3 originPos;
    NavMeshHit navHitPos;



    //Stat variables
    public int health = 2;
    public uint pointValue = 1;
    bool dead = false;


    //References
    NavMeshAgent navAgent;
    Collider AttackRegionCollider;
    BoxCollider bCollider;
    float idleTime = 0;
    public Object projectile;
    public Object poisonbreath;
    GameObject poisonBreath;
    IWeaponBehavior weaponScript;

    // Use this for initialization
    void Start ()
    {
       // weaponScript = mouthGizmo.transform.Find("Attack Collider").gameObject.transform.GetComponent<IWeaponBehavior>();
        bCollider = GetComponent<BoxCollider>();
        anim = GetComponent<Animator>();
        originPos = transform.position;
        navAgent = GetComponent<NavMeshAgent>();
        AttackRegionCollider = GetComponent<Collider>();
        currentState = AI.Idle;
        navHitPos.hit = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        idleTime += Time.deltaTime;
        targetdistance = Vector3.Distance(targetPos, transform.position);
        switch (currentState)
        {
            case AI.Idle:
                {
                    if (idleTime > 1f)
                    {
                        if (targetdistance < 20f)
                            currentState = AI.Slither;
                      //  else if (targetdistance < 8f)
                      //  {
                      //      currentState = BoarState.Walk;
                      //  }
                    }
                    if (idleTime > 3f)
                    {

                        currentState = AI.Wander;
                        navAgent.enabled = true;
                        idleTime = 0;
                    }
                    //idleTime += Time.deltaTime;
                }
                break;
            case AI.Slither:
                {
                    navAgent.SetDestination(targetPos);
                    if (idleTime > 1f)
                    {
                        if (targetdistance < 20f && targetdistance > 15f)
                        {
                            anim.SetBool("Slither", false);
                            currentState = AI.ProjectileAttack;
                        }
                        if (targetdistance<5f && targetdistance >= 2f)
                        {
                            anim.SetBool("Slither", false);
                            currentState = AI.BreathAttackStart;
                        }
                        if (targetdistance<2f)
                        {
                            anim.SetBool("Slither", false);
                            currentState = AI.BiteAttack;
                        }
                    }
                }
                break;
            case AI.BiteAttack:
                currentState = AI.Idle;

                break;
            case AI.ProjectileAttack:
               
                Vector3 position = transform.position + transform.forward * 2f;
                position.y = position.y + 2;
                GameObject PoisonBall = Instantiate(projectile,position,Quaternion.identity) as GameObject;
                PoisonBall.GetComponent<Rigidbody>().AddForce(transform.forward * 700);
                currentState = AI.Idle;
                anim.SetBool("Projectile Attack", false);
                //  
                break;
            case AI.BreathAttackStart:
                currentState = AI.BreathAttackLoop;
                poisonBreathCreated = false;
                idleTime = 0;
                break;
            case AI.BreathAttackEnd:
                if (idleTime > 3f)
                {
                     Destroy(poisonBreath);
                    anim.SetBool("Breath Attack", false);
                    currentState = AI.Idle;
                }
                break;
            case AI.BreathAttackLoop:
                if (idleTime >1f)
                { 
                    Vector3 position1 = transform.position + transform.forward * 2f;
                    position1.y = position1.y + 2;
                    if (poisonBreathCreated == false)
                    {
                        poisonBreath = Instantiate(poisonbreath, position1, Quaternion.identity) as GameObject;
                        poisonBreath.transform.forward = transform.forward;
                        poisonBreath.GetComponent<ParticleSystem>().enableEmission = true;
                        poisonBreath.GetComponent<GenericMeleeDamage>().AttackStart();

                        poisonBreathCreated = true;
                    }
                    currentState = AI.BreathAttackEnd;
               }
                break;
            case AI.CastSpell:
                break;
            case AI.TakeDamage:
                break;
            case AI.Die:
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
                        anim.SetBool("Slither", true);
                    }
                    else if (navAgent.remainingDistance < 2)
                    {
                        navHitPos.hit = true;
                        anim.SetBool("Slither", false);
                        currentState = AI.Idle;
                    }
                    navAgent.SetDestination(navHitPos.position);
                }
                break;
        }
    }
    void OnEnable()
    {
        EventSystem.onPlayerPositionUpdate += UpdateTargetPosition;
    }
    //unsubscribe from player movement
    void OnDisable()
    {
        EventSystem.onPlayerPositionUpdate -= UpdateTargetPosition;
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
            if (health <= 0)
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
}
