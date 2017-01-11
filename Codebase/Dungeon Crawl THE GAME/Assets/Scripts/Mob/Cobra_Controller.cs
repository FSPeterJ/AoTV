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
                    _cs = value;
                    break;
                case CobraState.BreathAttackEnd:
                    _cs = value;
                    break;
                case CobraState.BreathAttackLoop:
                    _cs = value;
                    break;
                case CobraState.BreathAttackStart:
                    _cs = value;
                    break;
                case CobraState.CastSpell:
                    _cs = value;
                    break;
                case CobraState.Die:
                    _cs = value;
                    break;
                case CobraState.ProjectileAttack:
                    _cs = value;
                    break;
                case CobraState.Slither:
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
    int health;

    //References
    NavMeshAgent navAgent;
    Collider AttackRegionCollider;

    float idleTime = 0;


	// Use this for initialization
	void Start ()
    {
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

        switch (currentState)
        {
            case CobraState.Idle:
                {
                    if (idleTime > 1f)
                    {
                      //  if (targetdistance < 20f && targetdistance > 10f)
                      //  {
                      //      currentState = BoarState.Run;
                      //  }
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
                }
                break;
            case CobraState.BiteAttack:
                break;
            case CobraState.ProjectileAttack:
                break;
            case CobraState.BreathAttackStart:
                break;
            case CobraState.BreathAttackEnd:
                break;
            case CobraState.BreathAttackLoop:
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
}
