using UnityEngine;
using System.Collections;

public class BasePlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("up") || Input.GetKey("w"))
        {
            transform.Translate(new Vector3(0,0,1));
        }

	}
}
