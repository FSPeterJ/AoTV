using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericMeleeDamage : MonoBehaviour, IWeaponBehavior
{



    public enum teams
    {
        Enemy, Ally, Neutral
    }
        
    public teams team;

    bool attacking = false;
    List<int> damagedUnits = new List<int>();

    private void Start()
    {
        enabled = false;
    }


    private void OnTriggerStay(Collider other)
    {
        if (attacking)
        {
            if (team != teams.Enemy)
            {
                if (other.gameObject.tag == "Enemy" && !damagedUnits.Contains(other.gameObject.GetInstanceID()))
                {
                    other.gameObject.GetComponent<IEnemyBehavior>().TakeDamage();
                    //Prevent multiple hits per second.
                    damagedUnits.Add(other.gameObject.GetInstanceID());
                }
            }

            if(team != teams.Ally) 
            {
                if (other.gameObject.tag == "Player" && !damagedUnits.Contains(other.gameObject.GetInstanceID()))
                {
                    other.gameObject.GetComponent<Player>().TakeDamage();
                    //Prevent multiple hits per second.
                    damagedUnits.Add(other.gameObject.GetInstanceID());
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
