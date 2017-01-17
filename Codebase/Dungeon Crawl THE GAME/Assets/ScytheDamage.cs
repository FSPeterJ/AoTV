using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheDamage : MonoBehaviour, IWeaponBehavior {

    bool attacking = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if(attacking && other.gameObject.layer == LayerMask.NameToLayer("ENEMY"))
        {
            other.gameObject.GetComponent<IEnemyBehavior>().TakeDamage();
        }
    }


    public void AttackStart()
    {
        attacking = true;

    }
    public void AttackEnd()
    {
        attacking = false;
    }
}
