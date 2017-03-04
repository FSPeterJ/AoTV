using UnityEngine;
using UnityEngine.AI;

public class GorgonController : MonoBehaviour
{
    private enum AI
    {
        idle,
        slither,
        stab,
        hairAttack,
        projectile,
        castSpell,
        defend,
        takeDamage,
        die
    }

    [SerializeField]
    private int maxHealth;

    [SerializeField]
    private GameObject fxProj;

    private int currentHealth;
    private Animator anim;
    private float PlayerDist;
    private Vector3 PlayerPos;
    private bool defendTime;
    private float idleTime;
    private NavMeshAgent navigate;
    public int pointValue = 1;
    private AI gs;
    private Vector3 wanderTarget;

    private AI currentState
    {
        get { return gs; }
        set
        {
            switch (value)
            {
                case AI.idle:
                    navigate.speed = 0;
                    navigate.Stop();
                    idleTime = 0;
                    gs = value;
                    break;

                case AI.slither:
                    anim.SetBool("Slither", true);
                    navigate.Resume();
                    navigate.speed = 8f;
                    gs = value;
                    break;

                case AI.stab:
                    anim.SetTrigger("Stab Attack");
                    navigate.speed = 0;
                    navigate.Stop();
                    idleTime = 0;
                    gs = value;
                    break;

                case AI.hairAttack:
                    anim.SetTrigger("Hair Snakes Attack");
                    navigate.speed = 0;
                    navigate.Stop();
                    idleTime = 0;
                    gs = value;
                    break;

                case AI.projectile:
                    anim.SetTrigger("Projectile Attack");
                    navigate.speed = 0;
                    navigate.Stop();
                    idleTime = 0;
                    gs = value;
                    break;

                case AI.castSpell:
                    anim.SetTrigger("Cast Spell");
                    navigate.speed = 0;
                    navigate.Stop();
                    idleTime = 0;
                    gs = value;
                    break;

                case AI.defend:
                    anim.SetBool("Defend", true);
                    navigate.speed = 0;
                    navigate.Stop();
                    idleTime = 0;
                    gs = value;
                    break;

                case AI.takeDamage:
                    anim.SetTrigger("Take Damage");
                    gs = value;
                    break;

                case AI.die:
                    anim.SetTrigger("Die");
                    EventSystem.ScoreIncrease(pointValue);
                    navigate.speed = 0f;
                    navigate.enabled = false;
                    gs = value;
                    break;
            }
        }
    }

    // Use this for initialization
    private void Start()
    {
        anim = GetComponent<Animator>();
        navigate = GetComponent<NavMeshAgent>();
        currentState = AI.idle;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    private void Update()
    {
        PlayerDist = Vector3.Distance(PlayerPos, transform.position);

        switch (currentState)
        {
            case AI.idle:
                if (idleTime > 1f)
                {
                    if (PlayerDist <= 4f)
                    {
                        if (defendTime)
                        {
                            currentState = AI.defend;
                            defendTime = false;
                        }
                        else
                        {
                            currentState = AI.stab;
                            defendTime = true;
                        }
                    }
                    else if (PlayerDist <= 6f)
                        currentState = AI.hairAttack;
                    else if (PlayerDist <= 15f)
                    {
                        if (currentHealth <= 5)
                            currentState = AI.castSpell;
                        else
                            currentState = AI.projectile;
                    }
                }
                if (idleTime > 3f)
                    idleTime = 0;
                idleTime += Time.deltaTime;
                break;

            case AI.slither:
                navigate.SetDestination(PlayerPos);
                if (PlayerDist <= 4f)
                {
                    currentState = AI.stab;
                    anim.SetBool("Move", false);
                }
                else if (PlayerDist <= 10f)
                {
                    currentState = AI.hairAttack;
                    anim.SetBool("Move", false);
                }
                break;

            case AI.stab:
                if (idleTime > 1f)
                    currentState = AI.idle;
                else
                    idleTime += Time.deltaTime;
                break;

            case AI.hairAttack:
                if (idleTime > 1f)
                    currentState = AI.idle;
                else
                    idleTime += Time.deltaTime;
                break;

            case AI.projectile:
                //Instantiate(fxProj);
                if (idleTime > 1f)
                    currentState = AI.idle;
                else
                    idleTime += Time.deltaTime;
                break;

            case AI.castSpell:
                if (idleTime > 1f)
                    currentState = AI.idle;
                else
                    idleTime += Time.deltaTime;
                break;

            case AI.defend:
                if (idleTime > 1f)
                {
                    currentState = AI.idle;
                    anim.SetBool("Defend", false);
                }
                else
                    idleTime += Time.deltaTime;
                break;

            case AI.takeDamage:
                currentHealth--;
                if (currentHealth < 1)
                    currentState = AI.die;
                break;

            case AI.die:
                break;
        }
    }

    private void OnEnable()
    {
        EventSystem.onPlayerPositionUpdate += UpdateTargetPosition;
    }

    private void OnDisable()
    {
        EventSystem.onPlayerPositionUpdate -= UpdateTargetPosition;
    }

    private void UpdateTargetPosition(Vector3 Pos)
    {
        PlayerPos = Pos;
    }
}