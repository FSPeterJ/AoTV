using UnityEngine;
using System.Collections;

public class togglePlatform : MonoBehaviour
{
    Transform start;
    public Transform destination;
    bool swap;

    public float speed;


    void Start()
    {
        start = transform;
        destination.parent = null;
    }
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (transform.position.x < start.position.x + 0.1 && transform.position.x > start.position.x - 0.1)
            swap = true;
        else if (transform.position.x < destination.position.x + 0.1 && transform.position.x > destination.position.x + 0.1)
            swap = false;

        if (swap)
            transform.position = Vector3.Lerp(transform.position, destination.position, speed * Time.deltaTime);
        else
            transform.position = Vector3.Lerp(destination.position, start.position, speed * Time.deltaTime);

        //transform.position = Vector3.Lerp(transform.position, startVector, speed * Time.deltaTime);
    }
}