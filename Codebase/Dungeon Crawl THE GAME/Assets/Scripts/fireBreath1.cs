using UnityEngine;
using System.Collections;

public class fireBreath : MonoBehaviour
{
    public GameObject Wowser;


    void OnCollisionEnter(Collider col)
    {
        if (col.tag =="Player")
        {
            if (Wowser.GetComponent<Wowser>().CurrentState == BossStates.Moving)
                Wowser.GetComponent<Wowser>().CurrentState = BossStates.FireBreath;
        }
    }
    public bool OnCollisionStay(Collider col)
    {
        if (col.tag =="Player")
        {
            return true;

        }
        return false;
    }
}
