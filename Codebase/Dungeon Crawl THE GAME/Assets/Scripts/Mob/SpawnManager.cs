using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    public GameObject SkeletonKnight;
    public GameObject GraveOne;
    public GameObject GraveTwo;
    public Collider Graveryard;
	// Use this for initialization
	void Start ()
    {
        StartCoroutine(EnemySpawn());
	}
	
    IEnumerator EnemySpawn()
    {
        while (true)
        {
            Instantiate(SkeletonKnight, GraveOne.transform.position, Quaternion.identity);
            Instantiate(SkeletonKnight, GraveTwo.transform.position, Quaternion.identity);
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
