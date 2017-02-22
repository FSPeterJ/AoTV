using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericMeleeDamage : MonoBehaviour, IWeaponBehavior
{
    bool AddForce = false;
    public GameObject ImpactEffect;
    [SerializeField]
    int Damage = 1;
    public enum teams
    {
        Enemy, Ally, Neutral
    }

    public teams team;
    [SerializeField]
    bool attacking = false;
    List<int> damagedUnits = new List<int>();

    void Start()
    {

    }


    void OnTriggerStay(Collider other)
    {
        if (attacking)
        {
            if (team != teams.Enemy)
            {
                if (other.gameObject.tag == "Enemy" && !damagedUnits.Contains(other.gameObject.GetInstanceID()))
                {
                    other.gameObject.GetComponentInParent<IEnemyBehavior>().TakeDamage(Damage);
                    //Prevent multiple hits per second.
                    damagedUnits.Add(other.gameObject.GetInstanceID());
                    if (ImpactEffect)
                    {
                        Instantiate(ImpactEffect, transform.position, transform.rotation);
                    }
                }
            }

            if (team != teams.Ally)
            {
                if (other.gameObject.tag == "Player" && !damagedUnits.Contains(other.gameObject.GetInstanceID()))
                {
                    if (AddForce == true)
                    {
                        other.gameObject.GetComponent<Player>().AddImpact(transform.position, 1100f);
                    }
                    other.gameObject.GetComponent<Player>().TakeDamage(Damage);
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
        AddForce = false;
        attacking = false;
        damagedUnits.Clear();
    }

    public void ResetAttack()
    {
        damagedUnits.Clear();
    }
    public void ImpactAttack(bool enabled)
    {
        AddForce = enabled;
    }

    public void SetDamage(int dmg)
    {
        Damage = dmg;
    }
}
