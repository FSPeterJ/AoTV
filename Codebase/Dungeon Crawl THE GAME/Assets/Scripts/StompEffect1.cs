using UnityEngine;
using System.Collections;

public class StompEffect1 : MonoBehaviour {


    Vector3 StartingSize;
    Vector3 EndingSize;
    public GameObject Wowser;
    public GameObject Mario;
    public GameObject Sphere;

	// Use this for initialization
	void Start () {
        StartingSize = new Vector3(0, 0, 0);
        EndingSize = new Vector3(3,3,3);

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Wowser.GetComponent<Wowser>().StartStompAnimation == true)
        {
            IncreaseSize();
            Vector3 sphereend = new Vector3(Sphere.transform.position.x, Sphere.transform.position.y,0);
            Sphere.transform.position = Vector3.Lerp(Sphere.transform.position, sphereend, 2.5f * Time.deltaTime);

        }
        else
        {
            
            transform.position = new Vector3(Wowser.transform.position.x,Wowser.transform.position.y - 2,Wowser.transform.position.z);
            Vector3 sphereend = new Vector3(Sphere.transform.position.x, Sphere.transform.position.y,10);
            transform.localScale = new Vector3(0, 0, 0);
        }

    }

    void IncreaseSize()
    {
       transform.localScale = Vector3.Lerp(transform.localScale, EndingSize, 2.5f * Time.deltaTime);
    }
}
