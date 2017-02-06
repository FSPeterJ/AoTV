using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public static class EventSystem
{

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
    public delegate void PlayerHealthUpdateHandler(int hp, int hpmax);
    public static event PlayerHealthUpdateHandler onPlayerHealthUpdate;
    public static void PlayerHealthUpdate(int hp, int hpmax)
    {

        if (onPlayerHealthUpdate != null) onPlayerHealthUpdate(hp, hpmax);
    }

    //player Died
    public delegate void PlayerDeathHandler();
    public static event PlayerDeathHandler onPlayerDeath;
    public static void PlayerDeath()
    {
        if (onPlayerDeath != null) onPlayerDeath();
    }

    //Teleport Cooldown
    public delegate void TeleportCooldownHandler(float seconds, float max);
    public static event TeleportCooldownHandler onTeleportCooldown;
    public static void TeleportCooldown(float seconds, float max)
    {
        if (onTeleportCooldown != null) onTeleportCooldown(seconds, max);
    }


    //Ranged Cooldown
    public delegate void RangedCooldownHandler(float seconds, float max);
    public static event RangedCooldownHandler onRangedCooldown;
    public static void RangedCooldown(float seconds, float max)
    {
        if (onRangedCooldown != null) onRangedCooldown(seconds, max);
    }

    //Spin Cooldown
    public delegate void SpinCooldownHandler(float seconds, float max);
    public static event SpinCooldownHandler onSpinCooldown;
    public static void SpinCooldown(float seconds, float max)
    {
        if (onSpinCooldown != null) onSpinCooldown(seconds, max);
    }

}
