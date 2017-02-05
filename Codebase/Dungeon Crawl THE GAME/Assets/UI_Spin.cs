using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Spin : UI_Cooldown
{


    void OnEnable()
    {
        EventSystem.onSpinCooldown += CooldownUpdate;
    }
    //unsubscribe
    void OnDisable()
    {
        EventSystem.onSpinCooldown -= CooldownUpdate;

    }
}
