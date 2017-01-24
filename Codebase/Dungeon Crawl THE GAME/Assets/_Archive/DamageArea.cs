using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour {

    int damage = 1;
    string tag = "";
    float damageCycle = 1f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionStay(Collision collision)
    {

    }


    void EnableArea(float _damagecycle = 1f,  int _damage = 1, string _tag = "") {
        enabled = true;
        damageCycle = _damagecycle;
        damage = _damage;
        tag = _tag;
    }

    void DisableArea()
    {
        enabled = false;
    }
}
