using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Boar_Controller : MonoBehaviour {

enum BoarState
    {
        Idle,Walk,Jump,Run,BiteAttack,TuskAttack,CastSpell,Defend,TakeDamage
    }

    //nav agent
    NavMeshAgent navAgent;
    //variables
    Animator anim;
    BoarState currentState;
    Vector3 targetPos;
    //wandering variarables;
    Vector3 wanderingSphere;
    Vector3 originPos;
    Vector3 randDirection;
    UnityEngine.AI.NavMeshHit navHitPos;
    float timer;
    float stopTimer = 1.8f;
    //Stat variables
    int health;


	// Use this for initialization
	void Start ()
    { 
        anim = GetComponent<Animator>();
        originPos= transform.position;
        randDirection = originPos;
        navHitPos.hit = true;
        navHitPos.position = transform.position;
        navAgent = GetComponent<NavMeshAgent>();
    }
	



	// Update is called once per frame
	void Update ()
    {
        //targetPos = //eventmanager passed pos
        timer += Time.deltaTime;


        //StateMachine
        switch (currentState)
        {
            case BoarState.Idle:
                {
                    if (timer >= stopTimer)
                    {
                        
                        anim.SetBool("Walk", true);
                        currentState = BoarState.Walk;
                    }
                    
                }
                break;
            case BoarState.Walk:
                {
                    navAgent.enabled = true;
                    if (navHitPos.hit == true)
                    {
                        while (Vector3.Distance(navHitPos.position, transform.position) < 5)
                        {
                            float x = originPos.x + (-10 + Random.Range(0, 20));
                            float z = originPos.z + (-10 + Random.Range(0, 20));
                            Vector3 randDirection = new Vector3(x, transform.position.y, z);
                            navHitPos.position = randDirection;
                        }
                        navAgent.SetDestination(navHitPos.position);
                        navHitPos.hit = false;
                        
                    }
                    if (navAgent.remainingDistance < .1)
                    {
                            navAgent.enabled = false;
                            anim.SetBool("Walk", false);
                            currentState = BoarState.Idle;
                            timer = 0;
                            navHitPos.hit = true;
                    }
                    
                    break;
                }
                
            case BoarState.Jump:
                {

                }
                break;
            case BoarState.Run:
                {

                }
                break;
            case BoarState.BiteAttack:
                {

                }
                break;
            case BoarState.TuskAttack:
                {

                }
                break;
            case BoarState.CastSpell:
                {

                }
                break;
            case BoarState.Defend:
                {

                }
                break;
            case BoarState.TakeDamage:
                {

                }
                break;
        }
       
    }
}
