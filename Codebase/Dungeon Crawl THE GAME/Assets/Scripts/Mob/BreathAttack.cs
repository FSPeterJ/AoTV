using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathAttack : MonoBehaviour {
    float timePassed = 0;
    float lifetime = 2f;
    ParticleSystem.EmissionModule particlesEM;
    IWeaponBehavior damage;
    // Use this for initialization
    void Start () {
        particlesEM = transform.GetComponent<ParticleSystem>().emission;
        damage = transform.GetComponent<IWeaponBehavior>();
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void StartAttack()
    {
        damage.AttackStart();
        particlesEM.enabled = true;
    }

    public void EndAttack()
    {
        damage.AttackEnd();
        particlesEM.enabled = false;
    }
}
