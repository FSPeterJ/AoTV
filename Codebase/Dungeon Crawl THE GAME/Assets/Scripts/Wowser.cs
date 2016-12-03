using UnityEngine;
using System.Collections;


public enum BossStates
{
    Idle,
    Moving,
    Stomp,
    FireBreath,
    Stunned

}

public class Wowser : MonoBehaviour
{

    float tail;
    bool IsIdle = true;
    bool IsStunned = false;
    bool lowHP = false;
    float timeElapsed = 0.0f;
    int bHealth = 3;
    Vector3 Position;
    float dist = 0;
    public BossStates CurrentState = BossStates.Idle;
    public GameObject Mario;
    public Collider wowser;
    public GameObject arena;
    public GameObject Bomb; void Start()
    {

    }

    void Update()
    {

        dist = Vector3.Distance(transform.position, Mario.transform.position);

        Position = transform.position;
        switch (CurrentState)
        {
            case BossStates.Idle:
                IdleState();
                break;
            case BossStates.Moving:
                MovingState();
                break;
            case BossStates.Stomp:
                StompState();
                break;
            case BossStates.FireBreath:
                FirebreathState();
                break;
            case BossStates.Stunned:
                StunndedState();
                break;
            default:
                break;
        }
    }
    void IdleState()
    {

        GetComponent<NavMeshAgent>().updatePosition = false;
        GetComponent<NavMeshAgent>().Warp(transform.position);

        GetComponent<NavMeshAgent>().SetDestination(Mario.transform.position);


        if (dist > 5f)
            Mov();
        //CurrentState = BossStates.Moving;
        //GetComponent<NavMeshAgent>().;
        //controls duration of IdleState // change hard coded 1 eventually
        if (timeElapsed > 1)
        {
            //CurrentState = BossStates.Moving;
            timeElapsed = 0;
        }

        while (timeElapsed < 1)
        {
            timeElapsed += Time.deltaTime;
            continue;
        }


    }

    private void Mov()
    {
        CurrentState = BossStates.Moving;
        GetComponent<NavMeshAgent>().Warp(transform.position);
        GetComponent<NavMeshAgent>().updatePosition = true;
    }
    void MovingState()
    {

        //GetComponent<NavMeshAgent>().updatePosition = true;
        if (dist < 5f)
            CurrentState = BossStates.Idle;

        GetComponent<NavMeshAgent>().SetDestination(Mario.transform.position);

        Debug.Log(dist);
        Debug.Log(CurrentState.ToString());

    }
    void StompState()
    {
        //int r = 1;
        while (timeElapsed < 1.5)
        {
            timeElapsed += Time.deltaTime;
        }
        if (timeElapsed >= 1.5)
        {

            if (true)
            { }
            //stomp circle code here
        }



    }
    void FirebreathState()
    {


    }
    void StunndedState()
    {

        int stunDuration = 3;
        if (timeElapsed > stunDuration)
        {
            CurrentState = BossStates.Moving;
            timeElapsed = 0;
        }

        while (timeElapsed < 1)
        {
            timeElapsed += Time.deltaTime;
            continue;
        }

    }

    void Follow()
    {
        arena.GetComponent<NavMeshAgent>();

        while (CurrentState == BossStates.Idle)
        {


        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Explosive")
        {
            --bHealth;
            Destroy(col.gameObject);
        }
    }

    void DeadDamage()
    {
        while (CurrentState == BossStates.FireBreath)
        {
            
        }
    }
    public void SetCurrentState(BossStates NewState)
    {
        CurrentState = NewState;
    }

}