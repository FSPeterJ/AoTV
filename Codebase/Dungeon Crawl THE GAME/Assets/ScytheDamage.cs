using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheDamage : MonoBehaviour, IWeaponBehavior {

    bool attacking = false;
    List<int> damagedUnits = new List<int>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (attacking && other.gameObject.tag == "Enemy" && !damagedUnits.Contains(other.gameObject.GetInstanceID()))
        {
            other.gameObject.GetComponent<IEnemyBehavior>().TakeDamage();
            //Prevent multiple hits per second.
            damagedUnits.Add(other.gameObject.GetInstanceID());
        }
    }


    public void AttackStart()
    {
        damagedUnits.Clear();
        attacking = true;

    }
    public void AttackEnd()
    {
        attacking = false;
        damagedUnits.Clear();
        
    }
}
