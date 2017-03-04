using UnityEngine;

public class togglePlatform : MonoBehaviour
{
    private Transform start;
    public Transform destination;
    private bool swap;

    public float speed;

    private void Start()
    {
        start = transform;
        destination.parent = null;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
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