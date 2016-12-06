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

    //Basic Settings
    public float timeToDodgeFire = 1;
    public int bHealth = 3;

    //Object Plugin
    public GameObject Mario;
    public Collider wowser;
    public GameObject arena;
    public GameObject StompArea;


    //Internal Systems
    float timeElapsed = 0.0f;


    BossStates _cs;
    public BossStates CurrentState
    {
        get { return _cs; }
        set
        {
            KillCoroutine();

            if (ACR == null)
            {

                switch (value)
                {
                    case BossStates.Idle:
                        FireEvent.DisableParticleSystem();
                        StompEM.enabled = false;
                        ChargeEM.enabled = false;
                        canGrabTail = true;
                        Nav.enabled = false;
                        Rigid.isKinematic = false;
                        break;
                    case BossStates.Moving:
                        Nav.enabled = true;
                        Rigid.isKinematic = true;
                        break;
                    default:
                        break;

                }
                if (value == BossStates.Idle)
                {

                }
                _cs = value;
            }
        }
    }

    bool _fb = false;
    public bool isFireBreath
    {
        get { return _fb; }
        set { _fb = value; }
    }

    bool _gt = false;
    public bool canGrabTail
    {
        get { return _gt; }
        set { _gt = value; }
    }

    bool _pr = false;
    public bool PlayerInRange
    {
        get { return _pr; }
        set { _pr = value; }
    }

    //Internal Components   
    NavMeshAgent Nav;
    Rigidbody Rigid;
    ParticleSystem.EmissionModule ChargeEM;
    ParticleSystem.EmissionModule StompEM;
    TriggerFireEvent FireEvent;

    //Action Coroutine
    IEnumerator ACR = null;




    void Start()
    {
        Nav = GetComponent<NavMeshAgent>();
        Rigid = GetComponent<Rigidbody>();
        CurrentState = BossStates.Moving;
        ChargeEM = GetComponent<ParticleSystem>().emission;
        StompEM = StompArea.GetComponent<ParticleSystem>().emission;
        FireEvent = GetComponentInChildren<TriggerFireEvent>();
    }

    void Update()
    {
        if (bHealth <= 0)
        {
            SceneManager.LoadScene("KillWowser");
        }


        //Animator shit that doesn't work
        //if (Mathf.Abs(transform.position.x) + Math.Abs(transform.position.z) > 21)
        //{
        //    wowser.GetComponent<Animator>().SetTrigger("Falling");
        //}

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

    //WORKING AREA:
    //============================================================
    /// <summary>
    /// Activates the given Action Coroutine if no Action Coroutine is currently active then clears the ACR after it is complete
    /// </summary>
    /// <param name="CR">Coroutine function</param>
    void StartActionCoroutine(IEnumerator CR)
    {
        if (ACR == null)
        {
            StartCoroutine(_TCR(CR));
        }
    }
    //Internal Function
    IEnumerator _TCR(IEnumerator CR)
    {
        ACR = CR;
        yield return StartCoroutine(CR);
        ACR = null;
    }

    /// <summary>
    /// Stops the current Action Coroutine and clears the ACR
    /// </summary>
    void KillCoroutine()
    {
        if (ACR != null)
        {
            StopCoroutine(ACR);
            ACR = null;
        }
    }

    //ToDo:

    //Activating a new CR should cancel active CR in certain conditions?
    //If CRUnkillable is true, set CRMustDie to true and kill CR ASAP

    /*
    - Store Action Coroutines (ACR)
    - Ability to stop ACR
    - Method for changing states
    - Setting Idle kills the ACR
    - Converted CurrentState, PlayerInRange, and isFireBreath public variables to Getter / Setters
    */


    //========================================================



    /// <summary>
    /// Wowser will charge forward, damaging the player
    /// </summary>
    void ChargeState()
    {
        StartActionCoroutine(ChargeAttack(1));
    }

    /// <summary>
    /// Wowser will do nothing
    /// </summary>
    void IdleState()
    {

    }

    /// <summary>
    /// Wowser will move to intercept the player
    /// </summary>
    void MovingState()
    {


        if (Nav.remainingDistance < 4f)
        {

            Nav.speed = 0.5f;
        }
        else if (Nav.remainingDistance >= 5f && Nav.remainingDistance < 11f)
        {
            Nav.speed = 3;

        }
        else if (Nav.remainingDistance > 10f)
        {
            CurrentState = BossStates.Charge;
        }

        Nav.destination = Mario.transform.position;

    }
    void StompState()
    {
        StartActionCoroutine(StompAttack(1));
    }
    void FirebreathState()
    {
        StartActionCoroutine(FireBreathAttack(timeToDodgeFire));
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
            if (CurrentState == BossStates.Idle)
            {
                --bHealth;
                Destroy(col.gameObject);
            }
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

    IEnumerator FireBreathAttack(float seconds)
    {

        Nav.enabled = false;
        FireEvent.EnableParticleSystem();

        yield return new WaitForSeconds(seconds);
        isFireBreath = true;

        yield return new WaitForSeconds(1);

        FireEvent.DisableParticleSystem();
        isFireBreath = false;

        yield return new WaitForSeconds(1.5f);
        if (CurrentState != BossStates.Idle)
        {
            CurrentState = BossStates.Moving;
            Nav.enabled = true;
        }
    }


    IEnumerator StompAttack(float seconds)
    {
        //TimeUntil Stomp Starts
        //yield return new WaitForSeconds(seconds);

        canGrabTail = false;
        Nav.Stop();

        Nav.enabled = false;


        //Physics On
        Rigid.isKinematic = false;
        Rigid.useGravity = true;

        yield return new WaitForSeconds(0.5f);
        Rigid.AddForce(new Vector3(0, 10, 0), ForceMode.Impulse);

        //Debug.Log("Liftoff");
        yield return new WaitForSeconds(2f);
        //Debug.Log("Land");
        StompEM.enabled = true;
        yield return new WaitForSeconds(0.5f);
        StompEM.enabled = false;
        canGrabTail = true;
        //Physics Off
        Rigid.isKinematic = true;
        Rigid.useGravity = false;
        //Nav.Warp(transform.position);
        Nav.enabled = true;


        //time until continues to walk
        yield return new WaitForSeconds(1);

        CurrentState = BossStates.Moving;

    }





    IEnumerator ChargeAttack(float seconds)
    {
        canGrabTail = false;
        ChargeEM.enabled = true;
        transform.forward = Nav.transform.forward;
        //TimeUntil Stomp Starts
        yield return new WaitForSeconds(seconds);
        Rigid.useGravity = true;
        Rigid.isKinematic = false;

        Nav.enabled = false;

        Rigid.AddForce(transform.forward * 25, ForceMode.Impulse);

        yield return new WaitForSeconds(.9f);
        Rigid.isKinematic = true;
        Nav.enabled = true;
        ChargeEM.enabled = false;
        canGrabTail = true;
        CurrentState = BossStates.Moving;
    }
}

