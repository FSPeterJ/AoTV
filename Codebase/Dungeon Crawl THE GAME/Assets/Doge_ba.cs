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
    public GameObject Destination1;
    public GameObject Destination2;
    public GameObject Destination3;
    public GameObject Destination4;
    float distance;


    //Movement variables
    NavMeshAgent dogeba; //GameObject that will be moving

    //take a vector of waypoints(gameobjects)
    //inside start function, use function FindGameObjectsWithtag
    void Start()
    {
         dogeba = GetComponent<NavMeshAgent>();
        //  Vector3[] destinations = { Destination1.transform.position, Destination2.transform.position, Destination3.transform.position };
        dogeba.SetDestination(Destination1.transform.position);
        
    }
    void Movement()
    {
        distance = dogeba.remainingDistance;
        
        if (distance < 1.5f)
        {
            dogeba.SetDestination(Destination4.transform.position);

            if (distance > 1.0f)
            {
                dogeba.SetDestination(Destination1.transform.position);
            }
        }
        
        
           
        
        ////while (IsAlive)
        //{
        //    for (int i = 0; i < destinations.Length; ++i)
        //    {
        //        currDes = destinations[i];
        //    }
        //}
    }

    void Update()
    {
        if (IsAlive)
        {
            Movement();
        }
    }
}
