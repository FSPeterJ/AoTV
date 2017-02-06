using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Teleport : UI_Cooldown {


    void OnEnable()
    {
        EventSystem.onTeleportCooldown += CooldownUpdate;
    }
    //unsubscribe
    void OnDisable()
    {
        EventSystem.onTeleportCooldown -= CooldownUpdate;

    }
}
