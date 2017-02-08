using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBall : MonoBehaviour {

    float SpawnTime;
    // Use this for initialization
    void Start()
    {
        SpawnTime = Time.time;
    }
    void OnCollisionEnter(Collision col)
    {
       
        Destroy(gameObject);
    }
  
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            Destroy(gameObject);
            //deal damage here
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.time - SpawnTime >= 4)
            Destroy(gameObject);
    }
}
