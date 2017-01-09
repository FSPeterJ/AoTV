using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGolem : MonoBehaviour {
    Animator anim;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {


        if (Input.GetKeyUp(KeyCode.Space))
            anim.SetBool("Jump", true);


        else if (Input.GetKey(KeyCode.S))
            anim.SetBool("Fly Forward", true);
        else if (Input.GetKey(KeyCode.D))
            anim.SetBool("Die", true);
    }


}
