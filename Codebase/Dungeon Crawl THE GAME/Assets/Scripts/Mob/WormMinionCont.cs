using UnityEngine;

public class WormMinionCont : MonoBehaviour, IEnemyBehavior
{
    private enum AI
    {
        Idle,
        Bite,
        Projectile,
        CastSpell,
        Defend,
        TakeDamage,
        Die
    }

    [SerializeField]
    private AI _ws;

    private Animator anim;
    private float PlayerDist;
    private Vector3 targetPos;
    private bool defendTime;
    private float idleTime;
    private int health;
    private bool dead = false;
    private GameObject weapon;
    private BoxCollider bCollider;
    private IWeaponBehavior weaponScript;
    public int pointValue = 1;
    public AudioClip deathSFX;

    private GameObject Proj;
    private string monsterName = "Worm Minion";

    private AI currentState
    {
        get { return _ws; }
        set
        {
            switch (value)
            {
                case AI.Idle:
                    _ws = value;
                    break;

                case AI.Bite:
                    anim.SetBool("Bite Attack", true);
                    _ws = value;
                    break;

                case AI.Projectile:
                    anim.SetBool("Projectile Attack", true);
                    _ws = value;
                    break;

                case AI.CastSpell:
                    anim.SetBool("Cast Spell", true);
                    _ws = value;
                    break;

                case AI.Defend:
                    anim.SetBool("Defend", true);
                    _ws = value;
                    break;

                case AI.TakeDamage:
                    anim.SetBool("Take Damage", true);
                    break;

                case AI.Die:
                    EventSystem.ScoreIncrease(pointValue);
                    foreach (Collider c in GetComponentsInChildren<Collider>())
                    {
                        c.enabled = false;
                    }
                    anim.SetTrigger("Die");
                    dead = true;
                    _ws = value;
                    break;
            }
        }
    }

    // Use this for initialization
    private void Start()
    {
        bCollider = GetComponent<BoxCollider>();
        anim = GetComponent<Animator>();
        Proj = (GameObject)Resources.Load("Prefabs/Projectiles/Fireball Projectile");
        weapon = FindWeapon(transform);
        weaponScript = weapon.GetComponent<IWeaponBehavior>();
        monsterName = "Worm Minion";
        defendTime = true;
        health = 2;
        idleTime = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        PlayerDist = Vector3.Distance(targetPos, transform.position);
        switch (currentState)
        {
            case AI.Idle:
                if (idleTime > 1f)
                {
                    if (PlayerDist < 25f && PlayerDist > 8f)
                        currentState = AI.Projectile;
                    else if (PlayerDist <= 15f && PlayerDist > 99f)
                        currentState = AI.CastSpell;
                    else if (PlayerDist <= 8f)
                    {
                        if (defendTime)
                        {
                            currentState = AI.Defend;
                            defendTime = false;
                        }
                        else
                        {
                            currentState = AI.Bite;
                            defendTime = true;
                        }
                    }
                }
                if (idleTime > 3f)
                {
                    idleTime = 0;
                }
                idleTime += Time.deltaTime;
                break;

            case AI.Bite:
                RotateToFaceTarget(targetPos);
                break;

            case AI.Projectile:
                RotateToFaceTarget(targetPos);
                break;

            case AI.CastSpell:
                break;

            case AI.Defend:
                RotateToFaceTarget(targetPos);
                if (idleTime > 1f)
                {
                    anim.SetBool("Defend", false);
                    currentState = AI.Idle;
                }
                idleTime += Time.deltaTime;
                break;

            case AI.TakeDamage:
                RotateToFaceTarget(targetPos);
                break;

            case AI.Die:
                break;

            default:
                currentState = AI.Idle;
                break;
        }
    }

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

    private void UpdateTargetPosition(Vector3 pos)
    {
        targetPos = pos;
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
                Scoreinc();
            }
            else
            {
                GetComponent<AudioSource>().Play();
                currentState = AI.TakeDamage;
            }
        }
    }

    private void Scoreinc()
    {
        //Event goes here
    }

    public int RemainingHealth()
    {
        return health;
    }

    public void Kill()
    {
        AttackFinished();
        currentState = AI.Die;
    }

    private void AttackFinished()
    {
        weaponScript.AttackEnd();
        currentState = AI.Idle;
    }

    public void CreateProjectile()
    {
        Instantiate(Proj, weapon.transform.position, weapon.transform.rotation * Quaternion.Euler(0, -90, 0));
    }

    public void AttackStart()
    {
        weaponScript.AttackStart();
    }

    private void PlayerDied()
    {
        //EventSystem.onPlayerPositionUpdate -= UpdateTargetPosition;
        targetPos = new Vector3(targetPos.x, 999999, targetPos.z);
    }

    public void ResetToIdle()
    {
        currentState = AI.Idle;
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

    private void RotateToFaceTarget(Vector3 _TargetPosition, float _LerpSpeed = 4f, float _AngleAdjustment = -90f)
    {
        if (!dead)
        {
            Vector3 lookPos = (transform.position - _TargetPosition);
            lookPos.y = 0;
            float angle = Mathf.LerpAngle(transform.rotation.eulerAngles.y, -(Mathf.Atan2(lookPos.z, lookPos.x) * Mathf.Rad2Deg) + _AngleAdjustment, Time.deltaTime * _LerpSpeed);
            transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, 1, 0));
        }
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