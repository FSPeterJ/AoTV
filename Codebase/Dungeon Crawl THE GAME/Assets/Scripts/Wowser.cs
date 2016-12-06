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
    bool isCoroutineExecuting = false;
    public float timeToDodgeFire = 1;
    float timeElapsed = 0.0f;
    public int bHealth = 3;
    public bool isFireBreath = false;
    public bool PlayerInRange = false;

    bool ResetTime;
    public BossStates CurrentState = BossStates.Idle;
    public GameObject Mario;
    public Collider wowser;
    public GameObject arena;
    public GameObject StompArea;
    //Components
    NavMeshAgent Nav;
    Rigidbody Rigid;

    void Start()
    {
        Nav = GetComponent<NavMeshAgent>();
        Rigid = GetComponent<Rigidbody>();
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

    

    
    //
    private void ChargeState()
    {
        Mario.GetComponent<BasePlayer>().canGrabTail = false;
        StartCoroutine(ChargeAttack(1));
        //    if (hasoccured == true)
        //    {
        //        Nav.enabled = false;
        //        GetComponentInChildren<Rigidbody>().isKinematic = false;
        //        distance = Vector3.Distance(transform.position, MarioPos);
        //        MarioPos = Mario.transform.position;
        //        hasoccured = false;
        //        Rigid.useGravity = true;
        //        starttime = Time.time;
        //    }
        //
        //    Vector3 heading = MarioPos - transform.position;
        //    Vector3 startlocation = transform.forward;
        //    Rigid.AddForce(transform.forward, ForceMode.VelocityChange);
        //    if (transform.position == heading)
        //    {
        //        CurrentState = BossStates.Moving;
        //    }
        // to break we need to check collision with mario or a set distance


    }

    void IdleState()
    {   
     
    }


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
        Mario.GetComponent<BasePlayer>().canGrabTail = false;
        //isCoroutineExecuting = false; //?????
        StartCoroutine(StompAttack(1));

    }
    void FirebreathState()
    {
        //isCoroutineExecuting = false;//?????
        StartCoroutine(FireBreathAttack(timeToDodgeFire));
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
            if (wowser.GetComponent<Wowser>().CurrentState == BossStates.Idle)
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

        if (isCoroutineExecuting)
            yield break;
        isCoroutineExecuting = true;
        Nav.enabled = false;
        GetComponentInChildren<TriggerFireEvent>().EnableParticleSystem();

        yield return new WaitForSeconds(seconds);
        isFireBreath = true;

        yield return new WaitForSeconds(1);

        GetComponentInChildren<TriggerFireEvent>().DisableParticleSystem();
        isFireBreath = false;

        yield return new WaitForSeconds(1.5f);
        if (CurrentState != BossStates.Idle)
        {
            CurrentState = BossStates.Moving;
            Nav.enabled = true;
        }
        isCoroutineExecuting = false;
    }


    IEnumerator StompAttack(float seconds)
    {
        if (isCoroutineExecuting)
            yield break;
        isCoroutineExecuting = true;
        //TimeUntil Stomp Starts
        //yield return new WaitForSeconds(seconds);
        
        Mario.GetComponent<BasePlayer>().canGrabTail = false;
        Nav.Stop(true);

        Nav.enabled = false;

        Rigid.isKinematic = false;

        Rigid.useGravity = true;
       
        yield return new WaitForSeconds(0.5f);
        Rigid.AddForce(new Vector3(0, 10, 0), ForceMode.Impulse);
        
        //Debug.Log("Liftoff");
        yield return new WaitForSeconds(2f);
        //Debug.Log("Land");
        StompArea.GetComponent<ParticleSystem>().enableEmission = true;
        yield return new WaitForSeconds(0.5f);
        StompArea.GetComponent<ParticleSystem>().enableEmission = false;
        Mario.GetComponent<BasePlayer>().canGrabTail = true;
        Rigid.isKinematic = true;
        Rigid.useGravity = false;
        //Nav.Warp(transform.position);
        Nav.enabled = true;


        //time until continues to walk
        yield return new WaitForSeconds(1);
        
        CurrentState = BossStates.Moving;
        isCoroutineExecuting = false;
        //
    }


    //From The following:
    //http://answers.unity3d.com/questions/714835/best-way-to-spawn-prefabs-in-a-circle.html
    Vector3 RandomCircle(Vector3 center, float radius)
    {

        float ang = UnityEngine.Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }
    IEnumerator ChargeAttack(float seconds)
    {
        if (isCoroutineExecuting)
            yield break;
        Mario.GetComponent<BasePlayer>().canGrabTail = false;
        GetComponent<ParticleSystem>().enableEmission = true;
        transform.forward = Nav.transform.forward;
        isCoroutineExecuting = true;
        //TimeUntil Stomp Starts
        yield return new WaitForSeconds(seconds);
        Rigid.useGravity = true;
        Rigid.isKinematic = false;
        
        Nav.enabled = false;
        
        Rigid.AddForce(transform.forward*25, ForceMode.Impulse);

        yield return new WaitForSeconds(.9f);
        Rigid.isKinematic = true;
        Nav.enabled = true;
        GetComponent<ParticleSystem>().enableEmission = false;
        Mario.GetComponent<BasePlayer>().canGrabTail = true;
        CurrentState = BossStates.Moving;
        isCoroutineExecuting = false;
    }
}

