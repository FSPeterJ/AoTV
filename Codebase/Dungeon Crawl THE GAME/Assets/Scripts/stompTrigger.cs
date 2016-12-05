using UnityEngine;
using System.Collections;

public class stompTrigger : MonoBehaviour {


    public GameObject Wowser;
    public GameObject Mario;

    bool PlayerInRange;

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("Entered");
        if (col.tag == "Player") {
            if (Wowser.GetComponent<Wowser>().CurrentState == BossStates.Moving)
            {

                Wowser.GetComponent<Wowser>().PlayerInRange = true;
                Wowser.GetComponent<Wowser>().CurrentState = BossStates.Stomp;
                Debug.Log("Entered");
            }

        }
    }




    public void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            Wowser.GetComponent<Wowser>().PlayerInRange = false;
        }
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
