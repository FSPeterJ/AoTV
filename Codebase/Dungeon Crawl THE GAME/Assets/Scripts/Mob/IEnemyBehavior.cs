using UnityEngine;

public interface IEnemyBehavior {

    void TakeDamage(int damage = 1);

    int RemainingHealth();

    void Kill();

    void ResetToIdle();
}
