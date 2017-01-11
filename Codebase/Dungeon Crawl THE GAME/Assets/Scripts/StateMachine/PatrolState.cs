using UnityEngine;

public class PatrolState : IEnemyState

{
    private readonly StatePatternEnemy enemy;//Keeps track of the current enemy state
    private int nextWayPoint;//Count through the way points
    public float heightMultiplier;
    public float sightDistance = 10;
    public PatrolState(StatePatternEnemy statePatternEnemy)//constructor
    {
        enemy = statePatternEnemy;//when an instance is created pass in the current enemy state pattern.
    }

    public void UpdateState()
    {
        Look();//Look every frame
        Patrol();//Patrol Every frame
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))//if an object tagged with player collides with our trigger
            ToAlertState();//go to alert state
    }

    public void ToPatrolState()
    {
        Debug.Log("Can't transition to same state");//Can't transition to same state, but must have this function for the interface
    }

    public void ToAlertState()
    {
        enemy.currentState = enemy.alertState;//change current state to alert state
    }

    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;//change current state to chase state
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

    private void Patrol()
    {
        enemy.meshRendererFlag.material.color = Color.green;//While in the patrol state turn green(not imperative)
        enemy.navMeshAgent.destination = enemy.wayPoints[nextWayPoint].transform.position;//Destination is equal to next way point in the array
        enemy.navMeshAgent.Resume();//Start walking towards new destination

        if (enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance && !enemy.navMeshAgent.pathPending)//if reached current way point
        {
            nextWayPoint = (nextWayPoint + 1) % enemy.wayPoints.Length;//loops back around if reached last way point
        }
    }
}