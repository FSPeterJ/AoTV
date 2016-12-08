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
        EndingSize = new Vector3(1,1,1);

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Wowser.GetComponent<Wowser>().StartStompAnimation == true)
        {
            IncreaseSize();
        }

    }

    void IncreaseSize()
    {
       transform.localScale = Vector3.Lerp(transform.localScale, EndingSize, 2.5f * Time.deltaTime);
    }
}
