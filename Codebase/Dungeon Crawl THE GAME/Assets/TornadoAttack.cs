using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoAttack : MonoBehaviour 
{

    float timepassed;
    public float timeout = 5f;
    ParticleSystem.MainModule particles;
    IWeaponBehavior damage;

	// Use this for initialization
	void Start () {
        particles = transform.GetComponent<ParticleSystem>().main;
        damage = transform.GetComponent<IWeaponBehavior>();
        damage.AttackStart();
    }
	
	// Update is called once per frame
	void Update () {
        if ((timepassed += Time.deltaTime) > timeout)
        {
            particles.loop = false;
            damage.AttackEnd();
        }
    }
}
