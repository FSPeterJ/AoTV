using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float time = 0;
    Rigidbody body;
    // Use this for initialization
    void Start ()
    {
        body = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update () {
        time += Time.deltaTime;
        body.AddForce(transform.forward);
        body.freezeRotation = true;
        if (time > 4)
        {
            Destroy(gameObject);
        }
        
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            Destroy(gameObject);
            other.gameObject.GetComponent<Player>().TakeDamage(2);
            other.gameObject.GetComponent<Animator>().SetTrigger("Take Damage");
        }
    }
}
