using UnityEngine;
using UnityEngine.AI;

public class QueenWormController : MonoBehaviour, IEnemyBehavior
{
    private enum AI
    {
        Idle,
        Move,
        ClawAttack,
        BiteAttack,
        CastSpell,
        BreathAttackStart,
        BreathAttackLoop,
        BreathAttackEnd,
        Summon,
        Defend,
        TakeDamage,
        Die
    }

    [SerializeField]
    private int maxHealth = 15;

    private int health = 15;
    private bool dead = false;

    private Animator anim;
    private float PlayerDist;
    private Vector3 PlayerPos;
    private bool defendTime;
    private float idleTime;
    private NavMeshAgent navigate;

    [SerializeField]
    private int pointValue = 1;

    private GameObject weapon;
    private IWeaponBehavior weaponScript;
    public AudioClip deathSFX;

    [SerializeField]
    private GameObject weaponBreath;

    private BreathAttack Breath;

    [SerializeField]
    private GameObject Exit;

    [SerializeField]
    private float BreathAttackTime = 888;

    private float BreathAttackCD = 15;

    [SerializeField]
    private int attackRange = 5;

    [SerializeField]
    private AI cs;

    private AI currentState
    {
        get { return cs; }
        set
        {
            switch (value)
            {
                case AI.Idle:
                    navigate.speed = 0;
                    navigate.Stop();
                    idleTime = 0;
                    cs = value;
                    break;

                case AI.Move:
                    anim.SetBool("Move", true);
                    navigate.Resume();
                    navigate.speed = 10f;
                    cs = value;
                    break;

                case AI.ClawAttack:
                    anim.SetBool("Claw Attack", true);
                    navigate.speed = 0;
                    navigate.Stop();
                    idleTime = 0;
                    cs = value;
                    break;

                case AI.BiteAttack:
                    anim.SetBool("Bite Attack", true);
                    navigate.speed = 0;
                    navigate.Stop();
                    idleTime = 0;
                    cs = value;
                    break;

                case AI.CastSpell:
                    anim.SetBool("Cast Spell", true);
                    navigate.speed = 0;
                    navigate.Stop();
                    idleTime = 0;
                    cs = value;
                    break;

                case AI.BreathAttackStart:
                    BreathAttackTime = 0;
                    anim.SetBool("Breath Attack", true);
                    navigate.speed = 0;
                    navigate.Stop();
                    idleTime = 0;
                    cs = value;
                    break;

                case AI.BreathAttackLoop:
                    cs = value;
                    break;

                case AI.BreathAttackEnd:
                    anim.SetBool("Bite Attack", false);
                    cs = value;
                    break;

                case AI.Summon:
                    anim.SetBool("Summon", true);
                    navigate.speed = 0;
                    navigate.Stop();
                    idleTime = 0;
                    cs = value;
                    break;

                case AI.Defend:
                    anim.SetBool("Defend", true);
                    navigate.speed = 0;
                    navigate.Stop();
                    idleTime = 0;
                    cs = value;
                    break;

                case AI.TakeDamage:
                    anim.SetBool("Take Damage", true);
                    break;

                case AI.Die:
                    EventSystem.ScoreIncrease(pointValue);

                    Exit.SetActive(true);
                    navigate.speed = 0f;
                    navigate.enabled = false;
                    anim.SetBool("Die", true);
                    dead = true;
                    cs = value;
                    break;
            }
        }
    }

    // Use this for initialization
    private void Start()
    {
        anim = GetComponent<Animator>();
        navigate = GetComponent<NavMeshAgent>();
        currentState = AI.Idle;
        health = 15;

        monsterName = "Queen Worm ABOMINATION";

        weapon = FindWeapon(transform).gameObject;
        weaponScript = weapon.GetComponent<IWeaponBehavior>();

        Breath = weaponBreath.GetComponent<BreathAttack>();
    }

