    using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


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
    public float timeToDodgeFire = 1;
    float timeElapsed = 0.0f;
    int bHealth = 3;
    public bool isFireBreath = false;
    public bool PlayerInRange = false;
    
    bool ResetTime;
    public BossStates CurrentState = BossStates.Idle;
    public GameObject Mario;
    public Collider wowser;
    public GameObject arena;
    public BasePlayer mariocontroller;
    private Vector3 moveDirection = Vector3.zero;

    //Components
    NavMeshAgent Nav;
    Rigidbody Rigid;

    void Start()
    {
        Nav = GetComponent<NavMeshAgent>();
        Rigid = GetComponent<Rigidbody>();
        Rigid.isKinematic = true;
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
            default:
                break;
        }
    }




    void IdleState()
    {


        //controls duration of IdleState // change hard coded 1 eventually
        if (timeElapsed > 1)


            //controls duration of IdleState // change hard coded 1 eventually
            if (timeElapsed > 5)

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
        

        if (Nav.remainingDistance < 4f)
        {

            Nav.speed = 0.5f;
        }
        else if (Nav.remainingDistance >= 5f)
        {
            Nav.speed = 3;

        }

        Nav.destination = Mario.transform.position;

    }
    void StompState()
    {
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

    IEnumerator FireBreathAttack(float seconds)
    {

        if (isCoroutineExecuting)
            yield break;

        isCoroutineExecuting = true;

        GetComponentInChildren<TriggerFireEvent>().EnableParticleSystem();

        yield return new WaitForSeconds(seconds);
        isFireBreath = true;

        yield return new WaitForSeconds(1);

        GetComponentInChildren<TriggerFireEvent>().DisableParticleSystem();
        isFireBreath = false;
   
        yield return new WaitForSeconds(1.5f);
        CurrentState = BossStates.Moving;

        isCoroutineExecuting = false;
    }


    IEnumerator StompAttack(float seconds)
    {
        if (isCoroutineExecuting)
            yield break;
        isCoroutineExecuting = true;
        //TimeUntil Stomp Starts
        //yield return new WaitForSeconds(seconds);

        
        Nav.Stop(true);
        
        Nav.enabled = false;
        
        Rigid.isKinematic = false;

        Rigid.useGravity = true;
        yield return new WaitForSeconds(1);
        Rigid.AddForce(new Vector3(0, 10, 0), ForceMode.Impulse);
        //
        Debug.Log("Liftoff");
        yield return new WaitForSeconds(5);
        Debug.Log("Land");
        Rigid.isKinematic = true;
        Rigid.useGravity = false;
        //Nav.Warp(transform.position);
        Nav.enabled = true;
        

        //time until continues to walk
        yield return new WaitForSeconds(1.5f);
        CurrentState = BossStates.Moving;

        isCoroutineExecuting = false;
        //
    }


        //From The following:
        //http://answers.unity3d.com/questions/714835/best-way-to-spawn-prefabs-in-a-circle.html
        Vector3 RandomCircle(Vector3 center, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }
}


