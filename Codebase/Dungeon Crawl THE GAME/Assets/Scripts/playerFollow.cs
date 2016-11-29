using UnityEngine;
using System.Collections;

public class playerFollow : MonoBehaviour
{
    public GameObject player;
    Vector3 offset;

    // Use this for initialization
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = player.transform.position + offset;
        transform.position = newPos;
    }
}
