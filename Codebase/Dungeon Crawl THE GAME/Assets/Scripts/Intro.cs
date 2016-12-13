using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Intro : MonoBehaviour
{
    public float camSpeed = 5.0f;
    public Transform DesA;
    public Transform DesB;
    public Transform[] DesPoints;
    float distance;
    NavMeshAgent camera;
    void Start()
    {
        camera = GetComponent<NavMeshAgent>();
        camera.transform.position = DesA.position;
        camera.destination = DesB.position;
    }

    void Path()
    {
        
        distance = camera.remainingDistance;

        if (distance < 1.5f)
        {
            camSpeed = 3.0f;
            for (int i = 0; i < DesPoints.Length; ++i)
            {
                camera.SetDestination(DesPoints[i].transform.position);

                if (distance > 1.0f)
                {
                    camSpeed = 5.0f;
                    camera.SetDestination(DesPoints[i].transform.position);
                }
            }
        }
    }
}
