using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogeBaHitMario : MonoBehaviour {

    public GameObject mario;

	void OnTriggerEnter(Collider Col)
    {
        if (Col.tag == "Player")
        {
            mario.transform.position = new Vector3(2.6f, 10, 81.51f);
            mario.GetComponent<BasePlayer>().TakeDamage();
        }
        if (Col.tag =="Fireball")
        {
            Destroy(gameObject);
        }
    }
}
