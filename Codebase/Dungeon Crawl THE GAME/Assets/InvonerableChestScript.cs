using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvonerableChestScript : MonoBehaviour {
    Animator anim;
    public GameObject ScorePowerUp;
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("Open", false);
    }
    void OnTriggerEnter(Collider Col)
    {
        if (Col.tag == "Player")
        {
            anim.SetBool("Open", true);
            Vector3 spawnLocation = transform.position;

            spawnLocation.x += 4;
            //waiting on ryans object
         //   Instantiate(InvulnerablePowerUp, spawnLocation, Quaternion.identity);
        }
    }
}
