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
    public GameObject Key4;
    public GameObject FightMusic;
    public ParticleSystem pushParticle;
    public ParticleSystem[] spawnParticle;

    SpawnManager spawn;
    StatePatternEnemy unitedStatePattern;
    Animator anim;
    Vector3 magePos;

    [SerializeField]
    int Health;
    int spawnCount;
    float timer;
    //float force;
    //float radius;
    bool alive;
    bool pushPlayer;
    bool inDialogue;
    // Use this for initialization
    void Start ()
    {
        spawn = GetComponent<SpawnManager>();
        unitedStatePattern = GetComponent<StatePatternEnemy>();
        anim = GetComponent<Animator>();
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SFX Volume");
        spawn.enabled = false;

        Health = 25;
        spawnCount = 0;

        timer = 5;
        //force = 5;
        //radius = 10;

        alive = true;
        pushPlayer = false;
        inDialogue = true;
}
	
	// Update is called once per frame
	void Update ()
    {
        if (playerLocation == null)
        {
            playerLocation = GameObject.FindGameObjectWithTag("Player");
        }
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
                //anim.SetLookAtPosition(playerLocation.transform.position);
                if (pushPlayer)
                {
                    playerLocation.SendMessage("ForcePush", magePos);
                    pushParticle.Emit(2000);
                }

                if (timer <= -2)
                {
                    timer = 5;
                    if (spawnCount < 5)
                    {
                        for (int i = 0; i < spawnParticle.Length; i++)
                        {
                            spawnParticle[i].Emit(3000);
                        }
                        spawn.EnemiesHaveSpawned = false;
                        StartCoroutine(spawn.EnemySpawn());
                        spawnCount++;
                    }
                    GetComponent<AudioSource>().PlayOneShot(RaiseDead);
                }
            }
        }
	}

    void Fight()
    {
        inDialogue = false;
        spawn.enabled = true;
        GetComponent<AudioSource>().PlayOneShot(RaiseDead);
        FightMusic.GetComponent<FightMusic>().TurnOn();
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
        if (C.gameObject.tag == "Player" && timer <= 0)
        {
            pushPlayer = true;
        }
    }

    void OnTriggerStay(Collider C)
    {
        if (C.gameObject.tag == "Player")
        {
            if (timer<= 0)
                pushPlayer = true;
            if (inDialogue)
            {
                EventSystem.StoryDialogue();
                    //freeze player
                Time.timeScale = 0.1f;
            }
            else
            {
                Time.timeScale = 1;
                Debug.Log("Yep");
            }
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
        Debug.Log("test");
        EventSystem.UI_KeyChange(1);
        anim.SetTrigger("Die");
        FightMusic.GetComponent<FightMusic>().TurnOff();
    }

    public void ResetToIdle()
    {
        anim.SetBool("Idle", true);
    }
}
