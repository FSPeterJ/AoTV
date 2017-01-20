using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericMeleeDamage : MonoBehaviour, IWeaponBehavior
{

    public enum teams
    {
        Enemy, Ally, Neutral
    }
        
    public teams attackTeam;

    bool attacking = false;
    List<int> damagedUnits = new List<int>();


    private void OnTriggerStay(Collider other)
    {
        if (attacking)
        {
            if (attackTeam != teams.Enemy)
            {
                if (other.gameObject.tag == "Enemy" && !damagedUnits.Contains(other.gameObject.GetInstanceID()))
                {
                    other.gameObject.GetComponent<IEnemyBehavior>().TakeDamage();
                    //Prevent multiple hits per second.
                    damagedUnits.Add(other.gameObject.GetInstanceID());
                }
            }

            else
            {
                if (other.gameObject.tag == "Player" && !damagedUnits.Contains(other.gameObject.GetInstanceID()))
                {
                    other.gameObject.GetComponent<IEnemyBehavior>().TakeDamage();
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
