using UnityEngine;
using System.Collections;

public class StageBounds : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerExit(Collision collision)
    {
        if (collision.transform.tag == "Wowser")
        {
           
        }
    }
    void OnTriggerEnter(Collision collision)
    {
        if (collision.transform.tag == "Wowser")
        {
           // IsFalling = false;
        }
    }
}
