using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventDelegates {

    public delegate void PlayerPositionUpdateHandler(Vector3 pLevelToLoad);
    public static event PlayerPositionUpdateHandler onPlayerPositionUpdate;
    public static void PlayerPositionUpdate(Vector3 pLevelToLoad) {
        if (onPlayerPositionUpdate != null) onPlayerPositionUpdate(pLevelToLoad);
    }

}
