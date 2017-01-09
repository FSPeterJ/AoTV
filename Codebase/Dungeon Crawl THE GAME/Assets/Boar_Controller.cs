using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar_Controller : MonoBehaviour {

enum BoarState
    {
        Idle,Walk,Jump,Run,BiteAttack,TuskAttack,CastSpell,Defend,TakeDamage
    }


    //variables
    Animator anim;
    BoarState currentState;
    Vector3 targetPos;
    //wandering variarables;
    Vector3 wanderingSphere;
    Vector3 originPos;
    UnityEngine.AI.NavMeshHit navHitPos;
    //Stat variables
    int health;


	// Use this for initialization
	void Start ()
    { 
        anim = GetComponent<Animator>();
        originPos= transform.position;
	}
	



	// Update is called once per frame
	void Update ()
    {
        //targetPos = //eventmanager passed pos



        //StateMachine
        switch (currentState)
        {
            case BoarState.Idle:
                {
                    anim.SetBool("Walk", true);
                    currentState = BoarState.Walk;
                }
                break;
            case BoarState.Walk:
                {
                    if (navHitPos.hit == true || navHitPos.position == Vector3.zero)
                    {
                        navHitPos.hit = false;
                        Vector3 randDirection = UnityEngine.Random.insideUnitSphere * 4f;
                        UnityEngine.AI.NavMesh.SamplePosition(randDirection, out navHitPos, 4f,2);
                        GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(navHitPos.position);
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
