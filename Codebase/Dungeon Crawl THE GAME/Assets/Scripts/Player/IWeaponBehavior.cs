using UnityEngine;

public interface IWeaponBehavior
{

    void AttackStart();

    void AttackEnd();

    void ResetAttack();
    void ImpactAttack(bool enabled);
}


