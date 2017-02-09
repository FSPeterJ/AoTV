using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GorgonController : MonoBehaviour
{
    enum GorgonState
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
    int maxHealth;

    [SerializeField]
    GameObject fxProj;

    int currentHealth;
    Animator anim;
    float PlayerDist;
    Vector3 PlayerPos;
    bool defendTime;
    float idleTime;
    NavMeshAgent navigate;
    ParticleSystem particles;
    public uint pointValue = 1;
    GorgonState gs;

    GorgonState currentState
    {
        get { return gs; }
        set
        {
            switch (value)
            {
                case GorgonState.idle:
                    navigate.speed = 0;
                    navigate.Stop();
                    idleTime = 0;
                    gs = value;
                    break;
                case GorgonState.slither:
                    anim.SetBool("Slither", true);
                    navigate.Resume();
                    navigate.speed = 8f;
                    gs = value;
                    break;
                case GorgonState.stab:
                    anim.SetTrigger("Stab Attack");
                    navigate.speed = 0;
                    navigate.Stop();
                    idleTime = 0;
                    gs = value;
                    break;
                case GorgonState.hairAttack:
                    anim.SetTrigger("Hair Snakes Attack");
                    navigate.speed = 0;
                    navigate.Stop();
                    idleTime = 0;
                    gs = value;
                    break;
                case GorgonState.projectile:
                    anim.SetTrigger("Projectile Attack");
                    navigate.speed = 0;
                    navigate.Stop();
                    idleTime = 0;
                    gs = value;
                    break;
                case GorgonState.castSpell:
                    anim.SetTrigger("Cast Spell");
                    navigate.speed = 0;
                    navigate.Stop();
                    idleTime = 0;
                    gs = value;
                    break;
                case GorgonState.defend:
                    anim.SetBool("Defend", true);
                    navigate.speed = 0;
                    navigate.Stop();
                    idleTime = 0;
                    gs = value;
                    break;
                case GorgonState.takeDamage:
                    anim.SetTrigger("Take Damage");
                    gs = value;
                    break;
                case GorgonState.die:
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
    void Start()
    {
        anim = GetComponent<Animator>();
        navigate = GetComponent<NavMeshAgent>();
        currentState = GorgonState.idle;
        currentHealth = maxHealth;
        particles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerDist = Vector3.Distance(PlayerPos, transform.position);

        switch (currentState)
        {
            case GorgonState.idle:
                if (idleTime > 1f)
                {
                    if (PlayerDist <= 4f)
                    {
                        if (defendTime)
                        {
                            currentState = GorgonState.defend;
                            defendTime = false;
                        }
                        else
                        {
                            currentState = GorgonState.stab;
                            defendTime = true;
                        }
                    }
                    else if (PlayerDist <= 6f)
                        currentState = GorgonState.hairAttack;
                    else if (PlayerDist <= 15f)
                    {
                        if (currentHealth <= 5)
                            currentState = GorgonState.castSpell;
                        else
                            currentState = GorgonState.projectile;
                    }
                }
                if (idleTime > 3f)
                    idleTime = 0;
                idleTime += Time.deltaTime;
                break;
            case GorgonState.slither:
                navigate.SetDestination(PlayerPos);
                if (PlayerDist <= 4f)
                {
                    currentState = GorgonState.stab;
                    anim.SetBool("Move", false);
                }
                else if (PlayerDist <= 10f)
                {
                    currentState = GorgonState.hairAttack;
                    anim.SetBool("Move", false);
                }
                break;
            case GorgonState.stab:
                if (idleTime > 1f)
                    currentState = GorgonState.idle;
                else
                    idleTime += Time.deltaTime;
                break;
            case GorgonState.hairAttack:
                if (idleTime > 1f)
                    currentState = GorgonState.idle;
                else
                    idleTime += Time.deltaTime;
                break;
            case GorgonState.projectile:
                //Instantiate(fxProj);
                if (idleTime > 1f)                
                    currentState = GorgonState.idle;
                else
                    idleTime += Time.deltaTime;
                break;
            case GorgonState.castSpell:
                if (idleTime > 1f)
                    currentState = GorgonState.idle;
                else
                    idleTime += Time.deltaTime;
                break;
            case GorgonState.defend:
                if (idleTime > 1f)
                {
                    currentState = GorgonState.idle;
                    anim.SetBool("Defend", false);
                }
                else
                    idleTime += Time.deltaTime;
                break;
            case GorgonState.takeDamage:
                currentHealth--;
                if (currentHealth < 1)
                    currentState = GorgonState.die;
                break;
            case GorgonState.die:
                break;
        }
    }

    void OnEnable()
    {
        EventSystem.onPlayerPositionUpdate += UpdateTargetPosition;
    }

    void OnDisable()
    {
        EventSystem.onPlayerPositionUpdate -= UpdateTargetPosition;
    }

    void UpdateTargetPosition(Vector3 Pos)
    {
        PlayerPos = Pos;
    }
}
