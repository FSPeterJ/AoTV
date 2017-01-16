using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    public GameObject Skeleton_Knight;
    public GameObject GraveOne;
    public GameObject GraveTwo;
    public Collider Graveryard;
    bool EnemiesHaveSpawned = false;
	// Use this for initialization
	void Start ()
    {
        //StartCoroutine(EnemySpawn());
	}
	
    IEnumerator EnemySpawn()
    {
        if (!EnemiesHaveSpawned)
        {
            Instantiate(Skeleton_Knight, GraveOne.transform.position, Quaternion.identity);
            Instantiate(Skeleton_Knight, GraveTwo.transform.position, Quaternion.identity);
            EnemiesHaveSpawned = true;
            yield return new WaitForSeconds(5);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(EnemySpawn());
        }
    }
}
