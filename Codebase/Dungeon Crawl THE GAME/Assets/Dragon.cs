﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dragon : MonoBehaviour, IEnemyBehavior
{   
    Ground_States cs;
    Ground_States CurrentgStates
    {
        get { return cs; }

        set
        {
            switch(value)
            {
                case Ground_States.Bite:
                    AttkAreaCollider.enabled = true;
                    Anim.SetBool("Bite", true);
                    idleTime = 0;
                    NavAgent.enabled = false;
                    cs = value;
                    break;

                case Ground_States.Spell:
                    Anim.SetBool("Cast Spell", true);
                    NavAgent.enabled = false;
                    NavAgent.speed = 0;
                    idleTime = 0;
                    cs = value;
                    break;
                case Ground_States.Die:
                    Dead = true;
                    GetComponent<BoxCollider>().enabled = false;
                    Anim.SetBool("Die", true);
                    Destroy(gameObject);
                    break;
                case Ground_States.Takedamage:
                    Anim.SetBool("TakeDamage", true);
                    break;
                case Ground_States.FireBreath:
                    AttkAreaCollider.enabled = true;
                    Anim.SetBool("Fire Breath", true);
                    NavAgent.enabled = false;
                    idleTime = 0;
                    NavAgent.speed = 0;
                    cs = value;
                    break;
                case Ground_States.Idle:
                    idleTime = 0;
                    NavAgent.enabled = false;
                    cs = value;
                    break;
                case Ground_States.Projectileattack:
                    AttkAreaCollider.enabled = true;
                    NavAgent.enabled = false;
                    NavAgent.speed = 0;
                    idleTime = 0;
                    Anim.SetBool("Projectile Attack", true);
                    cs = value;
                    break;
                case Ground_States.Run:
                    NavAgent.speed = 10.0f;
                    NavAgent.enabled = true;
                    Anim.SetBool("Run", true);
                    cs = value;
                    break;
                case Ground_States.Walk:
                    Anim.SetBool("Walk", true);
                    NavAgent.enabled = true;
                    NavAgent.speed = 5.0f;
                    cs = value;
                    break;
                default:
                    cs = value;
                    break;
            }
        }
    }

    enum Ground_States
    {
        Bite, Spell, Die, FireBreath, Idle, Projectileattack, Run, Takedamage, Walk
    }

    Fly_States _Cs;
    Fly_States CurrentfStates
    {
        get { return _Cs; }

        set
        {
            switch(value)
            {
                case Fly_States.Bite:
                    //flystates = true;
                    AttkAreaCollider.enabled = true;
                    NavAgent.enabled = false;
                    NavAgent.speed = 0;
                    idleTime = 0;
                    Anim.SetBool("Bite", true);
                    _Cs = value;
                    break;
                case Fly_States.Spell:
                    //flystates = true;
                    AttkAreaCollider.enabled = true;
                    NavAgent.enabled = false;
                    NavAgent.speed = 0;
                    idleTime = 0;
                    Anim.SetBool("Spell", true);
                    _Cs = value;
                    break;
                case Fly_States.Die:
                    Dead = true;
                    GetComponent<BoxCollider>().enabled = false;
                    Anim.SetBool("Die", true);
                    Destroy(gameObject);
                    break;
                case Fly_States.FireBreath:
                    //flystates = true;
                    idleTime = 0;
                    NavAgent.enabled = true;
                    NavAgent.speed = 0;
                    AttkAreaCollider.enabled = true;
                    Anim.SetBool("Fire breath", true);
                    _Cs = value;
                    break;
                case Fly_States.Forward:
                    //flystates = true;
                    Originpos.x++;
                    NavAgent.enabled = true;
                    NavAgent.speed = 5.0f;
                    AttkAreaCollider.enabled = true;
                    Anim.SetBool("Forward", true);
                    _Cs = value;
                    break;
                case Fly_States.Idle:
                    //flystates = true;
                    idleTime = 0;
                    NavAgent.enabled = false;
                    NavAgent.speed = 0;
                    AttkAreaCollider.enabled = true;
                    Anim.SetBool("idle", true);
                    _Cs = value;
                    break;
                case Fly_States.ProjectileAttack:
                    //flystates = true;
                    idleTime = 0;
                    NavAgent.enabled = false;
                    NavAgent.speed = 0;
                    Anim.SetBool("Flying Projectile Attack", true);
                    _Cs = value;
                    break;
                case Fly_States.TakeDamage:
                    //flystates = true;
                    Anim.SetBool("take damage", true);
                    _Cs = value;
                    break;
            }
        }
    }


    enum Fly_States
    {
        Bite, Spell, Die, FireBreath, Forward, Idle, ProjectileAttack, TakeDamage
    }

    //variables
    Animator Anim;
    Vector3 Targetposition;
    float TargetDist;
    bool flystates = false;

    //wamdering variables
    Vector3 wanderSphere;
    Vector3 Originpos;
    NavMeshHit NavhitPos;
    Vector3 cPos;

    //Stat Variables
    public int HP = 100;
    bool Dead = false;

    //References
    NavMeshAgent NavAgent;
    Collider AttkAreaCollider;

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
        AttkAreaCollider = GetComponent<Collider>();
        CurrentgStates = Ground_States.Idle;
        NavhitPos.hit = true;
        cPos = transform.position;
	}
	
	// Update is called once per frame
	void Update()
    {
        TargetDist = Vector3.Distance(Targetposition, transform.position);

        //Ground State machine
        switch(CurrentgStates)
        {
            case Ground_States.Idle:
                {
                    if (idleTime > 1f)
                    {
                        if (TargetDist < 20f && TargetDist > 10f)
                        {
                            CurrentgStates = Ground_States.Run;
                        }
                        else if (TargetDist < 8f)
                        {
                            CurrentgStates = Ground_States.Walk;
                        }
                    }

                    if (idleTime > 3f)
                    {
                        CurrentgStates = Ground_States.Walk;
                        NavAgent.enabled = true;
                        idleTime = 0;
                    }
                    idleTime += Time.deltaTime;
                }
                break;
            case Ground_States.Bite:
                {
                    if (idleTime > 1f)
                    {
                        CurrentgStates = Ground_States.Idle;
                        Anim.SetBool("Bite", false);
                        AttkAreaCollider.enabled = false;
                    }
                    else
                    {
                        idleTime += Time.deltaTime;
                    }
                    break;
                }
            case Ground_States.Walk:
                {
                    NavAgent.SetDestination(Targetposition);

                    if (TargetDist < 1.0f)
                    {
                        CurrentgStates = Ground_States.Bite;
                        Anim.SetBool("Walk", false);
                    }
                    else if (TargetDist < 20f && TargetDist > 10f)
                    {
                        CurrentgStates = Ground_States.Run;
                        Anim.SetBool("Walk", false);
                    }

                    if (TargetDist > 1.0f && TargetDist < 8.0f)
                    {
                        CurrentgStates = Ground_States.FireBreath;
                        Anim.SetBool("Run", false);
                    }
                    break;
                }
            case Ground_States.Run:
                {
                    NavAgent.SetDestination(Targetposition);

                    if (TargetDist < 1.8f)
                    {
                        CurrentgStates = Ground_States.Bite;
                        Anim.SetBool("Run", false);
                    }

                    if (TargetDist > 1.0f && TargetDist < 8.0f)
                    {
                        CurrentgStates = Ground_States.FireBreath;
                        Anim.SetBool("Run", false);
                    }
                    break;
                }
            case Ground_States.FireBreath:
                {
                    if (TargetDist > 1.0f && TargetDist < 8.0f)
                    {
                        CurrentgStates = Ground_States.Run;
                        Anim.SetBool("Firebreath", false);
                    }
                    else if (TargetDist >= 8.0f)
                    {
                        CurrentgStates = Ground_States.Walk;
                        Anim.SetBool("Firebreath", false);
                    }
                    else if (TargetDist > 10.0f)
                    {
                        CurrentgStates = Ground_States.Idle;
                        Anim.SetBool("Firebreath", false);
                    }
                    break;
                }
        }
        
        switch(CurrentfStates)
        {
            case Fly_States.Bite:
                {
                    if (idleTime > 1f)
                    {
                        CurrentfStates = Fly_States.Idle;
                        Anim.SetBool("Bite", false);
                        AttkAreaCollider.enabled = true;
                    }
                    else
                    {
                        idleTime += Time.deltaTime;
                    }
                    break;
                }
            case Fly_States.FireBreath:
                {
                    if (TargetDist > 10.0f && TargetDist < 15.0f)
                    {
                        CurrentfStates = Fly_States.ProjectileAttack;
                        Anim.SetBool("Firebreath", false);
                    }
                    else if (TargetDist > 10.0f)
                    {
                        CurrentfStates = Fly_States.Forward;
                        Anim.SetBool("Firebreath", false);
                    }
                    else if (TargetDist > 10.0f)
                    {
                        CurrentfStates = Fly_States.Idle;
                        Anim.SetBool("Firebreath", false);
                    }
                    break;
                }
            case Fly_States.Forward:
                {
                    if (TargetDist < 10.0f && TargetDist > 5.0f)
                    {
                        CurrentfStates = Fly_States.FireBreath;
                        Anim.SetBool("Forward", false);
                    }
                    else if (TargetDist < 1.0f)
                    {
                        CurrentfStates = Fly_States.Bite;
                        Anim.SetBool("Forward", false);
                    }
                    else if (TargetDist > 15.0f)
                    {
                        CurrentfStates = Fly_States.Idle;
                        Anim.SetBool("Forward", false);
                    }
                    break;
                }
            case Fly_States.Idle:
                {
                    if (idleTime > 1f)
                    {
                        if (TargetDist < 20f && TargetDist > 10f)
                        {
                            CurrentfStates = Fly_States.Forward;
                            Anim.SetBool("Idle", false);
                        }
                        else if (TargetDist < 10f)
                        {
                            CurrentfStates = Fly_States.FireBreath;
                            Anim.SetBool("Idle", false);
                        }
                    }
                    idleTime += Time.deltaTime;
                }
                break;
            case Fly_States.ProjectileAttack:
                {
                    if (TargetDist < 10.0f && TargetDist > 5.0f)
                    {
                        CurrentfStates = Fly_States.FireBreath;
                        Anim.SetBool("Projectile Attack", false);
                    }
                    break;
                }
            

        }

        if (HP <= 50)
        {
            flystates = true;
            if (flystates == true)
                cPos.y += 5;
        }
    }
    
    void UpdatetargetPos(Vector3 Pos)
    {
        Targetposition = Pos;
    }

    public void Kill()
    {
        CurrentgStates = Ground_States.Die;
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
                CurrentgStates = Ground_States.Takedamage;
            }
        }
    }

    public void ResetToIdle()
    {
        CurrentgStates = Ground_States.Idle;
    }

    public int RemainingHealth()
    {
        return HP;
    }

}
