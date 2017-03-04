﻿public interface IEnemyBehavior
{
    void TakeDamage(int damage = 1);

    int RemainingHealth();

    void Kill();

    void ResetToIdle();

    string Name();

    float HPOffsetHeight();
}