using UnityEngine;
using System.Collections;

public class TriggerFireEvent : MonoBehaviour {
    public GameObject Wowser;


    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            if (Wowser.GetComponent<Wowser>().CurrentState == BossStates.Moving)

                Wowser.GetComponent<Wowser>().CurrentState = BossStates.FireBreath;
        }
    }
}