    // Update is called once per frame
    private void Update()
    {
        BreathAttackTime += Time.deltaTime;

        PlayerDist = Vector3.Distance(PlayerPos, transform.position);

        switch (currentState)
        {
            case AI.Idle:
                if (idleTime > 1f)
                {
                    if (PlayerDist <= 5f && AttackRangeCheck(5))
                    {
                        if (defendTime)
                        {
                            currentState = AI.Defend;
                            defendTime = false;
                        }
                        else
                        {
                            currentState = AI.ClawAttack;
                            defendTime = true;
                        }
                    }
                    else if (PlayerDist <= 8f && AttackRangeCheck(9))
                        currentState = AI.BiteAttack;
                    else if (PlayerDist <= 30f)
                    {
                        if (health <= 10 && AttackRangeCheck(9) && BreathAttackTime > BreathAttackCD)
                        {
                            currentState = AI.BreathAttackStart;
                        }
                        else
                        {
                            currentState = AI.Move;
                        }
                    }
                }
                if (idleTime > 3f)
                    idleTime = 0;
                idleTime += Time.deltaTime;
                break;

            case AI.Move:
                navigate.SetDestination(PlayerPos);
                if (PlayerDist <= 12f)
                {
                    if (health <= 10 && AttackRangeCheck(11) && BreathAttackTime > BreathAttackCD)
                    {
                        currentState = AI.BreathAttackStart;
                        anim.SetBool("Move", false);
                    }
                    else if (AttackRangeCheck(5))
                    {
                        currentState = AI.BiteAttack;
                        anim.SetBool("Move", false);
                    }
                }
                else if (PlayerDist <= 5f && AttackRangeCheck(5))
                {
                    currentState = AI.ClawAttack;
                    anim.SetBool("Move", false);
                }
                break;

            case AI.ClawAttack:

                break;

            case AI.BiteAttack:

                break;

            case AI.CastSpell:

                break;

            case AI.BreathAttackStart:

                break;

            case AI.BreathAttackLoop:

                break;

            case AI.BreathAttackEnd:

                break;

            case AI.Summon:
                break;

            case AI.Defend:

                break;

            case AI.TakeDamage:

                break;

            case AI.Die:
                break;
        }
    }

    public void Kill()
    {
        AttackFinished();
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

    public void BreathStart()
    {
        Breath.StartAttack();
    }

    public void BreathFinished()
    {
        Breath.EndAttack();
        currentState = AI.Idle;
        anim.SetBool("Breath Attack", false);
    }

    public void TakeDamage(int damage = 1)
    {
        AttackFinished();
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SFX Volume");

        if (!dead)
        {
            health -= damage;
            if (health < 1)
            {
                GetComponent<AudioSource>().PlayOneShot(deathSFX);
                Kill();
            }
            else
            {
                currentState = AI.TakeDamage;
                GetComponent<AudioSource>().Play();
            }
        }
    }

    private void OnEnable()
    {
        EventSystem.onPlayerPositionUpdate += UpdateTargetPosistion;
    }

    private void OnDisable()
    {
        EventSystem.onPlayerPositionUpdate -= UpdateTargetPosistion;
    }

    private void UpdateTargetPosistion(Vector3 pos)
    {
        PlayerPos = pos;
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

    private Ray rangeForward;
    private Ray rangeLeft;
    private Ray rangeRight;
    public string monsterName;

    private bool AttackRangeCheck(float range = 10f)
    {
        Debug.DrawRay(transform.position + Vector3.up, transform.forward * range, Color.red);
        Debug.DrawRay(transform.position + Vector3.up, (transform.forward + transform.right * 0.25f) * range, Color.red);
        Debug.DrawRay(transform.position + Vector3.up, (transform.forward - transform.right * 0.25f) * range, Color.red);
        rangeForward = new Ray(transform.position + Vector3.up, transform.forward * range);
        rangeRight = new Ray(transform.position + Vector3.up, (transform.forward - transform.right * 0.25f) * range);
        rangeLeft = new Ray(transform.position + Vector3.up, (transform.forward + transform.right * 0.25f) * range);
        RaycastHit forwardHit;
        RaycastHit leftHit;
        RaycastHit rightHit;

        if (Physics.Raycast(rangeForward, out forwardHit, range) && forwardHit.transform.tag == "Player" || Physics.Raycast(rangeLeft, out leftHit, range) && leftHit.transform.tag == "Player" || Physics.Raycast(rangeRight, out rightHit, range) && rightHit.transform.tag == "Player")
        {
            return true;
        }
        return false;
    }

    public int RemainingHealth()
    {
        //Debug.Log(health);
        return health;
    }

    public void ResetToIdle()
    {
        currentState = AI.Idle;
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