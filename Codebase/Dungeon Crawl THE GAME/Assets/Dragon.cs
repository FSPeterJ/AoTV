using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Dragon : MonoBehaviour, IEnemyBehavior
{
    public AudioClip Fallen;
    public ParticleSystem fireBreath;
    public GameObject mouth;
    public GameObject fireBreathCollider;
    public GameObject Target;
    public Vector3[] Waypoints;
    NavMeshAgent navMeshAgent;
    Animator anim;
    Rigidbody body;
    DragonStates dCurrentState;
    IWeaponBehavior weaponBehavior;
    IWeaponBehavior fireBreathBehavior;

    bool biteAttacked;
    bool alive;
    int transitionNumber;
    int Health;
    float timer;
    public enum DragonStates
    {
        Dialogue, FlyBiteAttack, FlyBreathAttack, FlyForwardToWaypoint, FlyIdle, Land, Recover, Die
    };


    void Start()
    {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        weaponBehavior = mouth.GetComponent<IWeaponBehavior>();
        fireBreathBehavior = fireBreathCollider.GetComponent<IWeaponBehavior>();
        timer = 0;
        Health = 50;
        transitionNumber = 0;
        biteAttacked = false;
        alive = true;
        dCurrentState = DragonStates.Dialogue;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F2))
        {
            TakeDamage(999);
        }
        Debug.Log("Animation State: " + dCurrentState.ToString() + " " + transitionNumber);
        if (alive)
        {
            switch (dCurrentState)
            {
                case DragonStates.Dialogue:
                    if (Input.GetKeyDown(KeyCode.F1))
                    {
                        transitionNumber++;
                        anim.SetBool("Fly Idle", true);
                        dCurrentState = DragonStates.FlyIdle;
                        timer = 3;
                    }
                    break;
                case DragonStates.FlyBiteAttack:
                    timer -= Time.deltaTime;
                    if (timer <= 0)
                    {
                        Waypoints[1] = Target.transform.position;
                        navMeshAgent.SetDestination(Waypoints[1]);
                        transitionNumber++;
                        dCurrentState = DragonStates.FlyForwardToWaypoint;
                    }
                    break;
                case DragonStates.FlyBreathAttack:
                    timer -= Time.deltaTime;
                    body.rotation = Quaternion.Slerp(body.rotation, Quaternion.LookRotation(Target.transform.position - body.position), 10 * Time.deltaTime);
                    body.AddForce(gameObject.transform.up * -1);
                    anim.SetTrigger("Fly Fire Breath Attack");
                    if (timer <= 0)
                    {
                        biteAttacked = false;
                        anim.SetBool("Fly Idle", true);
                        transitionNumber++;
                        dCurrentState = DragonStates.Land;
                    }
                    break;
                case DragonStates.FlyForwardToWaypoint:
                    if (navMeshAgent.remainingDistance == 0 && !biteAttacked)
                    {
                        biteAttacked = true;
                        anim.SetBool("Fly Forward", false);
                        anim.SetBool("Fly Idle", true);
                        anim.SetTrigger("Fly Bite Attack");
                        transitionNumber++;
                        dCurrentState = DragonStates.FlyBiteAttack;
                    }
                    else if (navMeshAgent.remainingDistance == 0 && biteAttacked)
                    {
                        anim.SetBool("Fly Forward", false);
                        timer = 1.4f;
                        transitionNumber++;
                        dCurrentState = DragonStates.FlyIdle;
                    }
                    break;
                case DragonStates.FlyIdle:
                    body.rotation = Quaternion.Slerp(body.rotation, Quaternion.LookRotation(Target.transform.position - body.position), 10 * Time.deltaTime);
                    body.AddForce(gameObject.transform.up * -150);
                    timer -= Time.deltaTime;
                    if (timer <= 0)
                    {
                        if (!biteAttacked)
                        {
                            Waypoints[0] = Target.transform.position;
                            navMeshAgent.SetDestination(Waypoints[0]);
                            anim.SetBool("Fly Idle", false);
                            anim.SetBool("Fly Forward", true);
                            timer = 1.5f;
                            transitionNumber++;
                            dCurrentState = DragonStates.FlyForwardToWaypoint;
                        }
                        else
                        {
                            //anim.SetBool("Fly Idle", false);
                            anim.SetTrigger("Fly Fire Breath Attack");
                            timer = 5;
                            transitionNumber++;
                            dCurrentState = DragonStates.FlyBreathAttack;
                        }
                    }
                    break;
                case DragonStates.Land:
                    timer = 7;
                    if (anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex("Base Layer")).IsTag("Fly Idle"))
                    {
                        anim.ResetTrigger("Fly Fire Breath Attack");
                        anim.SetBool("Fly Idle", false);
                        anim.SetBool("Idle", true);
                        dCurrentState = DragonStates.Recover;
                        transitionNumber++;
                    }
                    break;
                case DragonStates.Recover:
                    timer -= Time.deltaTime;
                    if (timer <= 0 && anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex("Base Layer")).IsTag("Idle"))
                    {
                        transitionNumber++;
                        timer = 3;
                        anim.SetBool("Idle", false);
                        anim.SetBool("Fly Idle", true);
                        dCurrentState = DragonStates.FlyIdle;
                    }
                    break;
                case DragonStates.Die:
                        anim.SetTrigger("Die");
                    
                    
                    break;
                default:
                    break;
            }
        }
    }

    public void ActivateFireBreath()
    {
        fireBreath.Play();
        fireBreathBehavior.AttackStart();
    }

    public void DeactivateFireBreath()
    {
        fireBreath.Stop();
        fireBreathBehavior.AttackEnd();
    }

    public void TakeDamage(int damage = 1)
    {
        Health -= damage;
       // if (dCurrentState == DragonStates.Recover)
        {
            GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SFX Volume");
            GetComponent<AudioSource>().Play();
            if (RemainingHealth() <= 0)
            {
                alive = false;
                anim.SetBool("Idle", false);
                Kill();
            }
            else
            {
                anim.SetBool("Take Damage", true);
                
            }
        }
    }

    public int RemainingHealth()
    {
        return Health;
    }

    public void Kill()
    {
        GetComponent<AudioSource>().PlayOneShot(Fallen);
        dCurrentState = DragonStates.Die;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ResetToIdle()
    {
        anim.SetBool("Idle", true);
    }

    void BeginAttacking()
    {
        weaponBehavior.AttackStart();
    }

    void EndAttacking()
    {
        weaponBehavior.AttackEnd();
    }
}
