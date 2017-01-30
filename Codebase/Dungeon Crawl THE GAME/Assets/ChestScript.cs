using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour {
    Animator anim;
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
            //instaniate health object
        }
    }
}
