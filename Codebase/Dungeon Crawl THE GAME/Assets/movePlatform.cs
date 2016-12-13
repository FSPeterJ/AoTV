using System.Collections;
using UnityEngine;

public class movePlatform : MonoBehaviour
{
    Transform start;
    public Transform destination;
    bool swap;

    public float speed;


    // Use this for initialization
    void Start()
    {
        start = transform;
        swap = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= destination.position.x + 0.5 && transform.position.x >= destination.position.x - 0.5)
            swap = false;
        else if (transform.position.x <= start.position.x + 0.5 && transform.position.x >= start.position.x - 0.5)
            swap = true;
        Move();
    }

    void Move()
    {
        if (swap)
            transform.position = Vector3.Lerp(transform.position, destination.position, speed * Time.deltaTime);
        else
            transform.position = Vector3.Lerp(destination.position, start.position, speed * Time.deltaTime);
    }
}
