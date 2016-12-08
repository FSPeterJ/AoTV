using UnityEngine;
using System.Collections;

public class TriggerFireEvent : MonoBehaviour
{
    public GameObject Wowser;
    public GameObject Mario;
    ParticleSystem.EmissionModule em;

    private void Start()
    {
         em = GetComponent<ParticleSystem>().emission;
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
            if (Wowser.GetComponent<Wowser>().CurrentState == BossStates.Moving)
                Wowser.GetComponent<Wowser>().CurrentState = BossStates.FireBreath;
    }


    public void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
            if (Wowser.GetComponent<Wowser>().isFireBreath)
                Mario.GetComponent<BasePlayer>().TakeFireDamage();
    }
    public void EnableParticleSystem()
    {
        em.enabled = true;
    }
    public void DisableParticleSystem()
    {


        
        em.enabled = false;

    }
}
