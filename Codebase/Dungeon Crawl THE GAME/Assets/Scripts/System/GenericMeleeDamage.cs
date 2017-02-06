﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericMeleeDamage : MonoBehaviour, IWeaponBehavior
{


    public GameObject ImpactEffect;
    public enum teams
    {
        Enemy, Ally, Neutral
    }
        
    public teams team;

    bool attacking = false;
    List<int> damagedUnits = new List<int>();

    void Start()
    {
        enabled = false;
    }


    void OnTriggerStay(Collider other)
    {
        if (attacking)
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
                }
            }

            if(team != teams.Ally) 
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
                }
            }
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

    public void ResetAttack()
    {
        damagedUnits.Clear();
    }
}
