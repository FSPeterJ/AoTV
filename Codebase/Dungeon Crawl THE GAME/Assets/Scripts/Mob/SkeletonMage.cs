using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMage : MonoBehaviour, IEnemyBehavior {
    public AudioClip RaiseDead;
    SpawnManager spawn;
    Animator anim;
    StatePatternEnemy unitedStatePattern;
    public GameObject playerLocation;
    float timer = 5;
    int spawnCount;
    Vector3 magePos;
    float force = 5;
    float radius = 10;
    public int Health = 25;
    bool alive = true;
    bool pushPlayer = false;
    bool inDialogue = true;
    // Use this for initialization
    void Start ()
    {
        spawn = GetComponent<SpawnManager>();
        unitedStatePattern = GetComponent<StatePatternEnemy>();
        anim = GetComponent<Animator>();
        spawnCount = 0;
        spawn.enabled = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (alive && !inDialogue)
        {
            timer -= Time.deltaTime;
            magePos = gameObject.transform.position;
            if (timer > 0)
            {
                anim.SetTrigger("Chanting");
                anim.SetLookAtPosition(playerLocation.transform.position);
            }

            if (timer <= 0)
            {
                anim.ResetTrigger("Chanting");
                anim.SetTrigger("Raise Dead");
                anim.SetLookAtPosition(playerLocation.transform.position);
                if (pushPlayer)
                    playerLocation.SendMessage("ForcePush", magePos);
                if (spawnCount < 5)
                {
                    if (timer <= -2)
                    {
                        GetComponent<AudioSource>().PlayOneShot(RaiseDead);
                        spawn.EnemiesHaveSpawned = false;
                        timer = 5;
                        spawnCount++;
                    }
                }
            }
        }
	}

    void Fight()
    {
        inDialogue = false;
        spawn.enabled = true;
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

    void OnTriggerEnter(Collider C)
    {
        if (C.gameObject.tag == "Player")
        {
            pushPlayer = true;
        }
    }

    void OnTriggerStay(Collider C)
    {
        if (C.gameObject.tag == "Player")
        {
            pushPlayer = true;
        }
    }

    void OnTriggerExit(Collider C)
    {
        if (C.gameObject.tag == "Player")
        {
            pushPlayer = false;
        }
    }

    public void TakeDamage(int damage = 1)
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SFX Volume");
        GetComponent<AudioSource>().Play();
        if (RemainingHealth() <= 0)
        {
            //hud.UpdateScore();
            alive = false;
            Kill();
        }
        else
        {
            anim.SetBool("Take Damage", true);
            Health -= damage;
        }
    }

    public int RemainingHealth()
    {
        return Health;
    }

    public void Kill()
    {
        //navMeshAgent.enabled = false;        
        //anim.Stop();        
        anim.SetTrigger("Die");
        //Transform.Destroy(gameObject);
    }

    public void ResetToIdle()
    {
        anim.SetBool("Idle", true);
    }
}
