using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Ranged : UI_Cooldown
{


    void OnEnable()
    {
        EventSystem.onRangedCooldown += CooldownUpdate;
    }
    //unsubscribe
    void OnDisable()
    {
        EventSystem.onRangedCooldown -= CooldownUpdate;

    }
}
