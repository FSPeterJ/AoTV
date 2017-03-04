public class UI_Teleport : UI_Cooldown
{
    private void OnEnable()
    {
        EventSystem.onTeleportCooldown += CooldownUpdate;
    }

    //unsubscribe
    private void OnDisable()
    {
        EventSystem.onTeleportCooldown -= CooldownUpdate;
    }
}