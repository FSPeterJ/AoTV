using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveSys : MonoBehaviour {

    float timePassed = 0;
    float lifetime = 1.5f;

    // Use this for initialization
    void Start()
    {
        transform.GetChild(0).GetComponent<IWeaponBehavior>().AttackStart();
        transform.GetChild(0).GetComponent<IWeaponBehavior>().ImpactAttack(true);
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;

        if (timePassed > lifetime)
        {
            Destroy(gameObject);
        }
    }
}
