using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Snake_Controller : MonoBehaviour, IEnemyBehavior {

    //Score variables
    int pScore;

    enum SnakeState
    {
        Idle, Slither, BiteAttack, ProjectileAttack, BreathAttackStart, BreathAttackEnd, BreathAttackLoop, CastSpell, TakeDamage, Die, Wander

    }
    SnakeState _cs;
    SnakeState currentState
    {
        get { return _cs; }
        set
        {
            switch (value)
            {
                case SnakeState.Idle:
                    idleTime = 0;
                    navAgent.enabled = false;
                    //You can prevent a state assignment with a check here
                    _cs = value;
                    break;
                case SnakeState.Wander:
                    anim.SetBool("Slither", true);
                    navAgent.enabled = true;
                    navAgent.speed = 3.5f;
                    _cs = value;
                    break;
                case SnakeState.BiteAttack:
                    _cs = value;
                    break;
                case SnakeState.BreathAttackEnd:
                    _cs = value;
                    break;
                case SnakeState.BreathAttackLoop:
                    _cs = value;
                    break;
                case SnakeState.BreathAttackStart:
                    _cs = value;
                    break;
                case SnakeState.CastSpell:
                    _cs = value;
                    break;
                case SnakeState.Die:
                    EventSystem.ScoreIncrease(pointValue);

                    _cs = value;
                    break;
                case SnakeState.ProjectileAttack:
                    _cs = value;
                    break;
                case SnakeState.Slither:
                    anim.SetBool("Slither", true);
                    navAgent.enabled = true;
                    navAgent.speed = 3.5f;
                    _cs = value;
                    break;
                case SnakeState.TakeDamage:
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
   // NavMeshHit navHitPos;



    //Stat variables
    int health;
    bool death = false;
    public int pointValue = 1;


    //References
    NavMeshAgent navAgent;

    float idleTime = 0;
    private string monsterName;


    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
        originPos = transform.position;
        navAgent = GetComponent<NavMeshAgent>();
        currentState = SnakeState.Idle;
      //  navHitPos.hit = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        switch (currentState)
        {
            case SnakeState.Idle:
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

                        currentState = SnakeState.Wander;
                        navAgent.enabled = true;
                        idleTime = 0;
                    }
                    idleTime += Time.deltaTime;
                }
                break;
            case SnakeState.Slither:
                {
                    navAgent.SetDestination(targetPos);
                }
                break;
            case SnakeState.BiteAttack:
                break;
            case SnakeState.ProjectileAttack:
                break;
            case SnakeState.BreathAttackStart:
                break;
            case SnakeState.BreathAttackEnd:
                break;
            case SnakeState.BreathAttackLoop:
                break;
            case SnakeState.CastSpell:
                break;
            case SnakeState.TakeDamage:
                break;
            case SnakeState.Die:
                break;
            case SnakeState.Wander:
                {
          //      if (navHitPos.hit == true)
          //      {
          //          navHitPos.hit = false;
          //          float x = originPos.x + (-10 + Random.Range(0, 20));
          //          float z = originPos.z + (-10 + Random.Range(0, 20));
          //          Vector3 randDirection = new Vector3(x, transform.position.y, z);
          //          navHitPos.position = randDirection;
          //          anim.SetBool("Slither", true);
          //      }
          //      else if (navAgent.remainingDistance < 2)
          //      {
          //          navHitPos.hit = true;
          //          anim.SetBool("Slither", false);
          //          currentState = SnakeState.Idle;
          //      }
          //      navAgent.SetDestination(navHitPos.position);
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

    public void TakeDamage(int damage = 1)
    {
        if (!death)
        {
            health -= damage;
            if (health < 1)
            {
                Kill();
                Scoreinc();
            }
            else
            {
                currentState = SnakeState.TakeDamage;
            }
        }
    }

    public void Kill()
    {
        currentState = SnakeState.Die;

    }

    void Scoreinc()
    {
        ++pScore;
    }

    public int RemainingHealth()
    {
        return health;
    }

    public void ResetToIdle()
    {
        currentState = SnakeState.Idle;
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
