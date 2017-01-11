using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallScript : MonoBehaviour
{

    float SpawnTime;
    // Use this for initialization
    void Start()
    {
        SpawnTime = Time.time;
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "BigWall")
            Destroy(gameObject);
        //if (col.tag == "Mario")
        //{
        //    Destroy(gameObject);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - SpawnTime >= 4)
            Destroy(gameObject);
    }
}
