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
    bool isCoroutineExecuting = false;
    float starttime = 0;
   float timeElapsed = 0.0f;
    int bHealth = 3;
    Vector3 Position;
    float dist = 0;
    bool ResetTime;
    public BossStates CurrentState = BossStates.Idle;
    public GameObject Mario;
    public Collider wowser;
    public GameObject arena;
    //public GameObject Bomb;
    public BasePlayer mariocontroller;

    void Start()
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



        //controls duration of IdleState // change hard coded 1 eventually
        if (timeElapsed > 1)

     
       //controls duration of IdleState // change hard coded 1 eventually
        if (timeElapsed>5)

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


    void MovingState()
    {

        //   
        //if (dist < 5f )
        if (GetComponent<NavMeshAgent>().remainingDistance < 4f)
        {

        GetComponent<NavMeshAgent>().enabled = true;

        //   
        //if (dist < 5f )
        if (GetComponent<NavMeshAgent>().remainingDistance < 4f)
            {

            GetComponent<NavMeshAgent>().speed = 0.5f;
            //    GetComponent<NavMeshAgent>().Warp(transform.position);

        }
        else if (GetComponent<NavMeshAgent>().remainingDistance >= 5f)
        {
            GetComponent<NavMeshAgent>().speed = 3;
            //   GetComponent<NavMeshAgent>().Warp(transform.position);

        }
        GetComponent<NavMeshAgent>().SetDestination(Mario.transform.position);
    }
    void StompState()
    {

    }
    void FirebreathState()
    {
    //    starttime = Time.time;
    //    timeElapsed = 0;
       
       
       
      

            StartCoroutine(PreFireBreath(0.3f));

        //     if (GetComponentInChildren<TriggerFireEvent>().OnCollisionStay(Mario.GetComponent<Collider>()) == true)//oncollisionstay = triggered
        //     {
        //         Mario.GetComponent<BasePlayer>().TakeDamage();
        //     }
        // }
        isCoroutineExecuting = false;

            StartCoroutine(PostFireBreath(1.0f));
          
   


    }
    void StunndedState()
    {

        int stunDuration = 3;
        if (timeElapsed > stunDuration)
        {
            CurrentState = BossStates.Moving;
            timeElapsed = 0;
        }
        Mario.GetComponent<BasePlayer>().TakeDamage();
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
    public void SetCurrentState(BossStates NewState)
    {
        CurrentState = NewState;
    }
    void ResetTimeElapsed()
    {
        timeElapsed = 0;
    }
    IEnumerator PostFireBreath(float seconds)
    {
        if (isCoroutineExecuting)
            yield break;
        isCoroutineExecuting = true;
        
        yield return new WaitForSeconds(seconds);
        GetComponentInChildren<TriggerFireEvent>().DisableParticleSystem();
        Debug.Log("While loop broken");
        CurrentState = BossStates.Idle;



        isCoroutineExecuting = false;
    }
    IEnumerator PreFireBreath(float seconds)
    {
        if (isCoroutineExecuting)
            yield break;
        isCoroutineExecuting = true;
        yield return new WaitForSeconds(seconds);

        GetComponentInChildren<TriggerFireEvent>().EnableParticleSystem();

        isCoroutineExecuting = false;
    }
}