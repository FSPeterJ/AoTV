using UnityEngine;
using System.Collections;

public static class EventSystem {

    //Player Movements
    public delegate void PlayerPositionUpdateHandler(Vector3 PlayerPosition);
    public static event PlayerPositionUpdateHandler onPlayerPositionUpdate;
    public static void PlayerPositionUpdate(Vector3 PlayerPosition)
    {
        if (onPlayerPositionUpdate != null) onPlayerPositionUpdate(PlayerPosition);
    }


    //Mouse to world events
    public delegate void MousePositionUpdateHandler(Vector3 MousePosition);
    public static event MousePositionUpdateHandler onMousePositionUpdate;
    public static void MousePositionUpdate(Vector3 MousePosition)
    {
        if (onMousePositionUpdate != null) onMousePositionUpdate(MousePosition);
       
    }

}
