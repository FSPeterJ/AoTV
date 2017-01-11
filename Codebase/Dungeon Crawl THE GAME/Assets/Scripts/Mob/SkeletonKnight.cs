using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonKnight : MonoBehaviour{

    Animator anim;
    AnimatorControllerParameter direction;
    StatePatternEnemy unitedStatePattern;
	// Use this for initialization
	void Start ()
    {
        unitedStatePattern = GetComponent<StatePatternEnemy>();
        anim.SetBool("Die", true);
        unitedStatePattern.navMeshAgent.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    //IEnumerator EnemySpawn()
    //{
    //    while (true)
    //    {
    //        Instantiate(SkeletonKnight, GraveOne.transform.position, Quaternion.identity);
    //        Instantiate(SkeletonKnight, GraveTwo.transform.position, Quaternion.identity);
    //        yield return new WaitForSeconds(5);
    //        EnemiesHaveSpawned = true;
    //    }
    //}
}
