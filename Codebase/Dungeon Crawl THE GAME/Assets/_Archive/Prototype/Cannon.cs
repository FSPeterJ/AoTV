using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour
{
    public Rigidbody Cannonball;
    //Transform SpawnPoint;
    public float firerate = 0.5f;
    private float nextfire = 5.0f;
    public float speed = 20;
    //float SpawnTime;
	
    void Start()
    {
       // SpawnTime = Time.time;
    }

	// Update is called once per frame
	void Update()
    {
	    if (true)
        {
            Rigidbody cannonball = (Rigidbody)Instantiate(Cannonball, transform.position, transform.rotation);
            //cannonball.transform.position = SpawnPoint.position;
            cannonball.velocity = transform.TransformDirection(new Vector3(0, 0, speed));

            if (Time.time > nextfire)
            {
                nextfire = Time.time + firerate;
                Destroy(cannonball);
              //  GameObject clone = (GameObject)Instantiate(Cannonball, transform.position, transform.rotation);
            }
        }
	}
}
