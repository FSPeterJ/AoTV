using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericProjectile : MonoBehaviour, IWeaponBehavior
{


    public GameObject ImpactEffect;
    public float speed = 1;
    //public bool gravity = false;
    public bool destoryOnImpact = true;


    public enum teams
    {
        Enemy, Ally, Neutral
    }

    public teams team;

    List<int> damagedUnits = new List<int>();

    void Start()
    {
        enabled = true;
    }

    public void Update()
    {
        transform.localPosition += transform.forward * speed;
    }

    void OnTriggerStay(Collider other)
    {

        if (team != teams.Enemy)
        {
            if (other.gameObject.tag == "Enemy" && !damagedUnits.Contains(other.gameObject.GetInstanceID()))
            {
                other.gameObject.GetComponentInParent<IEnemyBehavior>().TakeDamage();
                //Prevent multiple hits per second.
                damagedUnits.Add(other.gameObject.GetInstanceID());
                if (ImpactEffect)
                {
                    Instantiate(ImpactEffect, transform.position, transform.rotation);
                }
                if (destoryOnImpact)
                    Destroy(gameObject);
            }
        }

        if (team != teams.Ally)
        {
            if (other.gameObject.tag == "Player" && !damagedUnits.Contains(other.gameObject.GetInstanceID()))
            {
                other.gameObject.GetComponent<Player>().TakeDamage();
                //Prevent multiple hits per second.
                damagedUnits.Add(other.gameObject.GetInstanceID());
                if (ImpactEffect)
                {
                    Instantiate(ImpactEffect, transform.position, transform.rotation);
                }
                if (destoryOnImpact)
                    Destroy(gameObject);
            }
        }
        if(other.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            if (ImpactEffect)
            {
                Instantiate(ImpactEffect, transform.position, transform.rotation);
            }
            if (destoryOnImpact)
                Destroy(gameObject);
        }
    }


    public void AttackStart()
    {
        damagedUnits.Clear();
    }
    public void AttackEnd()
    {
        damagedUnits.Clear();
    }

    public void ResetAttack()
    {
        damagedUnits.Clear();
    }
    public void ImpactAttack(bool enabled)
    {

    }
}
