using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    //player damage events
    public delegate void PlayerHealthUpdateHandler(int hp);
    public static event PlayerHealthUpdateHandler onPlayerHealthUpdate;
    public static void PlayerHealthUpdate(int hp)
    {
        
        if (onPlayerHealthUpdate != null) onPlayerHealthUpdate(hp);


        
}

    //player Died
    public delegate void PlayerDeathHandler();
    public static event PlayerDeathHandler onPlayerDeath;
    public static void PlayerDeath()
    {
        if (onPlayerDeath != null) onPlayerDeath();
        

}
    //Player Score
    public delegate void PlayerScorehandler(int score);
    public static event PlayerScorehandler onPlayerScore;
    public static void IncScore(int score)
    {
        if (onPlayerScore != null) onPlayerScore(score);
    }


}
