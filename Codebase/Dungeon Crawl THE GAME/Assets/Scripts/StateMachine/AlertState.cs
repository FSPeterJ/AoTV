using UnityEngine;

public class AlertState : IEnemyState

{
    private readonly StatePatternEnemy enemy;//Keeps track of the current enemy state
    private float searchTimer;//current amount of time searched
    public float heightMultiplier;
    public float sightDistance = 10;
    public AlertState(StatePatternEnemy unitedStatePattern)//constructor
    {
        enemy = unitedStatePattern;//when an instance is created pass in the current enemy state pattern.
    }

    public void UpdateState()
    {
        Look();//Look every frame
        Search();//Search Every frame
    }

    public void OnTriggerEnter(Collider other)
    {
    }

    public void ToPatrolState()
    {
        enemy.currentState = enemy.patrolState;//change to patrol state
        searchTimer = 0f;//set the current of amount of time searched to zero
        Debug.Log("Alert -> Patrol");//Can't transition to same state, but must have this function for the interface
    }

    public void ToAlertState()
    {
        Debug.Log("In Alert State");//Can't transition to same state, but must have this function for the interface
    }

    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;//change current state to chase state
        Debug.Log("Alert -> Chase");//Can't transition to same state, but must have this function for the interface

    }

    private void Look()//In patrol state, ray cast 20 units from his eyes
    {
        RaycastHit hit;

        Debug.DrawRay(enemy.eyes.transform.position + Vector3.up * heightMultiplier, enemy.eyes.transform.forward * sightDistance, Color.red);
        Debug.DrawRay(enemy.eyes.transform.position + Vector3.up * heightMultiplier, (enemy.eyes.transform.forward + enemy.eyes.transform.right).normalized * sightDistance, Color.red);
        Debug.DrawRay(enemy.eyes.transform.position + Vector3.up * heightMultiplier, (enemy.eyes.transform.forward - enemy.eyes.transform.right).normalized * sightDistance, Color.red);

        if (Physics.Raycast(enemy.eyes.transform.position + Vector3.up * heightMultiplier, enemy.eyes.transform.forward, out hit, sightDistance))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                enemy.chaseTarget = hit.transform;//Make the target of the chase the thing the ray cast hit
                ToChaseState();//transfer to chase state
            }
        }
        if (Physics.Raycast(enemy.eyes.transform.position + Vector3.up * heightMultiplier, (enemy.eyes.transform.forward + enemy.eyes.transform.right).normalized, out hit, sightDistance))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                enemy.chaseTarget = hit.transform;//Make the target of the chase the thing the ray cast hit
                ToChaseState();//transfer to chase state
            }
        }
        if (Physics.Raycast(enemy.eyes.transform.position + Vector3.up * heightMultiplier, (enemy.eyes.transform.forward - enemy.eyes.transform.right).normalized, out hit, sightDistance))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                enemy.chaseTarget = hit.transform;//Make the target of the chase the thing the ray cast hit
                ToChaseState();//transfer to chase state
            }
        }
    }

    private void Search()
    {
        //enemy.meshRendererFlag.material.color = Color.yellow;//While in the alert state turn yellow(not imperative)
        enemy.navMeshAgent.Stop();//stop moving
        enemy.transform.Rotate(0, enemy.searchingTurnSpeed * Time.deltaTime, 0);//rotate around the y axis for the amount of search time
        searchTimer += Time.deltaTime;//increase searchTimer

        if (searchTimer >= enemy.searchingDuration)//If the amount of time searched reaches the max search time
            ToPatrolState();//Transfer to patrol state
    }
}