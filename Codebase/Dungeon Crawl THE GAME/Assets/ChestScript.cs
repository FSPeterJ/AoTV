using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour {
    Animator anim;
    public GameObject ScorePowerUp;
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("Open", false);
    }
    void OnTriggerEnter(Collider Col)
    {
        if(Col.tag == "Player")
        {
            anim.SetBool("Open", true);
            Vector3 spawnLocation = transform.position;
            
            spawnLocation.x += 4;
           // Instantiate(, spawnLocation, Quaternion.identity);
        }
    }
}
