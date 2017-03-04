using System.Collections.Generic;
using UnityEngine;

public class GenericProjectile : MonoBehaviour, IWeaponBehavior
{
    [SerializeField]
    private GameObject ImpactEffect;

    [SerializeField]
    private float speed = 1;

    [SerializeField]
    private bool destoryOnImpact = true;

    [SerializeField]
    private float lifetime = 5f;

    [SerializeField]
    private float timePassed = 0f;

    [SerializeField]
    private int Damage = 1;

    public enum teams
    {
        Enemy, Ally, Neutral
    }

    public teams team;

    private List<int> damagedUnits = new List<int>();

    private void Start()
    {
        enabled = true;
    }

    public void Update()
    {
        transform.localPosition += transform.forward * speed;
        timePassed += Time.deltaTime;

        if (timePassed > lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
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
                if (destoryOnImpact)
                    Destroy(gameObject);
            }
        }

        if (team != teams.Ally)
        {
            if (other.gameObject.tag == "Player" && !damagedUnits.Contains(other.gameObject.GetInstanceID()))
            {
                other.gameObject.GetComponent<Player>().TakeDamage(Damage);
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
        if (other.gameObject.layer == LayerMask.NameToLayer("Terrain") || other.gameObject.layer == LayerMask.NameToLayer("TeleportBlock"))
        {
            if (ImpactEffect)
            {
                Instantiate(ImpactEffect, transform.position, transform.rotation);
            }
            if (destoryOnImpact)
            {
                Destroy(gameObject);
            }
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

    public void SetDamage(int dmg)
    {
        Damage = dmg;
    }
}