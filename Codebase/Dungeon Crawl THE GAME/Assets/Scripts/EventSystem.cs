using UnityEngine;
using System.Collections;

public static class EventSystem
{
    public delegate void PlayerPositionUpdateHandler(Vector3 pLevelToLoad);
    public static event PlayerPositionUpdateHandler onPlayerPositionUpdate;
    public static void PlayerPositionUpdate(Vector3 pLevelToLoad)
    {
        if (onPlayerPositionUpdate != null) onPlayerPositionUpdate(pLevelToLoad);
    }


}
