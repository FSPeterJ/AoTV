using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreChestScript : MonoBehaviour {
    Animator anim;
    public GameObject ScorePowerUp;
    bool notOpen = true;
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("Open", false);
    }
    void OnTriggerStay(Collider Col)
    {
        if (Col.tag == "Player")
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (notOpen == true)
                {
                    notOpen = false;
                    anim.SetBool("Open", true);
                    Vector3 spawnLocation = transform.position;
                    spawnLocation.x += 4;
                    Instantiate(ScorePowerUp, spawnLocation, Quaternion.identity);
                }
            }
        }
    }
}
