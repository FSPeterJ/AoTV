using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMage : MonoBehaviour, IEnemyBehavior
{
    [SerializeField]
    AudioClip RaiseDead;
    [SerializeField]
    GameObject StoryDialoguePanel;
    public GameObject playerLocation;

    SpawnManager spawn;
    StatePatternEnemy unitedStatePattern;
    Animator anim;
    Vector3 magePos;

    [SerializeField]
    int Health;
    int spawnCount;
    float timer;
    float force;
    float radius;
    bool alive;
    bool pushPlayer;
    bool inDialogue;
    // Use this for initialization
    void Start ()
    {
        spawn = GetComponent<SpawnManager>();
        unitedStatePattern = GetComponent<StatePatternEnemy>();
        anim = GetComponent<Animator>();
        spawn.enabled = false;

        Health = 25;
        spawnCount = 0;

        timer = 5;
        force = 5;
        radius = 10;

        alive = true;
        pushPlayer = false;
        inDialogue = true;
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
            if (inDialogue)
            {
                StoryDialoguePanel.SetActive(true);
                //freeze player
            }
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
        anim.SetTrigger("Die");
    }

    public void ResetToIdle()
    {
        anim.SetBool("Idle", true);
    }
}
