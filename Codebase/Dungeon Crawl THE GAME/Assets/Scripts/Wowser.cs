using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public enum BossStates
{
    Idle,
    Moving,
    Stomp,
    FireBreath,
    Stunned,
    Charge
}

public class Wowser : MonoBehaviour
{
    bool isCoroutineExecutingone = false;
    bool isCoroutineExecuting = false;
    public float timeToDodgeFire = 1;
    float timeElapsed = 0.0f;
    int bHealth = 3;
    public bool isFireBreath = false;
    bool ResetTime;
    public BossStates CurrentState = BossStates.Idle;
    public GameObject Mario;
    public Collider wowser;
    public GameObject arena;
    public BasePlayer mariocontroller;
    private Vector3 moveDirection = Vector3.zero;

    NavMeshAgent Nav;

    void Start()
    {
        Nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (bHealth <= 0)
        {
            SceneManager.LoadScene("KillWowser");
        }

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
            case BossStates.Charge:
                ChargeState();
                break;
            default:
                break;
        }
    }



    //clean upppppp
    bool hasoccured;
    float starttime;
    float distance;
    Vector3 MarioPos;
    //
    private void ChargeState()
    {

        if (hasoccured == true)
        {
            Nav.enabled = false;
            GetComponentInChildren<Rigidbody>().isKinematic = false;
            distance = Vector3.Distance(transform.position, MarioPos);
            MarioPos = Mario.transform.position;
            hasoccured = false;
             starttime= Time.time;
        }
        //   Vector3.Distance(transform.position, MarioPos)
        //   float step = 10 * Time.deltaTime;
        //  Vector3.MoveTowards(transform.position, MarioPos,);
        float dist = (Time.time - starttime) * 10;
        float frac = dist / distance;
        transform.position = Vector3.Lerp(transform.position, MarioPos, frac);
       
    }

    void IdleState()
    {
        CurrentState = BossStates.Moving;
    }


    void MovingState()
    {

        if (Nav.remainingDistance < 4f)
        {

            Nav.speed = 0.5f;
        }
        else if (Nav.remainingDistance >= 5f&& Nav.remainingDistance<11f)
        {
            Nav.speed = 3;

        }
        else if (Nav.remainingDistance>10f)
        {
            CurrentState = BossStates.Charge;
        }

        Nav.destination = Mario.transform.position;

    }
    void StompState()
    {

    }
    void FirebreathState()
    {
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        isCoroutineExecuting = false;
        StartCoroutine(PreFireBreath(timeToDodgeFire));
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

       // Debug.Log("While loop broken");
        isFireBreath = false;
        isCoroutineExecutingone = false;
        StartCoroutine(WaitTransitionState(1.5f));

        isCoroutineExecuting = false;
    }
    IEnumerator PreFireBreath(float seconds)
    {

        if (isCoroutineExecuting)
            yield break;
        
        isCoroutineExecuting = true;
        GetComponentInChildren<TriggerFireEvent>().EnableParticleSystem();
        yield return new WaitForSeconds(seconds);
        isFireBreath = true;
        isCoroutineExecuting = false;

        StartCoroutine(PostFireBreath(1.0f));
        isCoroutineExecuting = false;
    }
    IEnumerator WaitTransitionState(float seconds)
    {
        if (isCoroutineExecutingone)
            yield break;
        isCoroutineExecutingone = true;
        yield return new WaitForSeconds(seconds);
        GetComponent<NavMeshAgent>().enabled = true;
        CurrentState = BossStates.Moving;
        isCoroutineExecutingone = false;
    }
}