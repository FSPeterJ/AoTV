using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonKnight : MonoBehaviour{

    Animator anim;
    bool asleep = true;
    StatePatternEnemy unitedStatePattern;
    double lengthOfClip = 0;
	// Use this for initialization
	void Start ()
    {
        unitedStatePattern = GetComponent<StatePatternEnemy>();
        anim = GetComponent<Animator>();
       //StartCoroutine(WakeMeUpInside());
    }

    // Update is called once per frame
    void Update ()
    {
        while (!asleep)
        {
            if (unitedStatePattern.currentState.ToString() == "PatrolState")
            {
                anim.SetBool("Run", false);
                anim.SetBool("Defend", false);
                anim.SetBool("Walk", true);
            }
            if (unitedStatePattern.currentState.ToString() == "AlertState")
            {
                anim.SetBool("Walk", false);
                anim.SetBool("Run", false);
                anim.SetBool("Defend", true);
            }
            if (unitedStatePattern.currentState.ToString() == "ChaseState")
            {
                anim.SetBool("Defend", false);
                anim.SetBool("Walk", false);
                anim.SetBool("Run", true);
            }
        }
    }

    //IEnumerator WakeMeUpInside()
    //{
    //    while (asleep)
    //    {
    //        anim.SetBool("Die", true);
    //        yield return new WaitForSeconds(5);
    //        asleep = false;
    //    }
    //}
}
