using UnityEngine;
using System.Collections;

public class FireballController : MonoBehaviour
{

    float SpawnTime;
    // Use this for initialization
    void Start()
    {
        SpawnTime = Time.time;
    }
    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Fireball Collision");
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - SpawnTime >= 4)
            Destroy(gameObject);
    }
}