public interface IEnemyState
{
    void UpdateState();

    void ToPatrolState();

    void ToAlertState();

    void ToChaseState();
}