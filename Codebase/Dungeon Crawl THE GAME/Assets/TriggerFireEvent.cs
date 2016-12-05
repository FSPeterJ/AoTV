using UnityEngine;
using System.Collections;

public class TriggerFireEvent : MonoBehaviour
{
    public GameObject Wowser;
    public GameObject Mario;

    void OnTriggerEnter(Collider col)
    {
        return;
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
        GetComponent<ParticleSystem>().enableEmission = true;
    }
    public void DisableParticleSystem()
    {
        GetComponent<ParticleSystem>().enableEmission = false;
    }
}
