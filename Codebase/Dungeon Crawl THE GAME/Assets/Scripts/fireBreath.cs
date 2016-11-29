using UnityEngine;
using System.Collections;

public class fireBreath : MonoBehaviour
{
    public GameObject Bowser;

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Player")
        {
            if (transform.parent.GetComponent<Wowser>().CurrentState == BossStates.Moving)
                transform.parent.GetComponent<Wowser>().CurrentState = BossStates.FireBreath;
        }
    }
}
