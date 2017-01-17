using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float time = 0;

    // Use this for initialization
    void Start ()
    {
    }

    // Update is called once per frame
    void Update () {
        time += Time.deltaTime;
        //transform.position = arrowposti;
        if (time > 10)
        {
            Destroy(gameObject);
        }
        
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            other.gameObject.GetComponent<Player>().TakeDamage(2);
            other.gameObject.GetComponent<Animator>().SetTrigger("Take Damage");
            Destroy(gameObject);
        }
    }
}
