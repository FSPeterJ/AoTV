using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cobra_Controller : MonoBehaviour {

    

    enum CobraState
    {
        Idle, Slither, BiteAttack, ProjectileAttack, BreathAttackStart, BreathAttackEnd,BreathAttackLoop, CastSpell, TakeDamage, Die, Wander
            
    }

    CobraState _cs;
    CobraState currentState
    {
        get { return _cs; }
        set
        {
            switch(value)
            {
                case CobraState.Idle:
                    idleTime = 0;
                    navAgent.enabled = false;
                    //You can prevent a state assignment with a check here
                    _cs = value;
                    break;
                case CobraState.Wander:
                    anim.SetBool("Slither", true);
                    navAgent.enabled = true;
                    navAgent.speed = 3.5f;
                    _cs = value;
                    break;
                case CobraState.BiteAttack:
                    anim.SetBool("Bite Attack", true);
                    navAgent.speed = 0;
                    navAgent.Stop();
                    //navAgent.enabled = false;
                    idleTime = 0;
                    _cs = value;
                    break;
                case CobraState.BreathAttackEnd:
                   
                    _cs = value;
                    break;
                case CobraState.BreathAttackLoop:
                    _cs = value;
                    break;
                case CobraState.BreathAttackStart:
                    idleTime = 0;
                    anim.SetBool("Breath Attack", true);
                    navAgent.speed = 0;
                    navAgent.Stop();
                    //navAgent.enabled = false;
                    idleTime = 0;
                    _cs = value;
                    break;
                case CobraState.CastSpell:
                    _cs = value;
                    break;
                case CobraState.Die:
                    EventSystem.ScoreIncrease(pointValue);

                    dead = true;
                    navAgent.speed = 0;
                    navAgent.enabled = false;
                    anim.SetBool("Die", true);
                    bCollider.enabled = false;
                    _cs = value;
                    break;
                case CobraState.ProjectileAttack:
                    anim.SetBool("Projectile Attack", true);
                    navAgent.speed = 0;
                    navAgent.Stop();
                    _cs = value;
                    break;
                case CobraState.Slither:
                    idleTime = 0;
                    anim.SetBool("Slither", true);
                    navAgent.enabled = true;
                    navAgent.speed = 3.5f;
                    _cs = value;
                    break;
                case CobraState.TakeDamage:
                    _cs = value;
                    break;
            }
        }

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
    public uint pointValue = 1;
    bool dead = false;


    //References
    NavMeshAgent navAgent;
    Collider AttackRegionCollider;
    BoxCollider bCollider;
    float idleTime = 0;
    public Object projectile;
    public Object poisonbreath;

	// Use this for initialization
	void Start ()
    {
        bCollider = GetComponent<BoxCollider>();
        anim = GetComponent<Animator>();
        originPos = transform.position;
        navAgent = GetComponent<NavMeshAgent>();
        AttackRegionCollider = GetComponent<Collider>();
        currentState = CobraState.Idle;
        navHitPos.hit = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        idleTime += Time.deltaTime;
        targetdistance = Vector3.Distance(targetPos, transform.position);
        switch (currentState)
        {
            case CobraState.Idle:
                {
                    if (idleTime > 1f)
                    {
                        if (targetdistance < 20f)
                            currentState = CobraState.Slither;
                      //  else if (targetdistance < 8f)
                      //  {
                      //      currentState = BoarState.Walk;
                      //  }
                    }
                    if (idleTime > 3f)
                    {

                        currentState = CobraState.Wander;
                        navAgent.enabled = true;
                        idleTime = 0;
                    }
                    idleTime += Time.deltaTime;
                }
                break;
            case CobraState.Slither:
                {
                    navAgent.SetDestination(targetPos);
                    if (idleTime > 1f)
                    {
                        if (targetdistance < 20f && targetdistance > 15f)
                        {
                            anim.SetBool("Slither", false);
                            currentState = CobraState.ProjectileAttack;
                        }
                        if (targetdistance<5f && targetdistance >= 2f)
                        {
                            anim.SetBool("Slither", false);
                            currentState = CobraState.BreathAttackStart;
                        }
                        if (targetdistance<2f)
                        {
                            anim.SetBool("Slither", false);
                            currentState = CobraState.BiteAttack;
                        }
                    }
                }
                break;
            case CobraState.BiteAttack:
                break;
            case CobraState.ProjectileAttack:
               
                Vector3 position = transform.position + transform.forward * 2f;
                position.y = position.y + 2;
                GameObject PoisonBall = Instantiate(projectile,position,Quaternion.identity) as GameObject;
                PoisonBall.GetComponent<Rigidbody>().AddForce(transform.forward * 700);
                currentState = CobraState.Idle;
                anim.SetBool("Projectile Attack", false);
                //  
                break;
            case CobraState.BreathAttackStart:
                
                Vector3 position1 = transform.position + transform.forward * 2f;
                position1.y = position1.y + 2;
                Instantiate(poisonbreath, position1, Quaternion.identity);
                currentState = CobraState.BreathAttackLoop;
                break;
            case CobraState.BreathAttackEnd:
                Destroy(poisonbreath);
                anim.SetBool("Breath Attack", false);
                currentState = CobraState.Idle;
               
                break;
            case CobraState.BreathAttackLoop:
                if (idleTime > 3f)
                {
                    currentState = CobraState.BreathAttackEnd;
                }
                break;
            case CobraState.CastSpell:
                break;
            case CobraState.TakeDamage:
                break;
            case CobraState.Die:
                break;
            case CobraState.Wander:
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
                        currentState = CobraState.Idle;
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
    void UpdateTargetPosition(Vector3 pos)
    {
        targetPos = pos;
    }

    void Kill()
    {


    }
}
