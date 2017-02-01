using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormMinionCont : MonoBehaviour
{
    enum WormState
    {
        Idle,
        Bite,
        Projectile,
        CastSpell,
        Defend,
        TakeDamage,
        Death
    }

    [SerializeField]
    WormState _ws;


    Animator anim;
    float PlayerDist;
    Vector3 PlayerPos;
    bool defendTime;
    float idleTime;

    [SerializeField]
    GameObject Proj;

    WormState currentState
    {
        get { return _ws; }
        set
        {
            switch (value)
            {
                case WormState.Idle:
                    _ws = value;
                    break;
                case WormState.Bite:
                    anim.SetBool("Bite Attack", true);
                    _ws = value;
                    break;
                case WormState.Projectile:
                    anim.SetBool("Projectile Attack", true);
                    _ws = value;
                    break;
                case WormState.CastSpell:
                    anim.SetBool("Cast Spell", true);
                    _ws = value;
                    break;
                case WormState.Defend:
                    anim.SetBool("Defend", true);
                    _ws = value;
                    break;
                case WormState.TakeDamage:
                    anim.SetBool("Take Damage", true);
                    _ws = value;
                    break;
                case WormState.Death:
                    GetComponent<BoxCollider>().enabled = false;
                    anim.SetBool("Die", true);
                    _ws = value;
                    break;
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        defendTime = true;
        idleTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerDist = Vector3.Distance(PlayerPos, transform.position);
        switch (currentState)
        {
            case WormState.Idle:
                if (idleTime > 1f)
                {
                    if (PlayerDist < 25f && PlayerDist > 15f)
                        currentState = WormState.Projectile;
                    else if (PlayerDist <= 15f && PlayerDist > 4f)
                        currentState = WormState.CastSpell;
                    else if (PlayerDist <= 4f)
                    {
                        if (defendTime)
                        {
                            currentState = WormState.Defend;
                            defendTime = false;
                        }
                        else
                        {
                            currentState = WormState.Bite;
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
            case WormState.Bite:
                if (idleTime > 1f)
                {
                    currentState = WormState.Idle;
                    anim.SetBool("Bite Attack", false);
                }
                else
                    idleTime += Time.deltaTime;
                break;
            case WormState.Projectile:
                if (idleTime > 1f)
                {
                    currentState = WormState.Idle;
                    Instantiate(Proj, transform);
                    anim.SetBool("Projectile Attack", false);
                }
                else
                    idleTime += Time.deltaTime;
                break;
            case WormState.CastSpell:
                if (idleTime > 1f)
                {
                    currentState = WormState.Idle;
                    anim.SetBool("Cast Spell", false);

                }
                else
                    idleTime += Time.deltaTime;
                break;
            case WormState.Defend:
                if (idleTime > 1f)
                {
                    currentState = WormState.Idle;
                    anim.SetBool("Defend", false);
                }
                else
                    idleTime += Time.deltaTime;
                break;
            case WormState.TakeDamage:
                break;
            case WormState.Death:
                break;
            default:
                currentState = WormState.Idle;
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

    void UpdateTargetPosition(Vector3 pos)
    {
        PlayerPos = pos;
    }
}
