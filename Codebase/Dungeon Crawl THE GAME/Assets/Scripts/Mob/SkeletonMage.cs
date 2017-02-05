using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMage : MonoBehaviour {
    public AudioClip RaiseDead;
    SpawnManager spawn;
    Animator anim;
    StatePatternEnemy unitedStatePattern;
    public GameObject playerLocation;
    float timer = 5;
    int spawnCount;

    float force = 5;
    float radius = 10;
    // Use this for initialization
    void Start ()
    {
        spawn = GetComponent<SpawnManager>();
        unitedStatePattern = GetComponent<StatePatternEnemy>();
        anim = GetComponent<Animator>();

        spawnCount = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        timer -= Time.deltaTime;

        if (timer > 0)
        {
            anim.SetTrigger("Chanting");
        }


        if (timer <= 0 && spawnCount < 5)
        {
            anim.ResetTrigger("Chanting");
            anim.SetTrigger("Raise Dead");
            //GetComponent<Rigidbody>().AddExplosionForce(radius, gameObject.transform.position, 5);
            ForcePush();
            GetComponent<AudioSource>().PlayOneShot(RaiseDead);
            spawn.EnemiesHaveSpawned = false;
            timer = 5;
        }
	}

    void ForcePush()
    {
        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position, playerLocation.transform.position - gameObject.transform.position, out hit))
        {

        }
    }

    void CancelCurrentAnimation()
    {
        //if (unitedStatePattern.currentState.ToString() != "PatrolState")
        //{
            anim.SetBool("Walk", false);
        //}
        if (unitedStatePattern.currentState.ToString() != "AlertState")
        {
            anim.SetBool("Chanting", false);
        }
        if (unitedStatePattern.currentState.ToString() != "ChaseState") //|| unitedStatePattern.DistanceToPlayer < stopToAttackDistance)
        {
            anim.SetBool("Run", false);
        }
    }


}
