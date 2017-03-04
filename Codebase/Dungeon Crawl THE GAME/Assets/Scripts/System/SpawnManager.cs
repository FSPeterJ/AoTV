using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject EnemyOne;
    public GameObject[] SpawnPoints;
    public Collider Trigger;
    public bool EnemiesHaveSpawned = false;

    // Use this for initialization
    private void Start()
    {
        //StartCoroutine(EnemySpawn());
        Trigger = GetComponent<Collider>();
    }

    public IEnumerator EnemySpawn()
    {
        if (!EnemiesHaveSpawned)
        {
            for (int i = 0; i < SpawnPoints.Length; i++)
            {
                Instantiate(EnemyOne, SpawnPoints[i].transform.position, Quaternion.identity);
            }
            EnemiesHaveSpawned = true;
            yield return new WaitForSeconds(.5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && Trigger.gameObject.name != "Boss")
        {
            StartCoroutine(EnemySpawn());
        }
    }
}