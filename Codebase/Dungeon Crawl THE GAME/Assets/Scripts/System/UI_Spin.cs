using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Spin : UI_Cooldown
{


    void OnEnable()
    {
        EventSystem.onSpinCooldown += CooldownUpdate;
        EventSystem.onSpinTime += SecondaryCooldownUpdate;
    }
    //unsubscribe
    void OnDisable()
    {
        EventSystem.onSpinCooldown -= CooldownUpdate;
        EventSystem.onSpinTime -= SecondaryCooldownUpdate;

    }




}
