using UnityEngine;
using System.Collections;

public class Doge_ba : MonoBehaviour
{
    //GameObject variables
    public float DogeSpeed = 5.0f;
    public float attack;
    public bool IsAlive = true;
    public bool AttackMode = false;
    public int i = 0;
    public GameObject mario;
    float distance;


    //Movement variables
    UnityEngine.AI.NavMeshAgent dogeba; //GameObject that will be moving

    //take a vector of waypoints(gameobjects)
    //inside start function, use function FindGameObjectsWithtag
    void Start()
    {
         dogeba = GetComponent<UnityEngine.AI.NavMeshAgent>();
        //  Vector3[] destinations = { Destination1.transform.position, Destination2.transform.position, Destination3.transform.position };
        dogeba.destination = mario.transform.position;
    }
    void Update()
    {
        if (IsAlive)
        {            
            if (dogeba.remainingDistance < 20)
                dogeba.speed = 3;
            else
                dogeba.speed = 0f;
            dogeba.destination = mario.transform.position;
        }
    }

}
