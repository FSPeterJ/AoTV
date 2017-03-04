public class UI_Spin : UI_Cooldown
{
    private void OnEnable()
    {
        EventSystem.onSpinCooldown += CooldownUpdate;
        EventSystem.onSpinTime += SecondaryCooldownUpdate;
    }

    //unsubscribe
    private void OnDisable()
    {
        EventSystem.onSpinCooldown -= CooldownUpdate;
        EventSystem.onSpinTime -= SecondaryCooldownUpdate;
    }
}