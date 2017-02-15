using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dragon : MonoBehaviour
{
    public GameObject Target;
    public Vector3[] Waypoints;
    int transitionNumber;
    NavMeshAgent navMeshAgent;
    Animator anim;
    Rigidbody body;
    float timer;
    DragonStates dCurrentState;
    bool biteAttacked;
    bool readyToFire = false;

    public enum DragonStates
    {
        Dialogue, FlyBiteAttack, FlyBreathAttack, FlyForwardToWaypoint, FlyIdle, Land, Recover, Die
    };


    void Start()
    {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        timer = 0;
        transitionNumber = 0;
        biteAttacked = false;
        dCurrentState = DragonStates.Dialogue;
    }

    void Update()
    {
        Debug.Log("Animation State: " + dCurrentState.ToString() + " " + transitionNumber);
        switch (dCurrentState)
        {
            case DragonStates.Dialogue:
                transitionNumber++;
                dCurrentState = DragonStates.FlyIdle;
                timer = 3;
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
                    //anim.ResetTrigger("Fly Fire Breath Attack");
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
                anim.SetBool("Fly Idle", true);
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
                transitionNumber++;
                timer = 7;
                dCurrentState = DragonStates.Recover;
                break;
            case DragonStates.Recover:
                timer -= Time.deltaTime;
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Fly Idle"))
                {
                    anim.SetBool("Idle", true);
                }
                if (timer <= 0)
                {
                    transitionNumber++;
                    anim.SetBool("Idle", false);
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
