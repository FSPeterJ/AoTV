using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    public GameObject EnemyOne;
    public GameObject[] SpawnPoints;
    public Collider Graveyard;
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
            for (int i = 0; i < SpawnPoints.Length; i++)
            {
                Instantiate(EnemyOne, SpawnPoints[i].transform.position, Quaternion.identity);
            }
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
