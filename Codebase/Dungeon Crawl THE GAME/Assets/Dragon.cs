using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dragon : MonoBehaviour, IEnemyBehavior
{
    enum Dragon_States
    {
       Idle, Bite, Walk, Firebreath, Die, Takedamage, Fly_Idle, Fly_Bite, Fly_Firebreath, Fly_Forward
    }

    [SerializeField]
    Dragon_States currentstate;
    Dragon_States CurrentState
    {
        get { return currentstate; }

        set
        {
            switch(value)
            {
                case Dragon_States.Idle:
                    AttkAreaCollider.enabled = true;
                    Anim.SetBool("Idle", true);
                    NavAgent.Stop();
                    NavAgent.speed = 0;
                    currentstate = value;
                    break;
                case Dragon_States.Bite:
                    AttkAreaCollider.enabled = true;
                    Anim.SetTrigger("Bite attack");
                    NavAgent.Stop();
                    NavAgent.speed = 0;
                    currentstate = value;
                    break;
                case Dragon_States.Walk:
                    AttkAreaCollider.enabled = true;
                    Anim.SetBool("Walk", true);
                    NavAgent.Resume();
                    NavAgent.speed = 5.0f;
                    currentstate = value;
                    break;
                case Dragon_States.Firebreath:
                    AttkAreaCollider.enabled = true;
                    Anim.SetTrigger("Fire Breath Attack");
                    NavAgent.Stop();
                    idleTime = 0;
                    NavAgent.speed = 0;
                    currentstate = value;
                    break;
                case Dragon_States.Die:
                    Dead = true;
                    GetComponent<BoxCollider>().enabled = false;
                    Anim.SetBool("Die", true);
                    Destroy(gameObject);
                    break;
                case Dragon_States.Takedamage:
                    Anim.SetBool("Take Damage", true);
                    break;
                case Dragon_States.Fly_Idle:
                    Anim.SetBool("Fly Idle", true);
                    NavAgent.Stop();
                    NavAgent.speed = 0;
                    currentstate = value;
                    break;
                case Dragon_States.Fly_Bite:
                    NavAgent.Stop();
                    NavAgent.speed = 0;
                    Anim.SetTrigger("Fly Bite Attack");
                    currentstate = value;
                    break;
                case Dragon_States.Fly_Firebreath:
                    Anim.SetTrigger("Fly Fire Breath Attack");
                    idleTime = 0;
                    NavAgent.Resume();
                    NavAgent.speed = 5.0f;
                    currentstate = value;
                    break;
                case Dragon_States.Fly_Forward:
                    Anim.SetBool("Fly Forward", true);
                    NavAgent.Resume();
                    NavAgent.speed = 5.0f;
                    currentstate = value;
                    break;
                default:
                    break;
            }
        }
    }

   


    

    //variables
    Animator Anim;
    Vector3 Targetposition;
    float TargetDist;
    float groundTime = 0;
    float airTime = 0;

    //wandering variables
    //Vector3 wanderSphere;
    Vector3 Originpos;
    NavMeshHit NavhitPos;

    //Stat Variables
    public int HP = 50;
    bool Dead = false;

    //References
    NavMeshAgent NavAgent;
    BoxCollider AttkAreaCollider;

    float idleTime = 0;

    void OnEnable()
    {
        EventSystem.onPlayerPositionUpdate += UpdatetargetPos;
    }

    void OnDisable()
    {
        EventSystem.onPlayerPositionUpdate -= UpdatetargetPos;
    }

	// Use this for initialization
	void Start()
    {
        Anim = GetComponent<Animator>();
        Originpos = transform.position;
        NavAgent = GetComponent<NavMeshAgent>();
        AttkAreaCollider = GetComponent<BoxCollider>();
        currentstate = Dragon_States.Idle;
        NavhitPos.hit = true;
	}
	
	// Update is called once per frame
	void Update()
    {
        TargetDist = Vector3.Distance(Targetposition, transform.position);
        //Debug.Log("Target distance:" + TargetDist);

        //State machine
        switch (currentstate)
        {
            case Dragon_States.Idle:
                if (idleTime > 1)
                {
                    if (groundTime < 10)
                    {
                        if (TargetDist < 3f)
                        {
                            currentstate = Dragon_States.Bite;
                            Anim.SetTrigger("Bite Attack");
                            Anim.SetBool("Idle", false);
                        }
                        else if (TargetDist > 3f && TargetDist < 5.0f)
                        {
                            currentstate = Dragon_States.Firebreath;
                            Anim.SetTrigger("Fire Breath Attack");
                            Anim.SetBool("Idle", false);
                        }
                        else if (TargetDist > 5.0f && TargetDist < 10.0f)
                        {
                            currentstate = Dragon_States.Walk;
                            Anim.SetBool("Walk", true);
                            NavAgent.enabled = true;
                            Anim.SetBool("Idle", false);
                        }
                    }
                    groundTime += Time.deltaTime;
                }
                idleTime += Time.deltaTime;
                //Debug.Log("idle time:" + idleTime);
                break;
            case Dragon_States.Bite:
               if (groundTime < 10)
                {
                    if (TargetDist > 3f && TargetDist < 5.0f)
                    {
                        //Debug.Log("Dragon_States.Firebreath from Dragon_States.Bite");
                        currentstate = Dragon_States.Idle;
                        Anim.SetBool("Idle", true);
                    }
                    else if (TargetDist > 5.0f && TargetDist < 10.0f)
                    {
                        //Debug.Log("Dragon_States.Walk from Dragon_States.Bite");
                        currentstate = Dragon_States.Idle;
                        Anim.SetBool("Idle", true);
                    }
                    groundTime += Time.deltaTime;
                }
                break;
            case Dragon_States.Walk:
                if (groundTime < 10)
                {
                    if (TargetDist > 3f && TargetDist < 5.0f)
                    {
                        //Debug.Log("Dragon_States.Firebreath from Dragon_States.Walk");
                        currentstate = Dragon_States.Idle;
                        Anim.SetBool("Idle", true);
                        Anim.SetBool("Walk", false);
                    }
                    else if (TargetDist < 3f)
                    {
                        //Debug.Log("Dragon_States.Bite from Dragon_States.Walk");
                        currentstate = Dragon_States.Idle;
                        Anim.SetBool("Idle", true);
                        Anim.SetBool("Walk", false);
                    }
                    groundTime += Time.deltaTime;
                }
                
                break;
            case Dragon_States.Firebreath:
                if (groundTime < 10)
                {
                    if (idleTime > 2)
                    {
                        if (TargetDist > 5.0f && TargetDist < 10.0f)
                        {
                            currentstate = Dragon_States.Idle;
                            Anim.SetBool("Idle", true);
                        }
                        else if (TargetDist < 2.8f)
                        {
                            currentstate = Dragon_States.Idle;
                            Anim.SetBool("Idle", true);
                        }
                    }
                    idleTime += Time.deltaTime;
                }
                    groundTime += Time.deltaTime;
                break;
            case Dragon_States.Die:
                break;
            case Dragon_States.Takedamage:
                break;
            case Dragon_States.Fly_Idle:
                if (airTime < 10)
                {
                    if (TargetDist > 2.8f && TargetDist < 5.0f)
                    {
                        currentstate = Dragon_States.Fly_Firebreath;
                        Anim.SetBool("Fly Idle", false);
                    }
                    if (TargetDist > 5.0f && TargetDist < 10.0f)
                    {
                        currentstate = Dragon_States.Fly_Forward;
                        Anim.SetBool("Fly Idle", false);
                    }
                    if (TargetDist < 2.8f)
                    {
                        currentstate = Dragon_States.Fly_Bite;
                        Anim.SetBool("Fly Idle", false);
                    }
                }
                airTime += Time.deltaTime;
                break;
            case Dragon_States.Fly_Bite:
                if (airTime < 10)
                {
                    if (TargetDist > 2.8f && TargetDist < 5.0f)
                    {
                        currentstate = Dragon_States.Fly_Idle;
                    }
                    else if (TargetDist > 5.0f && TargetDist < 10.0f)
                    {
                        currentstate = Dragon_States.Fly_Idle;
                    }
                    else if (TargetDist < 2.8f)
                    {
                        currentstate = Dragon_States.Fly_Idle;
                    }
                }
                airTime += Time.deltaTime;
                break;
            case Dragon_States.Fly_Firebreath:
                if (airTime < 10)
                {
                    if (idleTime <= 2)
                    {
                        if (TargetDist > 2.8f && TargetDist < 5.0f)
                        {
                            currentstate = Dragon_States.Fly_Idle;
                        }
                        else if (TargetDist > 5.0f && TargetDist < 10.0f)
                        {
                            currentstate = Dragon_States.Fly_Idle;
                        }
                        else if (TargetDist < 2.8f)
                        {
                            currentstate = Dragon_States.Fly_Idle;
                        }
                    }
                }
                airTime += Time.deltaTime;
                idleTime += Time.deltaTime;
                break;
            case Dragon_States.Fly_Forward:
                if (airTime < 10)
                {
                    if (TargetDist > 2.8f && TargetDist < 5.0f)
                    {
                        currentstate = Dragon_States.Fly_Idle;
                        Anim.SetBool("Fly Forward", false);
                    }
                    else if (TargetDist < 2.8f)
                    {
                        currentstate = Dragon_States.Fly_Idle;
                        Anim.SetBool("Fly Forward", false);
                    }
                    else if (TargetDist > 5.0f && TargetDist < 10.0f)
                    {
                        currentstate = Dragon_States.Fly_Idle;
                    }
                }
                break;
            default:
                break;
        }

        if (groundTime >= 10)
            currentstate = Dragon_States.Fly_Idle;

    }
    
    void UpdatetargetPos(Vector3 Pos)
    {
        Targetposition = Pos;
    }

    public void Kill()
    {
        currentstate = Dragon_States.Die;
    }

    public void TakeDamage(int damage  = 1)
    {
        if (!Dead)
        {
            HP -= damage;

            if (HP < 1)
            {
                Kill();
            }
            else
            {
                currentstate = Dragon_States.Takedamage;
            }
        }
    }

    public void ResetToIdle()
    {
        currentstate = Dragon_States.Walk;
    }

    public int RemainingHealth()
    {
        return HP;
    }

}
