using System.Collections;
using UnityEngine;

public class movePlatform : MonoBehaviour
{
    [SerializeField]
    Transform start, destination;
    bool swap;

    public float speed;


    // Use this for initialization
    void Start()
    {
        swap = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= destination.position.x + 1 && transform.position.x >= destination.position.x - 1)
            swap = false;
        else if (transform.position.x <= start.position.x + 1 && transform.position.x >= start.position.x - 1)
            swap = true;
        Move();
    }

    void Move()
    {
        if (swap)
            transform.position = Vector3.Lerp(transform.position, destination.position, speed * Time.deltaTime);
        else
            transform.position = Vector3.Lerp(transform.position, start.position, speed * Time.deltaTime);
    }
}
