public class UI_Ranged : UI_Cooldown
{
    private void OnEnable()
    {
        EventSystem.onRangedCooldown += CooldownUpdate;
    }

    //unsubscribe
    private void OnDisable()
    {
        EventSystem.onRangedCooldown -= CooldownUpdate;
    }
}