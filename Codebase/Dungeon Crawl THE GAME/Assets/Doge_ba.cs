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

    //variables to tell the gameobject where to go
    //only destination1 and 4 are currently being used
    public GameObject Destination1;
    public GameObject Destination2;
    public GameObject Destination3;
    public GameObject Destination4;
    


    //Movement variables
    NavMeshAgent dogeba; //GameObject that will be moving
    float distance; //variable that holds the distance between the gameobject and the destination


    void Start()
    {
        //giving dogeba the NavMeshComponent
         dogeba = GetComponent<NavMeshAgent>();
        //setting the destination as soon as the game starts
        dogeba.SetDestination(Destination1.transform.position);
        
    }
    void Movement()
    {
        //giving distance variable the remaining distance between the gameobject and the destination
        distance = dogeba.remainingDistance;
        
        //checking to see if the distance is less than 1.5 
        if (distance < 1.5f)
        {
            //if so, set the destination back to it's original position
            dogeba.SetDestination(Destination4.transform.position);

            //finally, if the distance between the game object and the destination is greater than 1.0
            //the game object goes back to it's original position
            if (distance > 1.0f)
            {
                dogeba.SetDestination(Destination1.transform.position);
            }
        }
    }

    void Update()
    {
        if (IsAlive)
        {
            Movement();
        }
    }
}
