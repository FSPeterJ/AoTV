using UnityEngine;

public interface IEnemyBehavior {

    void TakeDamage(int damage);

    int RemainingHealth();

    void Kill();

}
