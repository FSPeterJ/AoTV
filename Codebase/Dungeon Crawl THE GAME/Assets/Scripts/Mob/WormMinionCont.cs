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

    WormState _ws;
    Animator anim;
    float PlayerDist;
    Vector3 PlayerPos;
    bool defendTime;
    float idleTime;

    WormState currentState
    {
        get { return _ws; }
        set
        {
            switch (value)
            {
                case WormState.Idle:
                    break;
                case WormState.Bite:
                    anim.SetBool("Bite Attack", true);
                    break;
                case WormState.Projectile:
                    anim.SetBool("Projectile Attack", true);
                    break;
                case WormState.CastSpell:
                    anim.SetBool("Cast Spell", true);
                    break;
                case WormState.Defend:
                    anim.SetBool("Defend", true);
                    break;
                case WormState.TakeDamage:
                    anim.SetBool("Take Damage", true);
                    break;
                case WormState.Death:
                    GetComponent<BoxCollider>().enabled = false;
                    anim.SetBool("Die", true);
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
                    if (PlayerDist < 15f && PlayerDist > 10f)
                        currentState = WormState.Projectile;
                    else if (PlayerDist <= 10f && PlayerDist > 4f)
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
                }
                else
                    idleTime += Time.deltaTime;
                break;
            case WormState.Projectile:
                if (idleTime > 1f)
                {
                    currentState = WormState.Idle;
                }
                else
                    idleTime += Time.deltaTime;
                break;
            case WormState.CastSpell:
                if (idleTime > 1f)
                {
                    currentState = WormState.Idle;
                }
                else
                    idleTime += Time.deltaTime;
                break;
            case WormState.Defend:
                if (idleTime > 1f)
                {
                    currentState = WormState.Idle;
                }
                else
                    idleTime += Time.deltaTime;
                break;
            case WormState.TakeDamage:
                break;
            case WormState.Death:
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
