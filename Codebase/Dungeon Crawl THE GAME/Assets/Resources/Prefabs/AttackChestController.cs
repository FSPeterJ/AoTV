using UnityEngine;
using UnityEngine.AI;

public class AttackChestController : MonoBehaviour, IEnemyBehavior
{
    //Use for executing commands on when first entering a state
    //Can also be used to prevent states from changing under certain conditions
    public AI _cs;

    public AI currentState
    {
        get { return _cs; }
        set
        {
            if (value == AI.SuperBite)
            {
                weaponScript.SetDamage(1);
            }

            switch (value)
            {
                case AI.Idle:
                    navAgent.speed = 0;
                    navAgent.enabled = false;
                    idleTime = 0;
                    _cs = value;
                    break;

                case AI.Wander:
                    anim.SetBool("Walk", true);
                    navAgent.Resume();
                    navAgent.speed = 3.5f;
                    _cs = value;
                    break;

                case AI.Jump:
                    _cs = value;
                    break;

                case AI.Run:
                    anim.SetBool("Run", true);
                    navAgent.Resume();
                    navAgent.speed = 15f;
                    _cs = value;
                    break;

                case AI.Walk:
                    anim.SetBool("Walk", true);
                    navAgent.enabled = true;
                    navAgent.speed = 3.5f;
                    _cs = value;
                    break;

                case AI.BiteAttack:
                    anim.SetBool("Bite Attack", true);
                    navAgent.speed = 0;
                    if (navAgent.enabled)
                        navAgent.Stop();
                    idleTime = 0;
                    _cs = value;
                    break;

                case AI.SuperBite:
                    weaponScript.SetDamage(999);
                    anim.SetBool("Rest", false);
                    anim.SetBool("Bite Attack", true);
                    navAgent.speed = 0;
                    if (navAgent.enabled)
                        navAgent.Stop();
                    idleTime = 0;
                    _cs = value;
                    break;

                case AI.TakeDamage:
                    anim.SetBool("Take Damage", true);
                    break;

                case AI.Die:
                    dead = true;
                    idleTime = 0;
                    navAgent.speed = 0;
                    navAgent.enabled = false;
                    anim.SetBool("Die", true);
                    bCollider.enabled = false;
                    EventSystem.ScoreIncrease(pointValue);

                    _cs = value;
                    break;

                case AI.Rest:
                    navAgent.speed = 0;
                    navAgent.enabled = false;
                    anim.SetBool("Rest", true);
                    _cs = value;
                    break;

                default:

                    _cs = value;
                    break;
            }
        }
    }

    public enum AI
    {
        Idle, Walk, Jump, Run, BiteAttack, CastSpell, Defend, TakeDamage, Wander, Die, Rest, SuperBite
    }

    //variables
    private Animator anim;

    private Vector3 targetPos;
    private float targetdistance;

    //wandering variarables;
    private Vector3 wanderingSphere;

    //Stat variables
    [SerializeField]
    public int health = 4;

    private bool dead = false;

    [SerializeField]
    public int pointValue = 1;

    //References
    private NavMeshAgent navAgent;

    private BoxCollider bCollider;
    private IWeaponBehavior weaponScript;
    private GameObject mouthGizmo;
    public AudioClip deathSFX;

    private float idleTime = 0;
    private string monsterName;

    private void OnEnable()
    {
        EventSystem.onPlayerPositionUpdate += UpdateTargetPosition;
        EventSystem.onPlayerDeath += PlayerDied;
    }

    //unsubscribe from player movement
    private void OnDisable()
    {
        EventSystem.onPlayerPositionUpdate -= UpdateTargetPosition;
        EventSystem.onPlayerDeath -= PlayerDied;
    }

    public void ResetToIdle()
    {
        currentState = AI.Idle;
    }

    public void TakeDamage(int damage = 1)
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SFX Volume");
        if (currentState == AI.Rest)
        {
            currentState = AI.Idle;
        }
        //AttackFinished();
        if (dead == false)
        {
            health -= damage;
            if (health < 1)
            {
                GetComponent<AudioSource>().PlayOneShot(deathSFX);
                Kill();
            }
            else
            {
                GetComponent<AudioSource>().Play();
                currentState = AI.TakeDamage;
            }
        }
    }

    public int RemainingHealth()
    {
        return health;
    }

    public void Kill()
    {
        //AttackFinished();
        currentState = AI.Die;
    }

    public void AttackFinished()
    {
        weaponScript.AttackEnd();
        currentState = AI.Idle;
    }

    public void AttackStart()
    {
        weaponScript.AttackStart();
    }

    private void UpdateTargetPosition(Vector3 pos)
    {
        targetPos = pos;
    }

    private void PlayerDied()
    {
        targetPos = new Vector3(targetPos.x, 999999, targetPos.z);
    }

    private void Start()
    {
        bCollider = GetComponent<BoxCollider>();
        anim = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        weaponScript = FindWeapon(transform).GetComponent<IWeaponBehavior>();
        currentState = AI.Rest;
    }

    private void Update()
    {
        targetdistance = Vector3.Distance(targetPos, transform.position);
        idleTime += Time.deltaTime;
        switch (currentState)
        {
            case AI.Idle:
                currentState = AI.Rest;
                break;

            case AI.Walk:
                navAgent.SetDestination(targetPos);
                if (targetdistance < 4f)
                {
                    anim.SetBool("Walk", false);
                    idleTime = 0;
                    currentState = AI.BiteAttack;
                }
                break;

            case AI.Jump:
                break;

            case AI.Run:
                break;

            case AI.BiteAttack:

                break;

            case AI.CastSpell:
                break;

            case AI.Defend:
                break;

            case AI.TakeDamage:
                break;

            case AI.Wander:
                break;

            case AI.Die:
                if (idleTime > 2)
                {
                    Destroy(gameObject);
                }
                break;

            case AI.Rest:
                if (health != 4)
                {
                    anim.SetBool("Rest", false);
                    currentState = AI.Walk;
                    idleTime = 0;
                }
                break;

            default:
                break;
        }
    }

    private void OnTriggerStay(Collider Col)
    {
        if (Col.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                currentState = AI.SuperBite;
            }
        }
    }

    private GameObject FindWeapon(Transform obj)
    {
        foreach (Transform tr in obj)
        {
            if (tr.tag == "Weapon")
            {
                return tr.gameObject;
            }
            if (tr.childCount > 0)
            {
                GameObject temp = FindWeapon(tr);
                if (temp)
                {
                    return temp;
                }
            }
        }
        return null;
    }

    public string Name()
    {
        return monsterName;
    }

    public float HPOffsetHeight()
    {
        return GetComponentInChildren<Renderer>().bounds.size.y + 1;
    }
}